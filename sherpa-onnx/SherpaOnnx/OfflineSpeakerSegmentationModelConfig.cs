using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineSpeakerSegmentationModelConfig
{
	public OfflineSpeakerSegmentationPyannoteModelConfig Pyannote;

	public int NumThreads;

	public int Debug;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Provider;

	public OfflineSpeakerSegmentationModelConfig()
	{
		Pyannote = new OfflineSpeakerSegmentationPyannoteModelConfig();
		NumThreads = 1;
		Debug = 0;
		Provider = "cpu";
	}
}
