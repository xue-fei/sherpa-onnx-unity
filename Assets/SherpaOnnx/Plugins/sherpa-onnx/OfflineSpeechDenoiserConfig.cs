/// Copyright (c)  2025  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineSpeechDenoiserConfig
    {
        public static OfflineSpeechDenoiserConfig Creat()
        {
            return new OfflineSpeechDenoiserConfig
            {
                Model = OfflineSpeechDenoiserModelConfig.Creat(),
            };
        }
        public OfflineSpeechDenoiserModelConfig Model;
    }
}
