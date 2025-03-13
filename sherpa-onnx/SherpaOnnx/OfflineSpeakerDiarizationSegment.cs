using System;
using System.Runtime.InteropServices;

namespace SherpaOnnx;

public class OfflineSpeakerDiarizationSegment
{
	private struct Impl
	{
		public float Start;

		public float End;

		public int Speaker;
	}

	public float Start;

	public float End;

	public int Speaker;

	public OfflineSpeakerDiarizationSegment(IntPtr handle)
	{
		Impl impl = (Impl)Marshal.PtrToStructure(handle, typeof(Impl));
		Start = impl.Start;
		End = impl.End;
		Speaker = impl.Speaker;
	}
}
