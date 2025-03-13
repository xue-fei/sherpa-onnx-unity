using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OnlineZipformer2CtcModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Model;

	public OnlineZipformer2CtcModelConfig()
	{
		Model = "";
	}
}
