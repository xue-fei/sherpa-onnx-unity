namespace SherpaOnnx;

public struct OfflineSpeakerDiarizationConfig
{
	public OfflineSpeakerSegmentationModelConfig Segmentation;

	public SpeakerEmbeddingExtractorConfig Embedding;

	public FastClusteringConfig Clustering;

	public float MinDurationOn;

	public float MinDurationOff;

	public OfflineSpeakerDiarizationConfig()
	{
		Segmentation = new OfflineSpeakerSegmentationModelConfig();
		Embedding = new SpeakerEmbeddingExtractorConfig();
		Clustering = new FastClusteringConfig();
		MinDurationOn = 0.3f;
		MinDurationOff = 0.5f;
	}
}
