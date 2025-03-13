using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineParaformerModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Model;

	public OfflineParaformerModelConfig()
	{
		Model = "";
	}
}
