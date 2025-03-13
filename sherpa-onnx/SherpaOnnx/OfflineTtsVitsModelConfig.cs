using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineTtsVitsModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Model;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Lexicon;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Tokens;

	[MarshalAs(UnmanagedType.LPStr)]
	public string DataDir;

	public float NoiseScale;

	public float NoiseScaleW;

	public float LengthScale;

	[MarshalAs(UnmanagedType.LPStr)]
	public string DictDir;

	public OfflineTtsVitsModelConfig()
	{
		Model = "";
		Lexicon = "";
		Tokens = "";
		DataDir = "";
		NoiseScale = 0.667f;
		NoiseScaleW = 0.8f;
		LengthScale = 1f;
		DictDir = "";
	}
}
