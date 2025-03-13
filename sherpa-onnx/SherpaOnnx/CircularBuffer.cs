using System;
using System.Runtime.InteropServices;

namespace SherpaOnnx;

public class CircularBuffer : IDisposable
{
	private HandleRef _handle;

	public int Size => SherpaOnnxCircularBufferSize(_handle.Handle);

	public int Head => SherpaOnnxCircularBufferHead(_handle.Handle);

	public CircularBuffer(int capacity)
	{
		IntPtr handle = SherpaOnnxCreateCircularBuffer(capacity);
		_handle = new HandleRef(this, handle);
	}

	public void Push(float[] data)
	{
		SherpaOnnxCircularBufferPush(_handle.Handle, data, data.Length);
	}

	public float[] Get(int startIndex, int n)
	{
		IntPtr intPtr = SherpaOnnxCircularBufferGet(_handle.Handle, startIndex, n);
		float[] array = new float[n];
		Marshal.Copy(intPtr, array, 0, n);
		SherpaOnnxCircularBufferFree(intPtr);
		return array;
	}

	public void Pop(int n)
	{
		SherpaOnnxCircularBufferPop(_handle.Handle, n);
	}

	public void Reset()
	{
		SherpaOnnxCircularBufferReset(_handle.Handle);
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	~CircularBuffer()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		SherpaOnnxDestroyCircularBuffer(_handle.Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxCreateCircularBuffer(int capacity);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxDestroyCircularBuffer(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxCircularBufferPush(IntPtr handle, float[] p, int n);

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxCircularBufferGet(IntPtr handle, int startIndex, int n);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxCircularBufferFree(IntPtr p);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxCircularBufferPop(IntPtr handle, int n);

	[DllImport("sherpa-onnx-c-api")]
	private static extern int SherpaOnnxCircularBufferSize(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern int SherpaOnnxCircularBufferHead(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxCircularBufferReset(IntPtr handle);
}
