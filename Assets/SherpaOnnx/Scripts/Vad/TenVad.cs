using SherpaOnnx;
using System.Collections.Generic;
using uMicrophoneWebGL;
using UnityEngine;

public class TenVad : MonoBehaviour
{
    string pathRoot;
    string modelPath;
    VoiceActivityDetector vad;
    MicrophoneWebGL microphone;

    // Start is called before the first frame update
    void Start()
    {
        pathRoot = Util.GetPath() + "/models";
        modelPath = pathRoot + "/ten-vad.onnx";
        var vadModelConfig = new VadModelConfig();
        vadModelConfig.TenVad.Model = modelPath;
        vadModelConfig.TenVad.Threshold = 0.3F;
        vadModelConfig.TenVad.MinSilenceDuration = 0.5F;
        vadModelConfig.TenVad.MinSpeechDuration = 0.25F;
        vadModelConfig.TenVad.MaxSpeechDuration = 5.0F;
        vadModelConfig.TenVad.WindowSize = 256;
        vadModelConfig.Debug = 0;
        vad = new VoiceActivityDetector(vadModelConfig, 60);

        microphone = GetComponent<MicrophoneWebGL>();
        microphone.dataEvent.AddListener(OnAudioData);
    }

    // Update is called once per frame
    void Update()
    {

    }

    float[] tempData = null;
    float timeStart;
    List<float> record = new List<float>();
    bool isSpeechDetected = false;
    void OnAudioData(float[] data)
    {
        vad.AcceptWaveform(data);
        if (vad.IsSpeechDetected())
        {
            if (tempData != null)
            {
                record.AddRange(tempData);
                tempData = null;
            }
            record.AddRange(data);
            if (!isSpeechDetected)
            {
                timeStart = Time.time;
                isSpeechDetected = true;
                Debug.LogWarning("有人讲话");
            }
        }
        else
        {
            tempData = data;
            if (isSpeechDetected)
            {
                Debug.LogWarning("时长:" + (Time.time - timeStart));
                isSpeechDetected = false;
                Debug.LogWarning("无人语我");
            }
        }
    }

    private void OnDestroy()
    {
        if (vad != null)
        {
            vad.Dispose();
        }
        Util.SaveClip(1, 16000, record.ToArray(), Application.streamingAssetsPath + "/record.wav");
    }
}