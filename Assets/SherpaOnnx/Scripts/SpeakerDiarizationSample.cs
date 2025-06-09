using System.IO;
using SherpaOnnx;
using UnityEngine;

public class SpeakerDiarizationSample : MonoBehaviour
{
    string pathRoot;
    string filename = "0-four-speakers-zh.wav";
    public SpeakerDiarization speakerDiarization;

    // Start is called before the first frame update
    void Start()
    {
        pathRoot = Util.GetPath() + "/models";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            byte[] bytes = File.ReadAllBytes(pathRoot + "/audio/" + filename);
            float[] data = Util.BytesToFloat(bytes);
            Loom.RunAsync(() =>
            {
                speakerDiarization.Diarization(data, OnProgress, OnResult);
            });
        }
    }

    void OnProgress(float progress)
    {
        Debug.Log("Progress: " + progress);
    }

    void OnResult(OfflineSpeakerDiarizationSegment[] osdss)
    {
        foreach (var osds in osdss)
        {
            Debug.LogFormat("{0} -- {1} speaker_{2}", string.Format("{0:0.00}", osds.Start), string.Format("{0:0.00}", osds.End), osds.Speaker);
        }
    }
}