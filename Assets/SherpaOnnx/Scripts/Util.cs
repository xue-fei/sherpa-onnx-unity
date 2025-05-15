using System.IO;
using UnityEngine;

public static class Util
{
    public static string GetPath()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor
            || Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.LinuxEditor
             || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            return Application.streamingAssetsPath;
        }
        else if (Application.platform == RuntimePlatform.Android
                  || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return Application.persistentDataPath;
        }
        else
        {
            return Application.streamingAssetsPath;
        }
    }

    public static void SaveClip(int channels, int frequency, float[] data, string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                // 写入RIFF头部标识
                writer.Write("RIFF".ToCharArray());
                // 写入文件总长度（后续填充）
                writer.Write(0);
                writer.Write("WAVE".ToCharArray());
                // 写入fmt子块
                writer.Write("fmt ".ToCharArray());
                writer.Write(16); // PCM格式块长度
                writer.Write((short)1); // PCM编码类型
                writer.Write((short)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2); // 字节率
                writer.Write((short)(channels * 2)); // 块对齐
                writer.Write((short)16); // 位深度
                                         // 写入data子块
                writer.Write("data".ToCharArray());
                writer.Write(data.Length * 2); // 音频数据字节数
                                               // 写入PCM数据（float转为short）
                foreach (float sample in data)
                {
                    writer.Write((short)(sample * 32767));
                }
                // 返回填充文件总长度
                fileStream.Position = 4;
                writer.Write((int)(fileStream.Length - 8));
            }
        }
    }
}