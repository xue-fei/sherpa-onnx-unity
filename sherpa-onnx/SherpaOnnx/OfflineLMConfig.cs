using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OfflineLMConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Model;

	public float Scale;

	public OfflineLMConfig()
	{
		Model = "";
		Scale = 0.5f;
	}
}
