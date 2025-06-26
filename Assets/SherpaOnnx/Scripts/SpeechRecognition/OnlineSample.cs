using SherpaOnnxUnity;
using uMicrophoneWebGL;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MicrophoneWebGL))]
public class OnlineSample : MonoBehaviour
{
    public SpeechRecognition speechRecognition;
    MicrophoneWebGL microphone;
    public InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        microphone = GetComponent<MicrophoneWebGL>();
        microphone.dataEvent.AddListener(OnAudioData);

        if (speechRecognition != null)
        {
            speechRecognition.onResult += OnResult;
            speechRecognition.onResultEnd += OnResultEnd;
        }
    }

    public void OnAudioData(float[] data)
    {
        if (speechRecognition != null)
        {
            speechRecognition.RecognizeOnline(16000, data);
        }
    }

    public void OnResult(string result)
    {
        inputField.text = result;
    }

    public void OnResultEnd(string result)
    {
        inputField.text = result;
    }
}