using System;
using System.Runtime.InteropServices;

namespace SherpaOnnx;

public class OfflineSpeakerDiarization : IDisposable
{
	private HandleRef _handle;

	public int SampleRate => SherpaOnnxOfflineSpeakerDiarizationGetSampleRate(_handle.Handle);

	public OfflineSpeakerDiarization(OfflineSpeakerDiarizationConfig config)
	{
		IntPtr handle = SherpaOnnxCreateOfflineSpeakerDiarization(ref config);
		_handle = new HandleRef(this, handle);
	}

	public void SetConfig(OfflineSpeakerDiarizationConfig config)
	{
		SherpaOnnxOfflineSpeakerDiarizationSetConfig(_handle.Handle, ref config);
	}

	public OfflineSpeakerDiarizationSegment[] Process(float[] samples)
	{
		IntPtr result = SherpaOnnxOfflineSpeakerDiarizationProcess(_handle.Handle, samples, samples.Length);
		return ProcessImpl(result);
	}

	public OfflineSpeakerDiarizationSegment[] ProcessWithCallback(float[] samples, OfflineSpeakerDiarizationProgressCallback callback, IntPtr arg)
	{
		IntPtr result = SherpaOnnxOfflineSpeakerDiarizationProcessWithCallback(_handle.Handle, samples, samples.Length, callback, arg);
		return ProcessImpl(result);
	}

	private unsafe OfflineSpeakerDiarizationSegment[] ProcessImpl(IntPtr result)
	{
		if (result == IntPtr.Zero)
		{
			return new OfflineSpeakerDiarizationSegment[0];
		}
		int num = SherpaOnnxOfflineSpeakerDiarizationResultGetNumSegments(result);
		IntPtr intPtr = SherpaOnnxOfflineSpeakerDiarizationResultSortByStartTime(result);
		OfflineSpeakerDiarizationSegment[] array = new OfflineSpeakerDiarizationSegment[num];
		int num2 = 12;
		for (int i = 0; i != num; i++)
		{
			IntPtr handle = new IntPtr((byte*)(void*)intPtr + i * num2);
			array[i] = new OfflineSpeakerDiarizationSegment(handle);
		}
		SherpaOnnxOfflineSpeakerDiarizationDestroySegment(intPtr);
		SherpaOnnxOfflineSpeakerDiarizationDestroyResult(result);
		return array;
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	~OfflineSpeakerDiarization()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		SherpaOnnxDestroyOfflineSpeakerDiarization(_handle.Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxCreateOfflineSpeakerDiarization(ref OfflineSpeakerDiarizationConfig config);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxDestroyOfflineSpeakerDiarization(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern int SherpaOnnxOfflineSpeakerDiarizationGetSampleRate(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern int SherpaOnnxOfflineSpeakerDiarizationResultGetNumSegments(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxOfflineSpeakerDiarizationProcess(IntPtr handle, float[] samples, int n);

	[DllImport("sherpa-onnx-c-api", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr SherpaOnnxOfflineSpeakerDiarizationProcessWithCallback(IntPtr handle, float[] samples, int n, OfflineSpeakerDiarizationProgressCallback callback, IntPtr arg);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxOfflineSpeakerDiarizationDestroyResult(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxOfflineSpeakerDiarizationResultSortByStartTime(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxOfflineSpeakerDiarizationDestroySegment(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxOfflineSpeakerDiarizationSetConfig(IntPtr handle, ref OfflineSpeakerDiarizationConfig config);
}
