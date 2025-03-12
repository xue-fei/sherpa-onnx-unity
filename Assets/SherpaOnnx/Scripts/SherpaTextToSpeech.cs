using SherpaOnnx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SherpaTextToSpeech : MonoBehaviour
{
    OfflineTts ot;
    OfflineTtsGeneratedAudio otga;
    OfflineTtsConfig config;
    OfflineTtsCallback otc;
    AudioSource audioSource;
    int SampleRate = 22050;
    AudioClip audioClip = null;
    List<float> audioData = new List<float>();
    /// <summary>
	/// 当前要读取的索引位置
	/// </summary>
	int curAudioClipPos = 0;

    public bool initDone = false;

    public float audioLength = 0f;
    public Action OnAudioEnd;
    string pathRoot;

    // Start is called before the first frame update
    void Start()
    {
        pathRoot = Util.GetPath();
        audioSource = GetComponent<AudioSource>();
        Loom.RunAsync(() =>
        {
            Init();
        });
    }

    private void Update()
    {
        if (audioLength > 0)
        {
            audioLength -= Time.deltaTime;
            if (audioLength < 0)
            {
                audioLength = 0;
                Debug.Log("音频播放完毕");
                if (OnAudioEnd != null)
                {
                    OnAudioEnd();
                }
            }
        }
    }

    void Init()
    {
        initDone = false;
        config = new OfflineTtsConfig();
        config.Model.Vits.Model = Path.Combine(pathRoot, "vits-melo-tts-zh_en/model.onnx");
        config.Model.Vits.Lexicon = Path.Combine(pathRoot, "vits-melo-tts-zh_en/lexicon.txt");
        config.Model.Vits.Tokens = Path.Combine(pathRoot, "vits-melo-tts-zh_en/tokens.txt");
        config.Model.Vits.DictDir = Path.Combine(pathRoot, "vits-melo-tts-zh_en/dict");
        config.Model.Vits.NoiseScale = 0.667f;
        config.Model.Vits.NoiseScaleW = 0.8f;
        config.Model.Vits.LengthScale = 1f;
        config.Model.NumThreads = 5;
        config.Model.Debug = 1;
        config.Model.Provider = "gpu";
        config.RuleFsts = pathRoot + "/vits-melo-tts-zh_en/phone.fst" + ","
                    + pathRoot + "/vits-melo-tts-zh_en/date.fst" + ","
                + pathRoot + "/vits-melo-tts-zh_en/number.fst";
        config.MaxNumSentences = 1;
        ot = new OfflineTts(config);
        SampleRate = ot.SampleRate;
        otc = new OfflineTtsCallback(OnAudioData);
        initDone = true;
        Loom.QueueOnMainThread(() =>
        {
            Debug.Log("文字转语音初始化完成");
        });
    }

    public void Generate(string text, float speed, int speakerId)
    {
        if (!initDone)
        {
            Debug.LogWarning("文字转语音未完成初始化");
            return;
        }
        Loom.RunAsync(() =>
        {
            otga = ot.GenerateWithCallback(text, speed, speakerId, otc);
        });
    }

    int OnAudioData(IntPtr samples, int n)
    {
        float[] tempData = new float[n];
        Marshal.Copy(samples, tempData, 0, n);
        audioData.AddRange(tempData);
        Loom.QueueOnMainThread(() =>
        {
            Debug.Log("n:" + n);
            audioLength += (float)n / (float)SampleRate;
            Debug.Log("音频长度增加 " + (float)n / (float)SampleRate + "秒");

            if (!audioSource.isPlaying && audioData.Count > SampleRate * 2)
            {
                audioClip = AudioClip.Create("SynthesizedAudio", SampleRate * 2, 1,
                    SampleRate, true, (float[] data) =>
                    {
                        ExtractAudioData(data);
                    });
                audioSource.clip = audioClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        });
        return n;
    }

    bool ExtractAudioData(float[] data)
    {
        if (data == null || data.Length == 0)
        {
            return false;
        }
        bool hasData = false;//是否真的读取到数据
        int dataIndex = 0;//当前要写入的索引位置
        if (audioData != null && audioData.Count > 0)
        {
            while (curAudioClipPos < audioData.Count && dataIndex < data.Length)
            {
                data[dataIndex] = audioData[curAudioClipPos];
                curAudioClipPos++;
                dataIndex++;
                hasData = true;
            }
        }

        //剩余部分填0
        while (dataIndex < data.Length)
        {
            data[dataIndex] = 0;
            dataIndex++;
        }
        return hasData;
    }

    private void OnApplicationQuit()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        if (ot != null)
        {
            ot.Dispose();
        }
        if (otc != null)
        {
            otc = null;
        }
        if (otga != null)
        {
            otga.Dispose();
        }
    }
}