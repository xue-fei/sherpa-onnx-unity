using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SherpaOnnx;

public class OfflinePunctuation : IDisposable
{
	private HandleRef _handle;

	public OfflinePunctuation(OfflinePunctuationConfig config)
	{
		IntPtr handle = SherpaOnnxCreateOfflinePunctuation(ref config);
		_handle = new HandleRef(this, handle);
	}

	public unsafe string AddPunct(string text)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] array = new byte[bytes.Length + 1];
		Array.Copy(bytes, array, bytes.Length);
		array[bytes.Length] = 0;
		IntPtr intPtr = SherpaOfflinePunctuationAddPunct(_handle.Handle, array);
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
			byte[] array2 = new byte[num];
			Marshal.Copy(intPtr, array2, 0, num);
			result = Encoding.UTF8.GetString(array2);
		}
		SherpaOfflinePunctuationFreeText(intPtr);
		return result;
	}

	public void Dispose()
	{
		Cleanup();
		GC.SuppressFinalize(this);
	}

	~OfflinePunctuation()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		SherpaOnnxDestroyOfflinePunctuation(_handle.Handle);
		_handle = new HandleRef(this, IntPtr.Zero);
	}

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOnnxCreateOfflinePunctuation(ref OfflinePunctuationConfig config);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOnnxDestroyOfflinePunctuation(IntPtr handle);

	[DllImport(Dll.Filename)]
	private static extern IntPtr SherpaOfflinePunctuationAddPunct(IntPtr handle, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] byte[] utf8Text);

	[DllImport(Dll.Filename)]
	private static extern void SherpaOfflinePunctuationFreeText(IntPtr p);
}
