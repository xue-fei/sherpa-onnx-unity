using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineTtsKokoroModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Model;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Voices;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Tokens;

	[MarshalAs(UnmanagedType.LPStr)]
	public string DataDir;

	public float LengthScale;

	[MarshalAs(UnmanagedType.LPStr)]
	public string DictDir;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Lexicon;

	public OfflineTtsKokoroModelConfig()
	{
		Model = "";
		Voices = "";
		Tokens = "";
		DataDir = "";
		LengthScale = 1f;
		DictDir = "";
		Lexicon = "";
	}
}
