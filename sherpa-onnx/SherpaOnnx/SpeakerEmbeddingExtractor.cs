using System;
using System.Runtime.InteropServices;

namespace SherpaOnnx;

public class SpeakerEmbeddingExtractor : IDisposable
{
	private HandleRef _handle;

	public int Dim => SherpaOnnxSpeakerEmbeddingExtractorDim(_handle.Handle);

	public SpeakerEmbeddingExtractor(SpeakerEmbeddingExtractorConfig config)
	{
		IntPtr handle = SherpaOnnxCreateSpeakerEmbeddingExtractor(ref config);
		_handle = new HandleRef(this, handle);
	}

	public OnlineStream CreateStream()
	{
		return new OnlineStream(SherpaOnnxSpeakerEmbeddingExtractorCreateStream(_handle.Handle));
	}

	public bool IsReady(OnlineStream stream)
	{
		return SherpaOnnxSpeakerEmbeddingExtractorIsReady(_handle.Handle, stream.Handle) != 0;
	}

	public float[] Compute(OnlineStream stream)
	{
		IntPtr intPtr = SherpaOnnxSpeakerEmbeddingExtractorComputeEmbedding(_handle.Handle, stream.Handle);
		int dim = Dim;
		float[] array = new float[dim];
		Marshal.Copy(intPtr, array, 0, dim);
		SherpaOnnxSpeakerEmbeddingExtractorDestroyEmbedding(intPtr);
		return array;
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	~SpeakerEmbeddingExtractor()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		SherpaOnnxDestroySpeakerEmbeddingExtractor(_handle.Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxCreateSpeakerEmbeddingExtractor(ref SpeakerEmbeddingExtractorConfig config);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxDestroySpeakerEmbeddingExtractor(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern int SherpaOnnxSpeakerEmbeddingExtractorDim(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxSpeakerEmbeddingExtractorCreateStream(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern int SherpaOnnxSpeakerEmbeddingExtractorIsReady(IntPtr handle, IntPtr stream);

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxSpeakerEmbeddingExtractorComputeEmbedding(IntPtr handle, IntPtr stream);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxSpeakerEmbeddingExtractorDestroyEmbedding(IntPtr p);
}
