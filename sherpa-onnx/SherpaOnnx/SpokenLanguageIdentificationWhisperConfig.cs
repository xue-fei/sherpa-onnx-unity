using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct SpokenLanguageIdentificationWhisperConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Encoder;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Decoder;

	public int TailPaddings;

	public SpokenLanguageIdentificationWhisperConfig()
	{
		Encoder = "";
		Decoder = "";
		TailPaddings = -1;
	}
}
