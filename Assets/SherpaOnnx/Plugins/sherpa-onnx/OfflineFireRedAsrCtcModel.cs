/// Copyright (c)  2026  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineFireRedAsrCtcModelConfig
    {
        public static OfflineFireRedAsrCtcModelConfig Creat()
        {
            return new OfflineFireRedAsrCtcModelConfig
            {
                Model = "",
            };
        }
        [MarshalAs(UnmanagedType.LPStr)]
        public string Model;
    }
}