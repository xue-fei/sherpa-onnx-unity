using System;
using System.Runtime.InteropServices;

namespace SherpaOnnx;

public class OnlineStream : IDisposable
{
	private HandleRef _handle;

	public IntPtr Handle => _handle.Handle;

	public OnlineStream(IntPtr p)
	{
		_handle = new HandleRef(this, p);
	}

	public void AcceptWaveform(int sampleRate, float[] samples)
	{
		SherpaOnnxOnlineStreamAcceptWaveform(Handle, sampleRate, samples, samples.Length);
	}

	public void InputFinished()
	{
		SherpaOnnxOnlineStreamInputFinished(Handle);
	}

	~OnlineStream()
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
		SherpaOnnxDestroyOnlineStream(Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxDestroyOnlineStream(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxOnlineStreamAcceptWaveform(IntPtr handle, int sampleRate, float[] samples, int n);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxOnlineStreamInputFinished(IntPtr handle);
}
