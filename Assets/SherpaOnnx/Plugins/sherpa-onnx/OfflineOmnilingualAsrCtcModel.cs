/// Copyright (c)  2025  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{

    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineOmnilingualAsrCtcModelConfig
    {
        public static OfflineOmnilingualAsrCtcModelConfig Creat()
        {
            return new OfflineOmnilingualAsrCtcModelConfig
            {
                Model = "",
            };
        }
        [MarshalAs(UnmanagedType.LPStr)]
        public string Model;
    }
}
