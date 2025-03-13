using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineTtsModelConfig
{
	public OfflineTtsVitsModelConfig Vits;

	public int NumThreads;

	public int Debug;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Provider;

	public OfflineTtsMatchaModelConfig Matcha;

	public OfflineTtsKokoroModelConfig Kokoro;

	public OfflineTtsModelConfig()
	{
		Vits = new OfflineTtsVitsModelConfig();
		Matcha = new OfflineTtsMatchaModelConfig();
		Kokoro = new OfflineTtsKokoroModelConfig();
		NumThreads = 1;
		Debug = 0;
		Provider = "cpu";
	}
}
