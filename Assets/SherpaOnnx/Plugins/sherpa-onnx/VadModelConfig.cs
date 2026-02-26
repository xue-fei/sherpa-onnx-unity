/// Copyright (c)  2024  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VadModelConfig
    {
        public static VadModelConfig Creat()
        {
            return new VadModelConfig
            {
                SileroVad = SileroVadModelConfig.Creat(),
                SampleRate = 16000,
                NumThreads = 1,
                Provider = "cpu",
                Debug = 0,
                TenVad = TenVadModelConfig.Creat(),
            };
        }

        public SileroVadModelConfig SileroVad;

        public int SampleRate;

        public int NumThreads;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Provider;

        public int Debug;

        public TenVadModelConfig TenVad;
    }
}

