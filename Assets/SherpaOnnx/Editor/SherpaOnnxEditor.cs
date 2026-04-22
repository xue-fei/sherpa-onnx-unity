using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using SharpCompress.Readers;

public class SherpaOnnxEditor : EditorWindow
{
    private const string BASE_PATH = "Assets/StreamingAssets/models";

    private string downloadUrl = "https://example.com/model.tar.bz2";
    private bool isDownloading = false;
    private bool isExtracting = false;
    private float downloadProgress = 0f;
    private string statusMessage = "";

    private WebClient webClient;

    [MenuItem("Tools/SherpaOnnx Downloader")]
    public static void ShowWindow()
    {
        GetWindow<SherpaOnnxEditor>("SherpaOnnx Downloader");
    }

    private void OnGUI()
    {
        GUILayout.Label("SherpaOnnx Model Downloader", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("Download URL:", EditorStyles.label);
        downloadUrl = EditorGUILayout.TextField(downloadUrl);

        GUILayout.Space(5);

        EditorGUILayout.HelpBox(
            $"Files will be saved to:\n{BASE_PATH}\nExtracted to subfolder named after the archive.",
            MessageType.Info
        );

        GUILayout.Space(15);

        EditorGUI.BeginDisabledGroup(isDownloading || isExtracting);
        if (GUILayout.Button("Download & Extract", GUILayout.Height(30)))
        {
            DownloadAndExtractAsync();
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(5);

        EditorGUI.BeginDisabledGroup(!isDownloading && !isExtracting);
        if (GUILayout.Button("Cancel", GUILayout.Height(20)))
        {
            CancelOperation();
        }
        EditorGUI.EndDisabledGroup();

        // ✅ 进度条
        if (isDownloading || isExtracting)
        {
            GUILayout.Space(5);
            Rect progressRect = EditorGUILayout.GetControlRect(GUILayout.Height(20));
            EditorGUI.ProgressBar(progressRect, downloadProgress, "");
        }

        // ✅ 状态信息
        if (!string.IsNullOrEmpty(statusMessage))
        {
            GUILayout.Space(5);
            MessageType msgType = statusMessage.Contains("failed") || statusMessage.Contains("cancelled")
                ? MessageType.Error
                : statusMessage.Contains("successfully")
                    ? MessageType.Info
                    : MessageType.None;
            EditorGUILayout.HelpBox(statusMessage, msgType);
        }
    }

    private async void DownloadAndExtractAsync()
    {
        isDownloading = true;
        downloadProgress = 0f;
        statusMessage = "Starting download...";

        string fileName = GetFileNameFromUrl(downloadUrl);
        if (string.IsNullOrEmpty(fileName))
            fileName = "downloaded_model.tar.bz2";

        string absoluteBasePath = GetAbsolutePath(BASE_PATH);
        string absoluteSavePath = Path.Combine(absoluteBasePath, fileName);

        if (!Directory.Exists(absoluteBasePath))
            Directory.CreateDirectory(absoluteBasePath);

        if (File.Exists(absoluteSavePath))
        {
            Debug.Log($"File already exists: {absoluteSavePath}. Skipping download.");
            isDownloading = false;
            isExtracting = true;
            statusMessage = "File already exists. Extracting...";
        }
        else
        {
            bool downloadSuccess = await DownloadFileAsync(downloadUrl, absoluteSavePath);

            if (!downloadSuccess)
            {
                isDownloading = false;
                statusMessage = "Download failed or cancelled.";
                Repaint();
                return;
            }

            isDownloading = false;
            isExtracting = true;
            downloadProgress = 0f;
            statusMessage = "Download complete. Extracting...";
        }

        string extractFolderName = Path.GetFileNameWithoutExtension(fileName);
        if (extractFolderName.EndsWith(".tar"))
            extractFolderName = Path.GetFileNameWithoutExtension(extractFolderName);

        string absoluteExtractPath = Path.Combine(absoluteBasePath, extractFolderName);

        bool extractSuccess = await ExtractTarBz2Async(absoluteSavePath, absoluteExtractPath);

        isExtracting = false;

        if (extractSuccess)
        {
            statusMessage = "Extraction completed successfully!";
            downloadProgress = 1f;
            AssetDatabase.Refresh();
        }
        else
        {
            statusMessage = "Extraction failed.";
        }

        Repaint();
    }

    private async Task<bool> DownloadFileAsync(string url, string destinationPath)
    {
        try
        {
            using (webClient = new WebClient())
            {
                var tcs = new TaskCompletionSource<bool>();

                webClient.DownloadProgressChanged += (sender, e) =>
                {
                    downloadProgress = e.TotalBytesToReceive > 0
                        ? (float)e.BytesReceived / e.TotalBytesToReceive
                        : 0f;
                    statusMessage = $"Downloading... {e.BytesReceived / 1024} KB / {e.TotalBytesToReceive / 1024} KB";
                    Repaint();
                };

                webClient.DownloadFileCompleted += (sender, e) =>
                {
                    if (e.Cancelled)
                        tcs.SetResult(false);
                    else if (e.Error != null)
                    {
                        Debug.LogError($"Download error: {e.Error.Message}");
                        tcs.SetResult(false);
                    }
                    else
                        tcs.SetResult(true);
                };

                webClient.DownloadFileAsync(new Uri(url), destinationPath);
                return await tcs.Task;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Download exception: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> ExtractTarBz2Async(string archivePath, string destinationPath)
    {
        return await Task.Run(() =>
        {
            try
            {
                if (!Directory.Exists(destinationPath))
                    Directory.CreateDirectory(destinationPath);

                FileInfo fi = new FileInfo(archivePath);
                if (!fi.Exists || fi.Length == 0)
                {
                    Debug.LogError($"Archive file missing or empty: {archivePath}");
                    return false;
                }

                // ✅ 第一遍：统计解压后所有文件的原始总字节数
                long totalUncompressedBytes = 0;
                using (Stream stream = File.OpenRead(archivePath))
                using (var reader = ReaderFactory.OpenReader(stream))
                {
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory && reader.Entry.Size > 0)
                            totalUncompressedBytes += reader.Entry.Size;
                    }
                }

                // 若无法获取 Size 信息则回退到文件数量计数
                bool useSizeProgress = totalUncompressedBytes > 0;
                int totalFileCount = 0;
                if (!useSizeProgress)
                {
                    using (Stream stream = File.OpenRead(archivePath))
                    using (var reader = ReaderFactory.OpenReader(stream))
                    {
                        while (reader.MoveToNextEntry())
                            if (!reader.Entry.IsDirectory) totalFileCount++;
                    }
                }

                // ✅ 第二遍：实际解压并用原始字节数计算进度
                long extractedBytes = 0;
                int extractedFileCount = 0;

                using (Stream stream = File.OpenRead(archivePath))
                using (var reader = ReaderFactory.OpenReader(stream))
                {
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            string entryKey = reader.Entry.Key.Replace('\\', '/');
                            int firstSlash = entryKey.IndexOf('/');
                            string relativePath = firstSlash >= 0
                                ? entryKey.Substring(firstSlash + 1)
                                : entryKey;

                            if (string.IsNullOrEmpty(relativePath)) continue;

                            string outputPath = Path.Combine(destinationPath, relativePath);
                            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                            using (var entryStream = reader.OpenEntryStream())
                            using (var fileStream = File.Create(outputPath))
                            {
                                byte[] buffer = new byte[8192];
                                int bytesRead;
                                while ((bytesRead = entryStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    fileStream.Write(buffer, 0, bytesRead);
                                    extractedBytes += bytesRead;
                                }
                            }

                            extractedFileCount++;

                            // ✅ 用解压后原始字节数计算进度，数据来源一致
                            float progress = useSizeProgress
                                ? Mathf.Clamp01((float)extractedBytes / totalUncompressedBytes)
                                : Mathf.Clamp01((float)extractedFileCount / totalFileCount);

                            string entryName = Path.GetFileName(relativePath);
                            EditorApplication.delayCall += () =>
                            {
                                downloadProgress = progress;
                                statusMessage = $"Extracting ({(progress * 100):F1}%): {entryName}";
                                Repaint();
                            };
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Extraction error: {ex.Message}");
                return false;
            }
        });
    }
    private void CancelOperation()
    {
        if (webClient != null && webClient.IsBusy)
            webClient.CancelAsync();

        isDownloading = false;
        isExtracting = false;
        statusMessage = "Operation cancelled.";
        downloadProgress = 0f;
        Repaint();
    }

    private string GetAbsolutePath(string relativePath)
    {
        if (Path.IsPathRooted(relativePath))
            return relativePath;

        string projectPath = Application.dataPath.Replace("/Assets", "").Replace("\\Assets", "");
        return Path.Combine(projectPath, relativePath);
    }

    private string GetFileNameFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return null;

        try
        {
            Uri uri = new Uri(url);
            string path = uri.AbsolutePath;
            return Path.GetFileName(path);
        }
        catch
        {
            string[] parts = url.Split('/');
            string lastPart = parts[parts.Length - 1];
            int queryIndex = lastPart.IndexOf('?');
            if (queryIndex > 0)
                lastPart = lastPart.Substring(0, queryIndex);
            return lastPart;
        }
    }

    private void OnDestroy()
    {
        CancelOperation();
        webClient?.Dispose();
        webClient = null;
    }
}