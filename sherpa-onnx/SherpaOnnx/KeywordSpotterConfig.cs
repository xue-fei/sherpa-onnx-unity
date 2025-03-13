using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct KeywordSpotterConfig
{
	public FeatureConfig FeatConfig;

	public OnlineModelConfig ModelConfig;

	public int MaxActivePaths;

	public int NumTrailingBlanks;

	public float KeywordsScore;

	public float KeywordsThreshold;

	[MarshalAs(UnmanagedType.LPStr)]
	public string KeywordsFile;

	[MarshalAs(UnmanagedType.LPStr)]
	public string KeywordsBuf;

	public int KeywordsBufSize;

	public KeywordSpotterConfig()
	{
		FeatConfig = new FeatureConfig();
		ModelConfig = new OnlineModelConfig();
		MaxActivePaths = 4;
		NumTrailingBlanks = 1;
		KeywordsScore = 1f;
		KeywordsThreshold = 0.25f;
		KeywordsFile = "";
		KeywordsBuf = "";
		KeywordsBufSize = 0;
	}
}
