using System;
using System.Runtime.InteropServices;

namespace SherpaOnnx;

public class VoiceActivityDetector : IDisposable
{
	private HandleRef _handle;

	public VoiceActivityDetector(VadModelConfig config, float bufferSizeInSeconds)
	{
		IntPtr handle = SherpaOnnxCreateVoiceActivityDetector(ref config, bufferSizeInSeconds);
		_handle = new HandleRef(this, handle);
	}

	public void AcceptWaveform(float[] samples)
	{
		SherpaOnnxVoiceActivityDetectorAcceptWaveform(_handle.Handle, samples, samples.Length);
	}

	public bool IsEmpty()
	{
		return SherpaOnnxVoiceActivityDetectorEmpty(_handle.Handle) == 1;
	}

	public bool IsSpeechDetected()
	{
		return SherpaOnnxVoiceActivityDetectorDetected(_handle.Handle) == 1;
	}

	public void Pop()
	{
		SherpaOnnxVoiceActivityDetectorPop(_handle.Handle);
	}

	public SpeechSegment Front()
	{
		IntPtr intPtr = SherpaOnnxVoiceActivityDetectorFront(_handle.Handle);
		SpeechSegment result = new SpeechSegment(intPtr);
		SherpaOnnxDestroySpeechSegment(intPtr);
		return result;
	}

	public void Clear()
	{
		SherpaOnnxVoiceActivityDetectorClear(_handle.Handle);
	}

	public void Reset()
	{
		SherpaOnnxVoiceActivityDetectorReset(_handle.Handle);
	}

	public void Flush()
	{
		SherpaOnnxVoiceActivityDetectorFlush(_handle.Handle);
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	~VoiceActivityDetector()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		SherpaOnnxDestroyVoiceActivityDetector(_handle.Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxCreateVoiceActivityDetector(ref VadModelConfig config, float bufferSizeInSeconds);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxDestroyVoiceActivityDetector(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxVoiceActivityDetectorAcceptWaveform(IntPtr handle, float[] samples, int n);

	[DllImport(Dll.Filename)]
	private static extern int SherpaOnnxVoiceActivityDetectorEmpty(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern int SherpaOnnxVoiceActivityDetectorDetected(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxVoiceActivityDetectorPop(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxVoiceActivityDetectorClear(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxVoiceActivityDetectorFront(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxDestroySpeechSegment(IntPtr segment);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxVoiceActivityDetectorReset(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxVoiceActivityDetectorFlush(IntPtr handle);
}
