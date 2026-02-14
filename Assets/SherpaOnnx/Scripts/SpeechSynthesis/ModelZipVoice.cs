using AOT;
using SherpaOnnx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SherpaOnnxUnity
{
    [RequireComponent(typeof(AudioSource))]
    public class ModelZipVoice : SpeechSynthesis
    {
        OfflineTts ot;
        OfflineTtsGeneratedAudio otga;
        OfflineTtsConfig config;
        OfflineTtsCallback otc;
        static ModelZipVoice Instance;
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
            pathRoot = Util.GetPath() + "/models";
            pathRoot += "sherpa-onnx-zipvoice-distill-int8-zh-en-emilia/";
            audioSource = GetComponent<AudioSource>();
            Loom.RunAsync(() =>
            {
                Init();
            });
        }

        // Update is called once per frame
        void Update()
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
            Instance = this;
            initDone = false;
            config = new OfflineTtsConfig();
            config.Model.ZipVoice.Encoder = Path.Combine(pathRoot, "encoder.int8.onnx");
            config.Model.ZipVoice.Decoder = Path.Combine(pathRoot, "decoder.int8.onnx");
            config.Model.ZipVoice.Vocoder = Path.Combine(pathRoot, "vocos_24khz.onnx");
            config.Model.ZipVoice.DataDir = Path.Combine(pathRoot, "espeak-ng-data");
            config.Model.ZipVoice.Tokens = Path.Combine(pathRoot, "tokens.txt");
            config.Model.ZipVoice.Lexicon = Path.Combine(pathRoot, "lexicon.txt");
            config.Model.NumThreads = 4;
            config.Model.Debug = 1;
            config.Model.Provider = "cpu";
            config.MaxNumSentences = 1;
            ot = new OfflineTts(config);
            SampleRate = ot.SampleRate;
            otc = new OfflineTtsCallback(OnStaticAudioData);
            initDone = true;
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("文字转语音初始化完成");
            });
        }

        public override void Generate(string text, float speed, int speakerId)
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

        [MonoPInvokeCallback(typeof(OfflineTtsCallback))]
        static int OnStaticAudioData(IntPtr samples, int n)
        {
            return Instance.OnAudioData(samples, n);
        }

        private int OnAudioData(IntPtr samples, int n)
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

        private void OnDestory()
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
}