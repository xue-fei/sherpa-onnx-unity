namespace SherpaOnnx;

public struct FeatureConfig
{
	public int SampleRate;

	public int FeatureDim;

	public FeatureConfig()
	{
		SampleRate = 16000;
		FeatureDim = 80;
	}
}
