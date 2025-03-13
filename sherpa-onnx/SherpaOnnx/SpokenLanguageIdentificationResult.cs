using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SherpaOnnx;

public class SpokenLanguageIdentificationResult
{
	private struct Impl
	{
		public IntPtr Lang;
	}

	private string _lang;

	public string Lang => _lang;

	public unsafe SpokenLanguageIdentificationResult(IntPtr handle)
	{
		Impl impl = (Impl)Marshal.PtrToStructure(handle, typeof(Impl));
		int num = 0;
		byte* ptr = (byte*)(void*)impl.Lang;
		while (*ptr != 0)
		{
			ptr++;
			num++;
		}
		byte[] array = new byte[num];
		Marshal.Copy(impl.Lang, array, 0, num);
		_lang = Encoding.UTF8.GetString(array);
	}
}
