using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OnlineRecognizerConfig
{
	public FeatureConfig FeatConfig;

	public OnlineModelConfig ModelConfig;

	[MarshalAs(UnmanagedType.LPStr)]
	public string DecodingMethod;

	public int MaxActivePaths;

	public int EnableEndpoint;

	public float Rule1MinTrailingSilence;

	public float Rule2MinTrailingSilence;

	public float Rule3MinUtteranceLength;

	[MarshalAs(UnmanagedType.LPStr)]
	public string HotwordsFile;

	public float HotwordsScore;

	public OnlineCtcFstDecoderConfig CtcFstDecoderConfig;

	[MarshalAs(UnmanagedType.LPStr)]
	public string RuleFsts;

	[MarshalAs(UnmanagedType.LPStr)]
	public string RuleFars;

	public float BlankPenalty;

	[MarshalAs(UnmanagedType.LPStr)]
	public string HotwordsBuf;

	public int HotwordsBufSize;

	public OnlineRecognizerConfig()
	{
		FeatConfig = new FeatureConfig();
		ModelConfig = new OnlineModelConfig();
		DecodingMethod = "greedy_search";
		MaxActivePaths = 4;
		EnableEndpoint = 0;
		Rule1MinTrailingSilence = 1.2f;
		Rule2MinTrailingSilence = 2.4f;
		Rule3MinUtteranceLength = 20f;
		HotwordsFile = "";
		HotwordsScore = 1.5f;
		CtcFstDecoderConfig = new OnlineCtcFstDecoderConfig();
		RuleFsts = "";
		RuleFars = "";
		BlankPenalty = 0f;
		HotwordsBuf = "";
		HotwordsBufSize = 0;
	}
}
