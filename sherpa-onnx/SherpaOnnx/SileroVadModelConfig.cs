using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct SileroVadModelConfig
{
	[MarshalAs(UnmanagedType.LPStr)]
	public string Model;

	public float Threshold;

	public float MinSilenceDuration;

	public float MinSpeechDuration;

	public int WindowSize;

	public float MaxSpeechDuration;

	public SileroVadModelConfig()
	{
		Model = "";
		Threshold = 0.5f;
		MinSilenceDuration = 0.5f;
		MinSpeechDuration = 0.25f;
		WindowSize = 512;
		MaxSpeechDuration = 5f;
	}
}
