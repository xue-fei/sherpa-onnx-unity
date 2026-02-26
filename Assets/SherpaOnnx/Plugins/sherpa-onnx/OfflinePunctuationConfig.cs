/// Copyright (c)  2024  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OfflinePunctuationConfig
    {
        public static OfflinePunctuationConfig Creat()
        {
            return new OfflinePunctuationConfig
            {
                Model = OfflinePunctuationModelConfig.Creat(),
            };
        }
        public OfflinePunctuationModelConfig Model;
    }
}

