using uMicrophoneWebGL;
using UnityEngine;

[RequireComponent(typeof(MicrophoneWebGL))]
public class KeywordSpottingSample : MonoBehaviour
{
    MicrophoneWebGL microphone;
    public KeywordSpotting keywordSpotting;

    // Start is called before the first frame update
    void Start()
    {
        keywordSpotting.Init();
        microphone = GetComponent<MicrophoneWebGL>();
        microphone.dataEvent.AddListener(OnAudioData);
    }

    public void OnAudioData(float[] data)
    {
        if (keywordSpotting != null)
        {
            keywordSpotting.AcceptData(data);
        }
    }

    float timer = 0f;
    float interval = 0.2f;
    string keyword;
    private void Update()
    {
        if (keywordSpotting != null && keywordSpotting.initDone)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                keyword = keywordSpotting.Recognize();
                if (!string.IsNullOrEmpty(keyword))
                {
                    Debug.Log("keyword:" + keyword);
                } 
                timer = 0f;
            } 
        }
    }
}