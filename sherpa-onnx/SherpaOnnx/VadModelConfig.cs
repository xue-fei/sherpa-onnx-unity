using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct VadModelConfig
{
	public SileroVadModelConfig SileroVad;

	public int SampleRate;

	public int NumThreads;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Provider;

	public int Debug;

	public VadModelConfig()
	{
		SileroVad = new SileroVadModelConfig();
		SampleRate = 16000;
		NumThreads = 1;
		Provider = "cpu";
		Debug = 0;
	}
}
