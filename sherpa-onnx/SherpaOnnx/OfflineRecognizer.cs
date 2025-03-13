using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SherpaOnnx;

public class OfflineRecognizer : IDisposable
{
	private HandleRef _handle;

	public OfflineRecognizer(OfflineRecognizerConfig config)
	{
		IntPtr handle = SherpaOnnxCreateOfflineRecognizer(ref config);
		_handle = new HandleRef(this, handle);
	}

	public OfflineStream CreateStream()
	{
		return new OfflineStream(SherpaOnnxCreateOfflineStream(_handle.Handle));
	}

	public void Decode(OfflineStream stream)
	{
		Decode(_handle.Handle, stream.Handle);
	}

	public void Decode(IEnumerable<OfflineStream> streams)
	{
		List<IntPtr> list = new List<IntPtr>();
		foreach (OfflineStream stream in streams)
		{
			list.Add(stream.Handle);
		}
		IntPtr[] array = list.ToArray();
		Decode(_handle.Handle, array, array.Length);
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	~OfflineRecognizer()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		SherpaOnnxDestroyOfflineRecognizer(_handle.Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxCreateOfflineRecognizer(ref OfflineRecognizerConfig config);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxDestroyOfflineRecognizer(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxCreateOfflineStream(IntPtr handle);

	[DllImport(Dll.Filename, EntryPoint = "SherpaOnnxDecodeOfflineStream")]
	private static extern void Decode(IntPtr handle, IntPtr stream);

	[DllImport(Dll.Filename, EntryPoint = "SherpaOnnxDecodeMultipleOfflineStreams")]
	private static extern void Decode(IntPtr handle, IntPtr[] streams, int n);
}
