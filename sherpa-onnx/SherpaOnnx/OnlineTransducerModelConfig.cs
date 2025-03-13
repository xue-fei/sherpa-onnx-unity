using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OnlineTransducerModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Encoder;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Decoder;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Joiner;

	public OnlineTransducerModelConfig()
	{
		Encoder = "";
		Decoder = "";
		Joiner = "";
	}
}
