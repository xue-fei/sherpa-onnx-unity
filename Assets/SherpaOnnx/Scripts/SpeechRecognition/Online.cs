using System.IO;
using SherpaOnnx;
using UnityEngine;

namespace SherpaOnnxUnity
{
    public class Online : SpeechRecognition
    {
        OnlineRecognizer recognizer = null;
        OnlineStream onlineStream = null;
        string tokensPath = "tokens.txt";
        string encoder = "encoder-epoch-99-avg-1.onnx";
        string decoder = "decoder-epoch-99-avg-1.onnx";
        string joiner = "joiner-epoch-99-avg-1.onnx";
        int numThreads = 1;
        string decodingMethod = "greedy_search";
        string pathRoot;
        string modelPath;
        int sampleRate = 16000;
        OfflinePunctuation offlinePunctuation = null;
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

        void Init()
        {
            modelPath = pathRoot + "/sherpa-onnx-streaming-zipformer-bilingual-zh-en-2023-02-20";
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }
            // 初始化配置
            OnlineRecognizerConfig config = new OnlineRecognizerConfig();
            config.FeatConfig.SampleRate = sampleRate;
            config.FeatConfig.FeatureDim = 80;
            config.ModelConfig.Transducer.Encoder = Path.Combine(modelPath, encoder);
            config.ModelConfig.Transducer.Decoder = Path.Combine(modelPath, decoder);
            config.ModelConfig.Transducer.Joiner = Path.Combine(modelPath, joiner);
            config.ModelConfig.Tokens = Path.Combine(modelPath, tokensPath);
            config.ModelConfig.Debug = 0;
            config.DecodingMethod = decodingMethod;
            config.EnableEndpoint = 1;

            // 创建识别器和在线流
            recognizer = new OnlineRecognizer(config);
            onlineStream = recognizer.CreateStream();

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

            initDone = true;
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("语音转文字初始化完成");
            });
        }

        public override void RecognizeOnline(int sampleRate, float[] input)
        {
            if (!initDone)
            {
                return;
            }
            onlineStream.AcceptWaveform(sampleRate, input);
        }

        string temp = "";
        string lastText = "";
        bool isEndpoint = false;
        void Update()
        {
            if (!initDone)
            {
                return;
            }
            if (recognizer.IsReady(onlineStream))
            {
                recognizer.Decode(onlineStream);
            }
            temp = recognizer.GetResult(onlineStream).Text;
            isEndpoint = recognizer.IsEndpoint(onlineStream);
            if (!string.IsNullOrWhiteSpace(temp) && lastText != temp)
            {
                if (string.IsNullOrWhiteSpace(lastText))
                {
                    lastText = temp;
                    Debug.Log(lastText.ToLower());
                }
                else
                {
                    lastText = temp;
                    if (onResult != null)
                    {
                        onResult(lastText);
                    }
                }
            }

            if (isEndpoint)
            {
                if (!string.IsNullOrWhiteSpace(temp))
                {
                    temp = offlinePunctuation.AddPunct(temp);
                    if (onResultEnd != null)
                    {
                        onResultEnd(temp);
                    }
                }
                recognizer.Reset(onlineStream);
            }
        }
    }
}