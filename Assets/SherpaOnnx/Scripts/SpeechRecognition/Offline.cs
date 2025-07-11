using SherpaOnnx;
using System;
using System.IO;
using UnityEngine;

namespace SherpaOnnxUnity
{
    public class Offline : SpeechRecognition
    {
        OfflineRecognizer recognizer = null;
        OfflineStream offlineStream = null;
        string tokensPath = "tokens.txt";
        string paraformer = "model.int8.onnx";
        string decodingMethod = "greedy_search";
        int numThreads = 1;

        string pathRoot;
        string modelPath;
        int sampleRate = 16000;

        OfflinePunctuation offlinePunctuation = null;
        OfflineSpeechDenoiser offlineSpeechDenoiser = null;
        DenoisedAudio denoisedAudio = null;
        string tempAudioPath;

        public bool initDone = false;

        // Start is called before the first frame update
        void Start()
        {
            pathRoot = Util.GetPath() + "/models";
            tempAudioPath = Util.GetPath() + "/temp";
            if (!Directory.Exists(tempAudioPath))
            {
                Directory.CreateDirectory(tempAudioPath);
            }
            Loom.RunAsync(() =>
            {
                Init();
            });
        }

        void Init()
        {
            modelPath = pathRoot + "/sherpa-onnx-paraformer-zh-small-2024-03-09";
            OfflineRecognizerConfig config = new OfflineRecognizerConfig();
            config.FeatConfig.SampleRate = sampleRate;
            config.FeatConfig.FeatureDim = 80;
            config.DecodingMethod = decodingMethod;

            OfflineModelConfig offlineModelConfig = new OfflineModelConfig();
            offlineModelConfig.Tokens = Path.Combine(modelPath, tokensPath);
            offlineModelConfig.NumThreads = numThreads;
            offlineModelConfig.Provider = "cpu";
            offlineModelConfig.Debug = 0;

            OfflineParaformerModelConfig paraformerConfig = new OfflineParaformerModelConfig();
            paraformerConfig.Model = Path.Combine(modelPath, paraformer);

            offlineModelConfig.Paraformer = paraformerConfig;
            config.ModelConfig = offlineModelConfig;

            OfflineLMConfig offlineLMConfig = new OfflineLMConfig();
            offlineLMConfig.Scale = 0.5f;
            config.LmConfig = offlineLMConfig;
            recognizer = new OfflineRecognizer(config);

            #region 添加标点符号
            OfflinePunctuationConfig opc = new OfflinePunctuationConfig();

            OfflinePunctuationModelConfig opmc = new OfflinePunctuationModelConfig();
            opmc.CtTransformer = pathRoot + "/sherpa-onnx-punct-ct-transformer-zh-en-vocab272727-2024-04-12/model.onnx";
            opmc.NumThreads = numThreads;
            opmc.Provider = "cpu";
            opmc.Debug = 0;

            opc.Model = opmc;
            offlinePunctuation = new OfflinePunctuation(opc);
            #endregion

            OfflineSpeechDenoiserGtcrnModelConfig osdgmc = new OfflineSpeechDenoiserGtcrnModelConfig();
            osdgmc.Model = pathRoot + "/gtcrn_simple.onnx";
            OfflineSpeechDenoiserModelConfig osdmc = new OfflineSpeechDenoiserModelConfig();
            osdmc.NumThreads = numThreads;
            osdmc.Provider = "cpu";
            osdmc.Debug = 0;
            osdmc.Gtcrn = osdgmc;
            OfflineSpeechDenoiserConfig osdc = new OfflineSpeechDenoiserConfig();
            osdc.Model = osdmc;
            offlineSpeechDenoiser = new OfflineSpeechDenoiser(osdc);

            initDone = true;
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("文字转语音初始化完成");
            });
        }

        public override void RecognizeOffline(float[] input, Action<string> onResult)
        {
            if (!initDone)
            {
                Debug.Log("Model is not ready yet.");
                return;
            }
            string filePath = tempAudioPath + "/" + DateTime.Now.ToFileTime().ToString() + ".wav";
            // 语音增强
            denoisedAudio = offlineSpeechDenoiser.Run(input, sampleRate); 
            if (denoisedAudio.SaveToWaveFile(filePath))
            {
                Debug.Log("Saved denoised audio to " + filePath);
            }
            byte[] bytes = File.ReadAllBytes(filePath);
            float[] data = Util.BytesToFloat(bytes);

            offlineStream = recognizer.CreateStream();
            offlineStream.AcceptWaveform(sampleRate, data);
            recognizer.Decode(offlineStream);
            string result = offlineStream.Result.Text;
            result = offlinePunctuation.AddPunct(result);
            offlineStream.Dispose();
            if (onResult != null)
            {
                onResult(result);
            }
        }
    }
}