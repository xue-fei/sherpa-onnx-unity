using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct SpokenLanguageIdentificationConfig
{
	public SpokenLanguageIdentificationWhisperConfig Whisper;

	public int NumThreads;

	public int Debug;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Provider;

	public SpokenLanguageIdentificationConfig()
	{
		Whisper = new SpokenLanguageIdentificationWhisperConfig();
		NumThreads = 1;
		Debug = 0;
		Provider = "cpu";
	}
}
