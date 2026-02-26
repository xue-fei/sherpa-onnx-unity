/// Copyright (c)  2025  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineSpeechDenoiserModelConfig
    {
        public static OfflineSpeechDenoiserModelConfig Creat()
        {
            return new OfflineSpeechDenoiserModelConfig
            {
                Gtcrn =   OfflineSpeechDenoiserGtcrnModelConfig.Creat(),
                NumThreads = 1,
                Debug = 0,
                Provider = "cpu",
            };
        }

        public OfflineSpeechDenoiserGtcrnModelConfig Gtcrn;

        public int NumThreads;

        public int Debug;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Provider;
    }
}
