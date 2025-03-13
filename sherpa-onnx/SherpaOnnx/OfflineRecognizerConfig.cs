using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineRecognizerConfig
{
	public FeatureConfig FeatConfig;

	public OfflineModelConfig ModelConfig;

	public OfflineLMConfig LmConfig;

	[MarshalAs(UnmanagedType.LPStr)]
	public string DecodingMethod;

	public int MaxActivePaths;

	[MarshalAs(UnmanagedType.LPStr)]
	public string HotwordsFile;

	public float HotwordsScore;

	[MarshalAs(UnmanagedType.LPStr)]
	public string RuleFsts;

	[MarshalAs(UnmanagedType.LPStr)]
	public string RuleFars;

	public float BlankPenalty;

	public OfflineRecognizerConfig()
	{
		FeatConfig = new FeatureConfig();
		ModelConfig = new OfflineModelConfig();
		LmConfig = new OfflineLMConfig();
		DecodingMethod = "greedy_search";
		MaxActivePaths = 4;
		HotwordsFile = "";
		HotwordsScore = 1.5f;
		RuleFsts = "";
		RuleFars = "";
		BlankPenalty = 0f;
	}
}
