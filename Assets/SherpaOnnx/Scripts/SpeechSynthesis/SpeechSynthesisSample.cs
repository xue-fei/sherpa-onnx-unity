using SherpaOnnxUnity;
using UnityEngine;
using UnityEngine.UI;

public class SpeechSynthesisSample : MonoBehaviour
{
    public SpeechSynthesis speechSynthesis;
    public InputField inputFieldSpeakerId;
    public InputField inputFieldContent;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(inputFieldContent.text))
            {
                return;
            }
            if (string.IsNullOrEmpty(inputFieldSpeakerId.text))
            {
                return;
            }
            int id = 0;
            int.TryParse(inputFieldSpeakerId.text, out id);
            speechSynthesis.Generate(inputFieldContent.text, 1.0f, id);
        });
    }
}