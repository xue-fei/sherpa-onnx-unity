using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SherpaOnnx;

public class KeywordSpotter : IDisposable
{
	private HandleRef _handle;

	public KeywordSpotter(KeywordSpotterConfig config)
	{
		IntPtr handle = SherpaOnnxCreateKeywordSpotter(ref config);
		_handle = new HandleRef(this, handle);
	}

	public OnlineStream CreateStream()
	{
		return new OnlineStream(SherpaOnnxCreateKeywordStream(_handle.Handle));
	}

	public OnlineStream CreateStream(string keywords)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(keywords);
		byte[] array = new byte[bytes.Length + 1];
		Array.Copy(bytes, array, bytes.Length);
		array[bytes.Length] = 0;
		return new OnlineStream(SherpaOnnxCreateKeywordStreamWithKeywords(_handle.Handle, array));
	}

	public bool IsReady(OnlineStream stream)
	{
		return IsReady(_handle.Handle, stream.Handle) != 0;
	}

	public void Decode(OnlineStream stream)
	{
		Decode(_handle.Handle, stream.Handle);
	}

	public void Reset(OnlineStream stream)
	{
		Reset(_handle.Handle, stream.Handle);
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

	public KeywordResult GetResult(OnlineStream stream)
	{
		IntPtr result = GetResult(_handle.Handle, stream.Handle);
		KeywordResult result2 = new KeywordResult(result);
		DestroyResult(result);
		return result2;
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	~KeywordSpotter()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		SherpaOnnxDestroyKeywordSpotter(_handle.Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxCreateKeywordSpotter(ref KeywordSpotterConfig config);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxDestroyKeywordSpotter(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxCreateKeywordStream(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxCreateKeywordStreamWithKeywords(IntPtr handle, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] utf8Keywords);

	[DllImport("sherpa-onnx-c-api", EntryPoint = "SherpaOnnxIsKeywordStreamReady")]
	private static extern int IsReady(IntPtr handle, IntPtr stream);

	[DllImport("sherpa-onnx-c-api", EntryPoint = "SherpaOnnxDecodeKeywordStream")]
	private static extern void Decode(IntPtr handle, IntPtr stream);

	[DllImport("sherpa-onnx-c-api", EntryPoint = "SherpaOnnxResetKeywordStream")]
	private static extern void Reset(IntPtr handle, IntPtr stream);

	[DllImport("sherpa-onnx-c-api", EntryPoint = "SherpaOnnxDecodeMultipleKeywordStreams")]
	private static extern void Decode(IntPtr handle, IntPtr[] streams, int n);

	[DllImport("sherpa-onnx-c-api", EntryPoint = "SherpaOnnxGetKeywordResult")]
	private static extern IntPtr GetResult(IntPtr handle, IntPtr stream);

	[DllImport("sherpa-onnx-c-api", EntryPoint = "SherpaOnnxDestroyKeywordResult")]
	private static extern void DestroyResult(IntPtr result);
}
