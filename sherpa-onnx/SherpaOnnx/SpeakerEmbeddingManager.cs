using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SherpaOnnx;

public class SpeakerEmbeddingManager : IDisposable
{
	private HandleRef _handle;

	private int _dim;

	public int NumSpeakers => SherpaOnnxSpeakerEmbeddingManagerNumSpeakers(_handle.Handle);

	public SpeakerEmbeddingManager(int dim)
	{
		IntPtr handle = SherpaOnnxCreateSpeakerEmbeddingManager(dim);
		_handle = new HandleRef(this, handle);
		_dim = dim;
	}

	public bool Add(string name, float[] v)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(name);
		byte[] array = new byte[bytes.Length + 1];
		Array.Copy(bytes, array, bytes.Length);
		array[bytes.Length] = 0;
		return SherpaOnnxSpeakerEmbeddingManagerAdd(_handle.Handle, array, v) == 1;
	}

	public bool Add(string name, ICollection<float[]> v_list)
	{
		int count = v_list.Count;
		float[] array = new float[count * _dim];
		int num = 0;
		foreach (float[] item in v_list)
		{
			item.CopyTo(array, num);
			num += _dim;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(name);
		byte[] array2 = new byte[bytes.Length + 1];
		Array.Copy(bytes, array2, bytes.Length);
		array2[bytes.Length] = 0;
		return SherpaOnnxSpeakerEmbeddingManagerAddListFlattened(_handle.Handle, array2, array, count) == 1;
	}

	public bool Remove(string name)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(name);
		byte[] array = new byte[bytes.Length + 1];
		Array.Copy(bytes, array, bytes.Length);
		array[bytes.Length] = 0;
		return SherpaOnnxSpeakerEmbeddingManagerRemove(_handle.Handle, array) == 1;
	}

	public unsafe string Search(float[] v, float threshold)
	{
		IntPtr intPtr = SherpaOnnxSpeakerEmbeddingManagerSearch(_handle.Handle, v, threshold);
		string result = "";
		int num = 0;
		byte* ptr = (byte*)(void*)intPtr;
		if (ptr != null)
		{
			while (*ptr != 0)
			{
				ptr++;
				num++;
			}
		}
		if (num > 0)
		{
			byte[] array = new byte[num];
			Marshal.Copy(intPtr, array, 0, num);
			result = Encoding.UTF8.GetString(array);
		}
		SherpaOnnxSpeakerEmbeddingManagerFreeSearch(intPtr);
		return result;
	}

	public bool Verify(string name, float[] v, float threshold)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(name);
		byte[] array = new byte[bytes.Length + 1];
		Array.Copy(bytes, array, bytes.Length);
		array[bytes.Length] = 0;
		return SherpaOnnxSpeakerEmbeddingManagerVerify(_handle.Handle, array, v, threshold) == 1;
	}

	public bool Contains(string name)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(name);
		byte[] array = new byte[bytes.Length + 1];
		Array.Copy(bytes, array, bytes.Length);
		array[bytes.Length] = 0;
		return SherpaOnnxSpeakerEmbeddingManagerContains(_handle.Handle, array) == 1;
	}

	public unsafe string[] GetAllSpeakers()
	{
		if (NumSpeakers == 0)
		{
			return new string[0];
		}
		IntPtr intPtr = SherpaOnnxSpeakerEmbeddingManagerGetAllSpeakers(_handle.Handle);
		string[] array = new string[NumSpeakers];
		byte** ptr = (byte**)(void*)intPtr;
		for (int i = 0; i != NumSpeakers; i++)
		{
			int num = 0;
			byte* ptr2 = ptr[i];
			while (*ptr2 != 0)
			{
				ptr2++;
				num++;
			}
			byte[] array2 = new byte[num];
			Marshal.Copy((IntPtr)ptr[i], array2, 0, num);
			array[i] = Encoding.UTF8.GetString(array2);
		}
		SherpaOnnxSpeakerEmbeddingManagerFreeAllSpeakers(intPtr);
		return array;
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	~SpeakerEmbeddingManager()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		SherpaOnnxDestroySpeakerEmbeddingManager(_handle.Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxCreateSpeakerEmbeddingManager(int dim);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxDestroySpeakerEmbeddingManager(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern int SherpaOnnxSpeakerEmbeddingManagerAdd(IntPtr handle, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] utf8Name, float[] v);

	[DllImport("sherpa-onnx-c-api")]
	private static extern int SherpaOnnxSpeakerEmbeddingManagerAddListFlattened(IntPtr handle, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] utf8Name, float[] v, int n);

	[DllImport("sherpa-onnx-c-api")]
	private static extern int SherpaOnnxSpeakerEmbeddingManagerRemove(IntPtr handle, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] utf8Name);

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxSpeakerEmbeddingManagerSearch(IntPtr handle, float[] v, float threshold);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxSpeakerEmbeddingManagerFreeSearch(IntPtr p);

	[DllImport("sherpa-onnx-c-api")]
	private static extern int SherpaOnnxSpeakerEmbeddingManagerVerify(IntPtr handle, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] utf8Name, float[] v, float threshold);

	[DllImport("sherpa-onnx-c-api")]
	private static extern int SherpaOnnxSpeakerEmbeddingManagerContains(IntPtr handle, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] utf8Name);

	[DllImport("sherpa-onnx-c-api")]
	private static extern int SherpaOnnxSpeakerEmbeddingManagerNumSpeakers(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern IntPtr SherpaOnnxSpeakerEmbeddingManagerGetAllSpeakers(IntPtr handle);

	[DllImport("sherpa-onnx-c-api")]
	private static extern void SherpaOnnxSpeakerEmbeddingManagerFreeAllSpeakers(IntPtr names);
}
