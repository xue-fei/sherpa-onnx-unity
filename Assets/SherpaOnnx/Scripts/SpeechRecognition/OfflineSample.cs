using SherpaOnnxUnity;
using System.Collections.Generic;
using uMicrophoneWebGL;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(MicrophoneWebGL))]
public class OfflineSample : MonoBehaviour
{
    public Button button;
    public InputField inputField;
    MicrophoneWebGL microphone;
    public SpeechRecognition speechRecognition;
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
    }

    void OnData(float[] input)
    {
        buffer.AddRange(input);
    }

    void PointerDown(BaseEventData data)
    {
        Debug.LogWarning("按下");
        buffer.Clear();
        microphone.Begin();
    }

    void PointerUp(BaseEventData data)
    {
        Debug.LogWarning("抬起");
        microphone.End();
        speechRecognition.RecognizeOffline(buffer.ToArray(), OnResult);
    }

    void OnResult(string result)
    {
        inputField.text = result;
    }
}