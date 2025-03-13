using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineSpeakerSegmentationPyannoteModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Model;

	public OfflineSpeakerSegmentationPyannoteModelConfig()
	{
		Model = "";
	}
}
