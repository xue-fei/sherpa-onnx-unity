using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineTtsConfig
{
	public OfflineTtsModelConfig Model;

	[MarshalAs(UnmanagedType.LPStr)]
	public string RuleFsts;

	public int MaxNumSentences;

	[MarshalAs(UnmanagedType.LPStr)]
	public string RuleFars;

	public float SilenceScale;

	public OfflineTtsConfig()
	{
		Model = new OfflineTtsModelConfig();
		RuleFsts = "";
		MaxNumSentences = 1;
		RuleFars = "";
		SilenceScale = 0.2f;
	}
}
