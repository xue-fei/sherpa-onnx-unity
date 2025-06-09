using System.IO;
using SherpaOnnx;
using UnityEngine;

/// <summary>
/// 语种识别
/// </summary>
public class SpokenLanguage : MonoBehaviour
{
    string pathRoot;
    SpokenLanguageIdentification sli = null;
    public bool initDone = false;

    // Start is called before the first frame update
    void Start()
    {
        pathRoot = Util.GetPath() + "/models";
        Loom.RunAsync(() =>
        {
            Init();
        });
    }

    public void Init()
    {
        SpokenLanguageIdentificationConfig config = new SpokenLanguageIdentificationConfig();
        config.Whisper.Encoder = pathRoot + "/sherpa-onnx-whisper-tiny/tiny-encoder.int8.onnx";
        config.Whisper.Decoder = pathRoot + "/sherpa-onnx-whisper-tiny/tiny-decoder.int8.onnx";
        sli = new SpokenLanguageIdentification(config);
        initDone = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!initDone)
            {
                return;
            }
            var filename = pathRoot + "/sherpa-onnx-whisper-tiny/test_wavs/0.wav";
            byte[] bytes = File.ReadAllBytes(filename);
            float[] data = Util.BytesToFloat(bytes);

            OfflineStream offlineStream = sli.CreateStream();
            offlineStream.AcceptWaveform(16000, data);
            Loom.RunAsync(() =>
            {
                var result = sli.Compute(offlineStream);
                Loom.QueueOnMainThread(() =>
                {
                    Debug.LogFormat($"Filename: {filename}");
                    Debug.LogFormat($"Detected language: {result.Lang}");
                });
            });
        }
    }
}