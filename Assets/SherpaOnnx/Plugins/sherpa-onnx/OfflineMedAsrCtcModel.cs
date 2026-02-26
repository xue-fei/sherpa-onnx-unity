/// Copyright (c)  2025  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{

    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineMedAsrCtcModelConfig
    {
        public static OfflineMedAsrCtcModelConfig Creat()
        {
            return new OfflineMedAsrCtcModelConfig
            {
                Model = "",
            };
        }
        [MarshalAs(UnmanagedType.LPStr)]
        public string Model;
    }
}
