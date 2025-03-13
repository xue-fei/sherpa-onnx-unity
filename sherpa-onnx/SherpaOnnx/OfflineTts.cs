using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SherpaOnnx;

public class OfflineTts : IDisposable
{
	private HandleRef _handle;

	public int SampleRate => SherpaOnnxOfflineTtsSampleRate(_handle.Handle);

	public int NumSpeakers => SherpaOnnxOfflineTtsNumSpeakers(_handle.Handle);

	public OfflineTts(OfflineTtsConfig config)
	{
		IntPtr handle = SherpaOnnxCreateOfflineTts(ref config);
		_handle = new HandleRef(this, handle);
	}

	public OfflineTtsGeneratedAudio Generate(string text, float speed, int speakerId)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] array = new byte[bytes.Length + 1];
		Array.Copy(bytes, array, bytes.Length);
		array[bytes.Length] = 0;
		return new OfflineTtsGeneratedAudio(SherpaOnnxOfflineTtsGenerate(_handle.Handle, array, speakerId, speed));
	}

	public OfflineTtsGeneratedAudio GenerateWithCallback(string text, float speed, int speakerId, OfflineTtsCallback callback)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] array = new byte[bytes.Length + 1];
		Array.Copy(bytes, array, bytes.Length);
		array[bytes.Length] = 0;
		return new OfflineTtsGeneratedAudio(SherpaOnnxOfflineTtsGenerateWithCallback(_handle.Handle, array, speakerId, speed, callback));
	}

	public OfflineTtsGeneratedAudio GenerateWithCallbackProgress(string text, float speed, int speakerId, OfflineTtsCallbackProgress callback)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] array = new byte[bytes.Length + 1];
		Array.Copy(bytes, array, bytes.Length);
		array[bytes.Length] = 0;
		return new OfflineTtsGeneratedAudio(SherpaOnnxOfflineTtsGenerateWithProgressCallback(_handle.Handle, array, speakerId, speed, callback));
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	~OfflineTts()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		SherpaOnnxDestroyOfflineTts(_handle.Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxCreateOfflineTts(ref OfflineTtsConfig config);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxDestroyOfflineTts(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern int SherpaOnnxOfflineTtsSampleRate(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern int SherpaOnnxOfflineTtsNumSpeakers(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxOfflineTtsGenerate(IntPtr handle, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] utf8Text, int sid, float speed);

	[DllImport(Dll.Filename, CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr SherpaOnnxOfflineTtsGenerateWithCallback(IntPtr handle, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] utf8Text, int sid, float speed, OfflineTtsCallback callback);

	[DllImport(Dll.Filename, CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr SherpaOnnxOfflineTtsGenerateWithProgressCallback(IntPtr handle, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] utf8Text, int sid, float speed, OfflineTtsCallbackProgress callback);
}
