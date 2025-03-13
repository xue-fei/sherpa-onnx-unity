using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineTdnnModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Model;

	public OfflineTdnnModelConfig()
	{
		Model = "";
	}
}
