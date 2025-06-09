using System;
using SherpaOnnx;
using UnityEngine;

/// <summary>
/// 说话人日志 谁在何时说话
/// </summary>
public class SpeakerDiarization : MonoBehaviour
{
    string pathRoot;
    OfflineSpeakerDiarization osd = null;
    Action<float> onProgress;
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

    public void Init()
    {
        OfflineSpeakerDiarizationConfig osdc = new OfflineSpeakerDiarizationConfig();
        osdc.Segmentation.Pyannote.Model = pathRoot + "/sherpa-onnx-pyannote-segmentation-3-0/model.onnx";
        osdc.Embedding.Model = pathRoot + "/3dspeaker_speech_eres2net_base_sv_zh-cn_3dspeaker_16k.onnx";
        osdc.Clustering.NumClusters = 4;
        osd = new OfflineSpeakerDiarization(osdc);
        initDone = true;
    }

    public void Diarization(float[] data, Action<float> progress, Action<OfflineSpeakerDiarizationSegment[]> result)
    {
        if (!initDone)
        {
            return;
        }
        this.onProgress = progress;
        OfflineSpeakerDiarizationProgressCallback osdpc = new OfflineSpeakerDiarizationProgressCallback(progressCallback);
        OfflineSpeakerDiarizationSegment[] osdss = osd.ProcessWithCallback(data, osdpc, IntPtr.Zero);
        if (osdss != null && osdss.Length > 0)
        {
            if (result != null)
            {
                result(osdss);
            }
        } 
    }

    int progressCallback(int numProcessedChunks, int numTotalChunks, IntPtr arg)
    {
        float progress = 1f * numProcessedChunks / numTotalChunks;
        if (onProgress != null)
        {
            onProgress(progress);
        }
        return 0;
    }
}