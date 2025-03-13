using System;
using System.Runtime.InteropServices;

namespace SherpaOnnx;

public class SpokenLanguageIdentification : IDisposable
{
	private HandleRef _handle;

	public SpokenLanguageIdentification(SpokenLanguageIdentificationConfig config)
	{
		IntPtr handle = SherpaOnnxCreateSpokenLanguageIdentification(ref config);
		_handle = new HandleRef(this, handle);
	}

	public OfflineStream CreateStream()
	{
		return new OfflineStream(SherpaOnnxSpokenLanguageIdentificationCreateOfflineStream(_handle.Handle));
	}

	public SpokenLanguageIdentificationResult Compute(OfflineStream stream)
	{
		IntPtr handle = SherpaOnnxSpokenLanguageIdentificationCompute(_handle.Handle, stream.Handle);
		SpokenLanguageIdentificationResult result = new SpokenLanguageIdentificationResult(handle);
		SherpaOnnxDestroySpokenLanguageIdentificationResult(handle);
		return result;
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	~SpokenLanguageIdentification()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		SherpaOnnxDestroySpokenLanguageIdentification(_handle.Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxCreateSpokenLanguageIdentification(ref SpokenLanguageIdentificationConfig config);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxDestroySpokenLanguageIdentification(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxSpokenLanguageIdentificationCreateOfflineStream(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxSpokenLanguageIdentificationCompute(IntPtr handle, IntPtr stream);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxDestroySpokenLanguageIdentificationResult(IntPtr handle);
}
