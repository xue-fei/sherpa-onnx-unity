using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflinePunctuationModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string CtTransformer;

	public int NumThreads;

	public int Debug;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Provider;

	public OfflinePunctuationModelConfig()
	{
		CtTransformer = "";
		NumThreads = 1;
		Debug = 0;
		Provider = "cpu";
	}
}
