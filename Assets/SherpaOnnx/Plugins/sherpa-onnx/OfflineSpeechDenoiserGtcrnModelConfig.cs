/// Copyright (c)  2025  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineSpeechDenoiserGtcrnModelConfig
    {
        public static OfflineSpeechDenoiserGtcrnModelConfig Creat()
        {
            return new OfflineSpeechDenoiserGtcrnModelConfig
            {
                Model = "",
            };
        }
        [MarshalAs(UnmanagedType.LPStr)]
        public string Model;
    }
}
