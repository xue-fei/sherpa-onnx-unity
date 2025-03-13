using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineSenseVoiceModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Model;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Language;

	public int UseInverseTextNormalization;

	public OfflineSenseVoiceModelConfig()
	{
		Model = "";
		Language = "";
		UseInverseTextNormalization = 0;
	}
}
