using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineTransducerModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Encoder;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Decoder;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Joiner;

	public OfflineTransducerModelConfig()
	{
		Encoder = "";
		Decoder = "";
		Joiner = "";
	}
}
