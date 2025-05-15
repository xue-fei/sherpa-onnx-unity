using SherpaOnnx;
using System.Collections.Generic;
using System.IO;
using uMicrophoneWebGL;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MicrophoneWebGL))]
public class SampleOnlineRecognizer : MonoBehaviour
{
    // 声明配置和识别器变量
    OnlineRecognizer recognizer = null;
    OnlineStream onlineStream = null;

    string tokensPath = "tokens.txt";
    string encoder = "encoder-epoch-99-avg-1.onnx";
    string decoder = "decoder-epoch-99-avg-1.onnx";
    string joiner = "joiner-epoch-99-avg-1.onnx";
    int numThreads = 1;
    string decodingMethod = "modified_beam_search";

    string pathRoot;
    string modelPath;
    int sampleRate = 16000;

    //OfflinePunctuation offlinePunctuation = null;
    VoiceActivityDetector vad = null;
    MicrophoneWebGL microphone;
    public bool initDone = false;

    public InputField inputField;

    public Keyword keyword;
    public SpeakerIdentification speakerIdentification;

    // Start is called before the first frame update
    void Start()
    {
        pathRoot = Util.GetPath();
        microphone = GetComponent<MicrophoneWebGL>();
        //microphone.dataEvent = new DataEvent();
        microphone.dataEvent.AddListener(OnAudioData);
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
        //OfflinePunctuationConfig opc = new OfflinePunctuationConfig();

        //OfflinePunctuationModelConfig opmc = new OfflinePunctuationModelConfig();
        //opmc.CtTransformer = pathRoot + "/sherpa-onnx-punct-ct-transformer-zh-en-vocab272727-2024-04-12/model.onnx";
        //opmc.NumThreads = numThreads;
        //opmc.Provider = "cpu";
        //opmc.Debug = 0;

        //opc.Model = opmc;
        //offlinePunctuation = new OfflinePunctuation(opc);
        #endregion

        #region vad
        VadModelConfig vadModelConfig = new VadModelConfig();

        SileroVadModelConfig SileroVad = new SileroVadModelConfig();
        SileroVad.Model = pathRoot + "/silero_vad.onnx";
        SileroVad.MinSilenceDuration = 0.25f;
        SileroVad.MinSpeechDuration = 0.5f;
        SileroVad.Threshold = 0.5f;
        SileroVad.WindowSize = 512;

        vadModelConfig.SileroVad = SileroVad;
        vadModelConfig.SampleRate = sampleRate;
        vadModelConfig.NumThreads = numThreads;
        vadModelConfig.Provider = "cpu";
        vadModelConfig.Debug = 0;

        vad = new VoiceActivityDetector(vadModelConfig, 60);

        if (keyword != null)
        {
            keyword.Init();
        }
        if(speakerIdentification!=null)
        {
            speakerIdentification.Init();
        }
        initDone = true;
        Loom.QueueOnMainThread(() =>
        {
            Debug.Log("语音转文字初始化完成");
            inputField.text = "init done \n";
        });
        #endregion
    }
     
    public void OnAudioData(float[] data)
    {
        if (!initDone)
        {
            return;
        }

        if (keyword != null)
        {
            keyword.AcceptData(data);
        }

        if (speakerIdentification != null)
        {
            speakerIdentification.AcceptData(data);
        }
        onlineStream.AcceptWaveform(sampleRate, data);
    }

    string lastText = "";
    // Update is called once per frame
    void Update()
    {
        if (initDone)
        { 
            // 每帧更新识别器状态
            if (recognizer.IsReady(onlineStream))
            {
                recognizer.Decode(onlineStream);
            }

            var text = recognizer.GetResult(onlineStream).Text;
            bool isEndpoint = recognizer.IsEndpoint(onlineStream);
            if (!string.IsNullOrWhiteSpace(text) && lastText != text)
            {
                if (string.IsNullOrWhiteSpace(lastText))
                {
                    lastText = text;
                    Debug.Log(lastText.ToLower());
                }
                else
                {

                    Debug.Log(text.Replace(lastText, "").ToLower());
                    lastText = text;
                    inputField.text += lastText + "\n";
                }
            }

            if (isEndpoint)
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    if (keyword != null)
                    {
                        keyword.Recognize();
                    }

                    if (speakerIdentification != null)
                    {
                        speakerIdentification.Search();
                    }
                    Debug.Log(text.ToLower());
                    //Debug.Log(offlinePunctuation.AddPunct(text.ToLower()));
                    inputField.text = text.ToLower() + "\n";
                }
                recognizer.Reset(onlineStream);
            } 
        }
    }

    private void OnApplicationQuit()
    {
        initDone = false;
        if (recognizer != null)
        {
            recognizer.Dispose();
        }
        if (onlineStream != null)
        {
            onlineStream.Dispose();
        }
        //if (offlinePunctuation != null)
        //{
        //    offlinePunctuation.Dispose();
        //}
        if (vad != null)
        {
            vad.Dispose();
        }
    }
}