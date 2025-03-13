using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SherpaOnnx;

public class OnlineRecognizer : IDisposable
{
	private HandleRef _handle;

	public OnlineRecognizer(OnlineRecognizerConfig config)
	{
		IntPtr handle = SherpaOnnxCreateOnlineRecognizer(ref config);
		_handle = new HandleRef(this, handle);
	}

	public OnlineStream CreateStream()
	{
		return new OnlineStream(SherpaOnnxCreateOnlineStream(_handle.Handle));
	}

	public bool IsReady(OnlineStream stream)
	{
		return IsReady(_handle.Handle, stream.Handle) != 0;
	}

	public bool IsEndpoint(OnlineStream stream)
	{
		return SherpaOnnxOnlineStreamIsEndpoint(_handle.Handle, stream.Handle) != 0;
	}

	public void Decode(OnlineStream stream)
	{
		Decode(_handle.Handle, stream.Handle);
	}

	public void Decode(IEnumerable<OnlineStream> streams)
	{
		List<IntPtr> list = new List<IntPtr>();
		foreach (OnlineStream stream in streams)
		{
			list.Add(stream.Handle);
		}
		IntPtr[] array = list.ToArray();
		Decode(_handle.Handle, array, array.Length);
	}

	public OnlineRecognizerResult GetResult(OnlineStream stream)
	{
		IntPtr result = GetResult(_handle.Handle, stream.Handle);
		OnlineRecognizerResult result2 = new OnlineRecognizerResult(result);
		DestroyResult(result);
		return result2;
	}

	public void Reset(OnlineStream stream)
	{
		SherpaOnnxOnlineStreamReset(_handle.Handle, stream.Handle);
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	~OnlineRecognizer()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		SherpaOnnxDestroyOnlineRecognizer(_handle.Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxCreateOnlineRecognizer(ref OnlineRecognizerConfig config);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxDestroyOnlineRecognizer(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxCreateOnlineStream(IntPtr handle);

	[DllImport(Dll.Filename, EntryPoint = "SherpaOnnxIsOnlineStreamReady")]
	private static extern int IsReady(IntPtr handle, IntPtr stream);

	[DllImport(Dll.Filename, EntryPoint = "SherpaOnnxDecodeOnlineStream")]
	private static extern void Decode(IntPtr handle, IntPtr stream);

	[DllImport(Dll.Filename, EntryPoint = "SherpaOnnxDecodeMultipleOnlineStreams")]
	private static extern void Decode(IntPtr handle, IntPtr[] streams, int n);

	[DllImport(Dll.Filename, EntryPoint = "SherpaOnnxGetOnlineStreamResult")]
	private static extern IntPtr GetResult(IntPtr handle, IntPtr stream);

	[DllImport(Dll.Filename, EntryPoint = "SherpaOnnxDestroyOnlineRecognizerResult")]
	private static extern void DestroyResult(IntPtr result);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxOnlineStreamReset(IntPtr handle, IntPtr stream);

	[DllImport(Dll.Filename)]
	private static extern int SherpaOnnxOnlineStreamIsEndpoint(IntPtr handle, IntPtr stream);
}
