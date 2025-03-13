using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SherpaOnnx;

public class OfflineTtsGeneratedAudio
{
	private struct Impl
	{
		public IntPtr Samples;

		public int NumSamples;

		public int SampleRate;
	}

	private HandleRef _handle;

	public IntPtr Handle => _handle.Handle;

	public int NumSamples => ((Impl)Marshal.PtrToStructure(Handle, typeof(Impl))).NumSamples;

	public int SampleRate => ((Impl)Marshal.PtrToStructure(Handle, typeof(Impl))).SampleRate;

	public float[] Samples
	{
		get
		{
			Impl impl = (Impl)Marshal.PtrToStructure(Handle, typeof(Impl));
			float[] array = new float[impl.NumSamples];
			Marshal.Copy(impl.Samples, array, 0, impl.NumSamples);
			return array;
		}
	}

	public OfflineTtsGeneratedAudio(IntPtr p)
	{
		_handle = new HandleRef(this, p);
	}

	public bool SaveToWaveFile(string filename)
	{
		Impl impl = (Impl)Marshal.PtrToStructure(Handle, typeof(Impl));
		byte[] bytes = Encoding.UTF8.GetBytes(filename);
		byte[] array = new byte[bytes.Length + 1];
		Array.Copy(bytes, array, bytes.Length);
		array[bytes.Length] = 0;
		return SherpaOnnxWriteWave(impl.Samples, impl.NumSamples, impl.SampleRate, array) == 1;
	}

	~OfflineTtsGeneratedAudio()
	{
		Cleanup();
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	private void Cleanup()
	{
		SherpaOnnxDestroyOfflineTtsGeneratedAudio(Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxDestroyOfflineTtsGeneratedAudio(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern int SherpaOnnxWriteWave(IntPtr samples, int n, int sample_rate, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] utf8Filename);
}
