/// Copyright (c)  2025  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioTaggingConfig
    {
        public static AudioTaggingConfig Creat()
        {
            return new AudioTaggingConfig
            {
                Model = AudioTaggingModelConfig.Creat(),
                Labels = "",
                TopK = 5,
            };
        }

        public AudioTaggingModelConfig Model;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Labels;

        public int TopK;
    }
}
