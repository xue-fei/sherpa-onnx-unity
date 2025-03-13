using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct SpeakerEmbeddingExtractorConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Model;

	public int NumThreads;

	public int Debug;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Provider;

	public SpeakerEmbeddingExtractorConfig()
	{
		Model = "";
		NumThreads = 1;
		Debug = 0;
		Provider = "cpu";
	}
}
