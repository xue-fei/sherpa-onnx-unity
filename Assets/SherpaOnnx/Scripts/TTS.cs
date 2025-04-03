using UnityEngine;
using UnityEngine.UI;

public class TTS : MonoBehaviour
{
    public SherpaTextToSpeech stts;
    public InputField inputField;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                return;
            }
            stts.Generate(inputField.text, 1.0f, 0);
        });
    }
}