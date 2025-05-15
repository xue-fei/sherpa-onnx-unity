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

    OfflineSpeechDenoiser offlineSpeechDenoiser = null;

    string[] testFiles;

    // Start is called before the first frame update
    void Start()
    {
        pathRoot = Util.GetPath();
        modelPath = pathRoot + "/3dspeaker_speech_eres2net_base_sv_zh-cn_3dspeaker_16k.onnx";

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
        //byte[] bytes = File.ReadAllBytes(pathRoot + "/xuefei.wav");
        //float[] data = BytesToFloat(bytes);
        //DenoisedAudio denoisedAudio = offlineSpeechDenoiser.Run(data, 16000);

        //if (denoisedAudio.SaveToWaveFile(pathRoot + "/xuefei1.wav"))
        //{

        //}

        var config = new SpeakerEmbeddingExtractorConfig();
        config.Model = modelPath;
        config.Debug = 1;
        extractor = new SpeakerEmbeddingExtractor(config);
        manager = new SpeakerEmbeddingManager(extractor.Dim);

        var spk1Files =
            new string[] {
          pathRoot+"/xuefei1.wav",
            };
        var spk1Vec = new float[spk1Files.Length][];

        for (int i = 0; i < spk1Files.Length; ++i)
        {
            spk1Vec[i] = ComputeEmbedding(extractor, spk1Files[i]);
        }

        // 给注册音频降噪一下
        //byte[] bytes = File.ReadAllBytes(pathRoot + "/xuefei1.wav");
        //float[] data = BytesToFloat(bytes);
        //DenoisedAudio denoisedAudio = offlineSpeechDenoiser.Run(data, 16000);
        //if (denoisedAudio.SaveToWaveFile(pathRoot + "/xuefei1.wav"))
        //{

        //}

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
          pathRoot+"/test1.wav",
          pathRoot+"/test2.wav",
          pathRoot+"/test3.wav",
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

    public void Search()
    { 
        string filePath = pathRoot + "/" + DateTime.Now.ToFileTime().ToString() + ".wav";
        //DenoisedAudio denoisedAudio = offlineSpeechDenoiser.Run(audioData.ToArray(), 16000); 
        //if (denoisedAudio.SaveToWaveFile(filePath))
        //{

        //}
        Util.SaveClip(1, 16000, audioData.ToArray(), filePath);
        var embedding = ComputeEmbedding(extractor, filePath);
        string name = manager.Search(embedding, threshold);
        if (name == "")
        {
            name = "<Unknown>";
        }
        Debug.Log("name:" + name);
        audioData.Clear();
    }

    public float[] ComputeEmbedding(SpeakerEmbeddingExtractor extractor, string filename)
    {
        byte[] bytes = File.ReadAllBytes(filename);
        float[] data = BytesToFloat(bytes);
        var stream = extractor.CreateStream();
        stream.AcceptWaveform(16000, data);
        stream.InputFinished();
        var embedding = extractor.Compute(stream);
        return embedding;
    }

    public float[] ComputeEmbedding(SpeakerEmbeddingExtractor extractor, int sample, float[] data)
    {
        var stream = extractor.CreateStream();
        stream.AcceptWaveform(sample, data);
        stream.InputFinished();
        var embedding = extractor.Compute(stream);
        return embedding;
    }

    public float[] BytesToFloat(byte[] byteArray)
    {
        float[] sounddata = new float[byteArray.Length / 2];
        for (int i = 0; i < sounddata.Length; i++)
        {
            sounddata[i] = BytesToFloat(byteArray[i * 2], byteArray[i * 2 + 1]);
        }
        return sounddata;
    }

    private float BytesToFloat(byte firstByte, byte secondByte)
    {
        //小端和大端顺序要调整
        short s;
        if (BitConverter.IsLittleEndian)
            s = (short)((secondByte << 8) | firstByte);
        else
            s = (short)((firstByte << 8) | secondByte);
        // convert to range from -1 to (just below) 1
        return s / 32768.0F;
    }
}