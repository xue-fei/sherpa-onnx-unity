/// Copyright (c)  2024.5 by 东风破

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineTtsModelConfig
    {
        public static OfflineTtsModelConfig Creat()
        {
            return new OfflineTtsModelConfig
            {
                Vits = OfflineTtsVitsModelConfig.Creat(),
                Matcha = OfflineTtsMatchaModelConfig.Creat(),
                Kokoro = OfflineTtsKokoroModelConfig.Creat(),
                Kitten = OfflineTtsKittenModelConfig.Creat(),
                ZipVoice = OfflineTtsZipVoiceModelConfig.Creat(),
                Pocket = OfflineTtsPocketModelConfig.Creat(),
                NumThreads = 1,
                Debug = 0,
                Provider = "cpu",
            };
        }

        public OfflineTtsVitsModelConfig Vits;
        public int NumThreads;
        public int Debug;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Provider;

        public OfflineTtsMatchaModelConfig Matcha;
        public OfflineTtsKokoroModelConfig Kokoro;
        public OfflineTtsKittenModelConfig Kitten;
        public OfflineTtsZipVoiceModelConfig ZipVoice;
        public OfflineTtsPocketModelConfig Pocket;
    }
}
