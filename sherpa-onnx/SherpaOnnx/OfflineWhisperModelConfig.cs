using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineWhisperModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Encoder;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Decoder;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Language;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Task;

	public int TailPaddings;

	public OfflineWhisperModelConfig()
	{
		Encoder = "";
		Decoder = "";
		Language = "";
		Task = "transcribe";
		TailPaddings = -1;
	}
}
