using System.IO;
using System;
using SherpaOnnx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using uMicrophoneWebGL;
using System.Collections.Generic;

public class SampleOfflineRecognizer : MonoBehaviour
{
    public Button button;
    public InputField inputField;

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

    MicrophoneWebGL microphone;

    bool isDone = false;
    List<float> buffer = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        UnityAction<BaseEventData> down = new UnityAction<BaseEventData>(PointerDown);
        EventTrigger.Entry eDown = new EventTrigger.Entry();
        eDown.eventID = EventTriggerType.PointerDown;
        eDown.callback.AddListener(down);
        EventTrigger etDown = button.gameObject.AddComponent<EventTrigger>();
        etDown.triggers.Add(eDown);

        UnityAction<BaseEventData> up = new UnityAction<BaseEventData>(PointerUp);
        EventTrigger.Entry eUp = new EventTrigger.Entry();
        eUp.eventID = EventTriggerType.PointerUp;
        eUp.callback.AddListener(up);
        EventTrigger etUp = button.gameObject.AddComponent<EventTrigger>();
        etUp.triggers.Add(eUp);

        microphone = GetComponent<MicrophoneWebGL>();
        microphone.isAutoStart = false;
        microphone.dataEvent.AddListener(OnData);

        Loom.RunAsync(() =>
        {
            Init();
        });
    }

    void Init()
    {
        pathRoot = Util.GetPath();
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

        isDone = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PointerDown(BaseEventData data)
    {
        Debug.LogWarning("按下");
        if (!isDone)
        {
            Debug.LogWarning("not ok");
            return;
        }
        buffer.Clear();
        microphone.Begin();
    }

    void PointerUp(BaseEventData data)
    {
        Debug.LogWarning("抬起");
        if (!isDone)
        {
            Debug.LogWarning("not ok");
            return;
        }
        microphone.End();
        Recognize(buffer.ToArray());
    }

    void OnData(float[] input)
    {
        buffer.AddRange(input);
    }

    void Recognize(float[] input)
    {
        // 语音增强
        denoisedAudio = offlineSpeechDenoiser.Run(input, sampleRate);
        input = denoisedAudio.Samples;

        offlineStream = recognizer.CreateStream();
        offlineStream.AcceptWaveform(sampleRate, input);
        recognizer.Decode(offlineStream);
        string result = offlineStream.Result.Text;
        offlineStream.Dispose();
        inputField.text = offlinePunctuation.AddPunct(result.ToLower());
        Debug.Log("识别结果:" + result);
    }
}