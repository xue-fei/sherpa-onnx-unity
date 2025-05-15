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

    // Start is called before the first frame update
    void Start()
    {
        pathRoot = Util.GetPath();
        modelPath = pathRoot + "/3dspeaker_speech_campplus_sv_zh-cn_16k-common.onnx";
        
    }

    public void Init()
    {
        var config = new SpeakerEmbeddingExtractorConfig();
        config.Model = modelPath;
        config.Debug = 1;
        extractor = new SpeakerEmbeddingExtractor(config);
        manager = new SpeakerEmbeddingManager(extractor.Dim);

        var spk1Files =
            new string[] {
          pathRoot+"/xuefei.wav",
            };
        var spk1Vec = new float[spk1Files.Length][];

        for (int i = 0; i < spk1Files.Length; ++i)
        {
            spk1Vec[i] = ComputeEmbedding(extractor, spk1Files[i]);
        }
        if (!manager.Add("xuefei", spk1Vec))
        {
            Debug.LogError("Failed to register xuefei");
        }

        var allSpeakers = manager.GetAllSpeakers();
        foreach (var s in allSpeakers)
        {
            Debug.Log(s);
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
    public void Verify(string filePath)
    {
        if (!manager.Verify("xuefei", ComputeEmbedding(extractor, filePath), threshold))
        {
            Debug.Log("should match xuefei!");
            return;
        } 
    }

    public void Verify()
    {
        if (!manager.Verify("xuefei", ComputeEmbedding(extractor,16000, audioData.ToArray()), threshold))
        {
            Debug.Log("should match xuefei!");
            audioData.Clear();
            return;
        }
        audioData.Clear();
    }

    public float[] ComputeEmbedding(SpeakerEmbeddingExtractor extractor, string filename)
    {
        byte[] bytes = File.ReadAllBytes(filename);
        float[] data = BytesToFloat(bytes);
        var stream = extractor.CreateStream();
        stream.AcceptWaveform(48000, data);
        stream.InputFinished(); 
        var embedding = extractor.Compute(stream); 
        return embedding;
    }

    public float[] ComputeEmbedding(SpeakerEmbeddingExtractor extractor,int sample, float[] data)
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
