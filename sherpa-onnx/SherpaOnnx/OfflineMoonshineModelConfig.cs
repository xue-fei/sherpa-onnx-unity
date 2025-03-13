using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineMoonshineModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Preprocessor;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Encoder;

	[MarshalAs(UnmanagedType.LPStr)]
	public string UncachedDecoder;

	[MarshalAs(UnmanagedType.LPStr)]
	public string CachedDecoder;

	public OfflineMoonshineModelConfig()
	{
		Preprocessor = "";
		Encoder = "";
		UncachedDecoder = "";
		CachedDecoder = "";
	}
}
