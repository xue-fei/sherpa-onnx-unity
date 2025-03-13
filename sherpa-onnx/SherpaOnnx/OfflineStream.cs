using System;
using System.Runtime.InteropServices;

namespace SherpaOnnx;

public class OfflineStream : IDisposable
{
	private HandleRef _handle;

	public OfflineRecognizerResult Result
	{
		get
		{
			IntPtr result = GetResult(_handle.Handle);
			OfflineRecognizerResult result2 = new OfflineRecognizerResult(result);
			DestroyResult(result);
			return result2;
		}
	}

	public IntPtr Handle => _handle.Handle;

	public OfflineStream(IntPtr p)
	{
		_handle = new HandleRef(this, p);
	}

	public void AcceptWaveform(int sampleRate, float[] samples)
	{
		AcceptWaveform(Handle, sampleRate, samples, samples.Length);
	}

	~OfflineStream()
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
		SherpaOnnxDestroyOfflineStream(Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxDestroyOfflineStream(IntPtr handle);

	[DllImport(Dll.Filename, EntryPoint = "SherpaOnnxAcceptWaveformOffline")]
	private static extern void AcceptWaveform(IntPtr handle, int sampleRate, float[] samples, int n);

	[DllImport(Dll.Filename, EntryPoint = "SherpaOnnxGetOfflineStreamResult")]
	private static extern IntPtr GetResult(IntPtr handle);

	[DllImport(Dll.Filename, EntryPoint = "SherpaOnnxDestroyOfflineRecognizerResult")]
	private static extern void DestroyResult(IntPtr handle);
}
