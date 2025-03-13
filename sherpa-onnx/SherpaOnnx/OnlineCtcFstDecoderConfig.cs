using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OnlineCtcFstDecoderConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Graph;

	public int MaxActive;

	public OnlineCtcFstDecoderConfig()
	{
		Graph = "";
		MaxActive = 3000;
	}
}
