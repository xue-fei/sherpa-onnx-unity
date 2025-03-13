using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineFireRedAsrModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Encoder;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Decoder;

	public OfflineFireRedAsrModelConfig()
	{
		Encoder = "";
		Decoder = "";
	}
}
