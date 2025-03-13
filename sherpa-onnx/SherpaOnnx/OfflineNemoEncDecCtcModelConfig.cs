using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineNemoEncDecCtcModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Model;

	public OfflineNemoEncDecCtcModelConfig()
	{
		Model = "";
	}
}
