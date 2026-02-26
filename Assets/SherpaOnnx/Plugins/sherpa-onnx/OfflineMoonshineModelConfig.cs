/// Copyright (c)  2024  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineMoonshineModelConfig
    {
        public static OfflineMoonshineModelConfig Creat()
        {
            return new OfflineMoonshineModelConfig
            {
                Preprocessor = "",
                Encoder = "",
                UncachedDecoder = "",
                CachedDecoder = "",
            };
        }
        [MarshalAs(UnmanagedType.LPStr)]
        public string Preprocessor;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Encoder;

        [MarshalAs(UnmanagedType.LPStr)]
        public string UncachedDecoder;

        [MarshalAs(UnmanagedType.LPStr)]
        public string CachedDecoder;
    }
}
