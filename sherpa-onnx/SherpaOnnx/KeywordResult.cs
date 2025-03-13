using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SherpaOnnx;

public class KeywordResult
{
	private struct Impl
	{
		public IntPtr Keyword;
	}

	private string _keyword;

	public string Keyword => _keyword;

	public unsafe KeywordResult(IntPtr handle)
	{
		Impl impl = (Impl)Marshal.PtrToStructure(handle, typeof(Impl));
		int num = 0;
		byte* ptr = (byte*)(void*)impl.Keyword;
		while (*ptr != 0)
		{
			ptr++;
			num++;
		}
		byte[] array = new byte[num];
		Marshal.Copy(impl.Keyword, array, 0, num);
		_keyword = Encoding.UTF8.GetString(array);
	}
}
