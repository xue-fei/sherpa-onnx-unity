using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SherpaOnnx;

public class OnlineRecognizerResult
{
	private struct Impl
	{
		public IntPtr Text;

		public IntPtr Tokens;

		public IntPtr TokensArr;

		public IntPtr Timestamps;

		public int Count;
	}

	private string _text;

	private string[] _tokens;

	private float[] _timestamps;

	public string Text => _text;

	public string[] Tokens => _tokens;

	public float[] Timestamps => _timestamps;

	public unsafe OnlineRecognizerResult(IntPtr handle)
	{
		Impl impl = (Impl)Marshal.PtrToStructure(handle, typeof(Impl));
		int num = 0;
		byte* ptr = (byte*)(void*)impl.Text;
		while (*ptr != 0)
		{
			ptr++;
			num++;
		}
		byte[] array = new byte[num];
		Marshal.Copy(impl.Text, array, 0, num);
		_text = Encoding.UTF8.GetString(array);
		_tokens = new string[impl.Count];
		byte* ptr2 = (byte*)(void*)impl.Tokens;
		for (int i = 0; i < impl.Count; i++)
		{
			num = 0;
			byte* ptr3 = ptr2;
			while (*ptr2 != 0)
			{
				ptr2++;
				num++;
			}
			ptr2++;
			array = new byte[num];
			fixed (byte* ptr4 = array)
			{
				for (int j = 0; j < num; j++)
				{
					ptr4[j] = ptr3[j];
				}
			}
			_tokens[i] = Encoding.UTF8.GetString(array);
		}
		float* ptr5 = (float*)(void*)impl.Timestamps;
		if (ptr5 != null)
		{
			_timestamps = new float[impl.Count];
			fixed (float* timestamps = _timestamps)
			{
				for (int k = 0; k < impl.Count; k++)
				{
					timestamps[k] = ptr5[k];
				}
			}
		}
		else
		{
			_timestamps = new float[0];
		}
	}
}
