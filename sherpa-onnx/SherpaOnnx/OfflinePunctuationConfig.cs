namespace SherpaOnnx;

public struct OfflinePunctuationConfig
{
	public OfflinePunctuationModelConfig Model;

	public OfflinePunctuationConfig()
	{
		Model = new OfflinePunctuationModelConfig();
	}
}
