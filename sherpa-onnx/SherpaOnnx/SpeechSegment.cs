using System;
using System.Runtime.InteropServices;

namespace SherpaOnnx;

public class SpeechSegment
{
	private struct Impl
	{
		public int Start;

		public IntPtr Samples;

		public int Count;
	}

	public int _start;

	private float[] _samples;

	public int Start => _start;

	public float[] Samples => _samples;

	public unsafe SpeechSegment(IntPtr handle)
	{
		Impl impl = (Impl)Marshal.PtrToStructure(handle, typeof(Impl));
		_start = impl.Start;
		float* ptr = (float*)(void*)impl.Samples;
		_samples = new float[impl.Count];
		fixed (float* samples = _samples)
		{
			for (int i = 0; i < impl.Count; i++)
			{
				samples[i] = ptr[i];
			}
		}
	}
}
