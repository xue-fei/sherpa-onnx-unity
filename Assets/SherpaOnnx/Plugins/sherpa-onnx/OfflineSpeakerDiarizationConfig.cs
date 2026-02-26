/// Copyright (c)  2024  Xiaomi Corporation

using System.Runtime.InteropServices;

namespace SherpaOnnx
{

    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineSpeakerDiarizationConfig
    {
        public static OfflineSpeakerDiarizationConfig Creat()
        {
            return new OfflineSpeakerDiarizationConfig
            {
                Segmentation = OfflineSpeakerSegmentationModelConfig.Creat(),
                Embedding = SpeakerEmbeddingExtractorConfig.Creat(),
                Clustering = FastClusteringConfig.Create(),

                MinDurationOn = 0.3F,
                MinDurationOff = 0.5F,
            };
        }

        public OfflineSpeakerSegmentationModelConfig Segmentation;
        public SpeakerEmbeddingExtractorConfig Embedding;
        public FastClusteringConfig Clustering;

        public float MinDurationOn;
        public float MinDurationOff;
    }
}