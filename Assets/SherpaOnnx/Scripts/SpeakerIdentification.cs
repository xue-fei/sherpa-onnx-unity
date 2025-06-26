using System;
using System.Collections.Generic;
using System.IO;
using SherpaOnnx;
using UnityEngine;

public class SpeakerIdentification : MonoBehaviour
{
    SpeakerEmbeddingExtractor extractor;
    SpeakerEmbeddingManager manager;
    string pathRoot;
    string modelPath;
    string tempAudioPath;

    OfflineSpeechDenoiser offlineSpeechDenoiser = null;

    string[] testFiles;

    // Start is called before the first frame update
    void Start()
    {
        pathRoot = Util.GetPath() + "/models";
        modelPath = pathRoot + "/3dspeaker_speech_eres2net_base_200k_sv_zh-cn_16k-common.onnx";

        tempAudioPath = Util.GetPath() + "/temp";
        if (!Directory.Exists(tempAudioPath))
        {
            Directory.CreateDirectory(tempAudioPath);
        }
    }

    public void Init()
    {
        OfflineSpeechDenoiserGtcrnModelConfig osdgmc = new OfflineSpeechDenoiserGtcrnModelConfig();
        osdgmc.Model = pathRoot + "/gtcrn_simple.onnx";
        OfflineSpeechDenoiserModelConfig osdmc = new OfflineSpeechDenoiserModelConfig();
        osdmc.NumThreads = 1;
        osdmc.Provider = "cpu";
        osdmc.Debug = 0;
        osdmc.Gtcrn = osdgmc;
        OfflineSpeechDenoiserConfig osdc = new OfflineSpeechDenoiserConfig();
        osdc.Model = osdmc;
        offlineSpeechDenoiser = new OfflineSpeechDenoiser(osdc);

        var config = new SpeakerEmbeddingExtractorConfig();
        config.Model = modelPath;
        config.Debug = 1;
        extractor = new SpeakerEmbeddingExtractor(config);
        manager = new SpeakerEmbeddingManager(extractor.Dim);

        var spk1Files =
            new string[] {
          pathRoot+"/audio/xuefei1.wav",
            };
        var spk1Vec = new float[spk1Files.Length][];

        for (int i = 0; i < spk1Files.Length; ++i)
        {
            spk1Vec[i] = ComputeEmbedding(extractor, spk1Files[i]);
        }

        //注册说话人
        if (!manager.Add("xuefei", spk1Vec))
        {
            Debug.LogError("Failed to register xuefei");
        }

        var allSpeakers = manager.GetAllSpeakers();
        foreach (var s in allSpeakers)
        {
            Debug.Log(s);
        }

        //验证测试
        testFiles =
        new string[] {
          pathRoot+"/audio/test1.wav",
          pathRoot+"/audio/test2.wav",
          pathRoot+"/audio/test3.wav",
        };
        float threshold = 0.6f;
        foreach (var file in testFiles)
        {
            var embedding = ComputeEmbedding(extractor, file);
            var name = manager.Search(embedding, threshold);
            if (name == "")
            {
                name = "<Unknown>";
            }
            Debug.Log(file + " :" + name);
        }
    }

    /// <summary>
    /// 说话人识别 用的临时数据
    /// </summary>
    List<float> audioData = new List<float>();
    public void AcceptData(float[] data)
    {
        audioData.AddRange(data);
    }

    float threshold = 0.6f;

    public void Search(Action<string> callback)
    {
        Loom.RunAsync(() =>
        {
            string filePath = tempAudioPath + "/" + DateTime.Now.ToFileTime().ToString() + ".wav";
            DenoisedAudio denoisedAudio = offlineSpeechDenoiser.Run(audioData.ToArray(), 16000);
            if (denoisedAudio.SaveToWaveFile(filePath))
            {
                Debug.Log("Saved denoised audio to " + filePath);
            }
            var embedding = ComputeEmbedding(extractor, filePath);
            string name = manager.Search(embedding, threshold);
            if (name == "")
            {
                name = "<Unknown>";
            }
            audioData.Clear();
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("name:" + name);
                callback?.Invoke(name);
            });
        });
    }

    public float[] ComputeEmbedding(SpeakerEmbeddingExtractor extractor, string filename)
    {
        byte[] bytes = File.ReadAllBytes(filename);
        float[] data = Util.BytesToFloat(bytes);
        var stream = extractor.CreateStream();
        stream.AcceptWaveform(16000, data);
        stream.InputFinished();
        var embedding = extractor.Compute(stream);
        return embedding;
    }
}