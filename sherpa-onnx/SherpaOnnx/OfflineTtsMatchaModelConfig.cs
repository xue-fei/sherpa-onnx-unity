using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineTtsMatchaModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string AcousticModel;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Vocoder;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Lexicon;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Tokens;

	[MarshalAs(UnmanagedType.LPStr)]
	public string DataDir;

	public float NoiseScale;

	public float LengthScale;

	[MarshalAs(UnmanagedType.LPStr)]
	public string DictDir;

	public OfflineTtsMatchaModelConfig()
	{
		AcousticModel = "";
		Vocoder = "";
		Lexicon = "";
		Tokens = "";
		DataDir = "";
		NoiseScale = 0.667f;
		LengthScale = 1f;
		DictDir = "";
	}
}
