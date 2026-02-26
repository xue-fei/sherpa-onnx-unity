/// Copyright (c)  2024.5 by 东风破

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    public struct SpokenLanguageIdentificationConfig
    {
        public static SpokenLanguageIdentificationConfig Creat()
        {
            return new SpokenLanguageIdentificationConfig
            {
                Whisper = SpokenLanguageIdentificationWhisperConfig.Creat(),
                NumThreads = 1,
                Debug = 0,
                Provider = "cpu",
            };
        }
        public SpokenLanguageIdentificationWhisperConfig Whisper;

        public int NumThreads;
        public int Debug;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Provider;
    }

}