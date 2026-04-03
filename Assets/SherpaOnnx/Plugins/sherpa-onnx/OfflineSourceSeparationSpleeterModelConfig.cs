/// Copyright (c)  2026  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineSourceSeparationSpleeterModelConfig
    {
        public static OfflineSourceSeparationSpleeterModelConfig Creat()
        {
            return new OfflineSourceSeparationSpleeterModelConfig
            {
                Vocals = "",
                Accompaniment = "",
            };
        }

        [MarshalAs(UnmanagedType.LPStr)]
        public string Vocals;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Accompaniment;
    }
}