/// Copyright (c)  2026  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OnlinePunctuationConfig
    {
        public static OnlinePunctuationConfig Creat()
        {
            return new OnlinePunctuationConfig
            {
                Model = new OnlinePunctuationModelConfig(),
            };
        }

        public OnlinePunctuationModelConfig Model;
    }
}