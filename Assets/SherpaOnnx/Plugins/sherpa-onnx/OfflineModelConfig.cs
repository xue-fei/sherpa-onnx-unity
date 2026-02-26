/// Copyright (c)  2024.5 by 东风破

using System.Runtime.InteropServices;

namespace SherpaOnnx
{

    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineModelConfig
    {
        public static OfflineModelConfig Creat()
        {
            return new OfflineModelConfig
            {
                Transducer = OfflineTransducerModelConfig.Creat(),
                Paraformer = new OfflineParaformerModelConfig(),
                NeMoCtc = OfflineNemoEncDecCtcModelConfig.Creat(),
                Whisper = new OfflineWhisperModelConfig(),
                Tdnn = new OfflineTdnnModelConfig(),
                Tokens = "",
                NumThreads = 1,
                Debug = 0,
                Provider = "cpu",
                ModelType = "",
                ModelingUnit = "cjkchar",
                BpeVocab = "",
                TeleSpeechCtc = "",
                SenseVoice =   OfflineSenseVoiceModelConfig.Creat(),
                Moonshine =   OfflineMoonshineModelConfig.Creat(),
                FireRedAsr =  OfflineFireRedAsrModelConfig.Creat(),
                Dolphin = OfflineDolphinModelConfig.Create(),
                ZipformerCtc =   OfflineZipformerCtcModelConfig.Creat(),
                Canary =   OfflineCanaryModelConfig.Creat(),
                WenetCtc =   OfflineWenetCtcModelConfig.Creat(),
                Omnilingual =  OfflineOmnilingualAsrCtcModelConfig.Creat(),
                MedAsr =   OfflineMedAsrCtcModelConfig.Creat(),
                FunAsrNano =   OfflineFunAsrNanoModelConfig.Creat(),
            };
        }
        public OfflineTransducerModelConfig Transducer;
        public OfflineParaformerModelConfig Paraformer;
        public OfflineNemoEncDecCtcModelConfig NeMoCtc;
        public OfflineWhisperModelConfig Whisper;
        public OfflineTdnnModelConfig Tdnn;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Tokens;

        public int NumThreads;

        public int Debug;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Provider;

        [MarshalAs(UnmanagedType.LPStr)]
        public string ModelType;

        [MarshalAs(UnmanagedType.LPStr)]
        public string ModelingUnit;

        [MarshalAs(UnmanagedType.LPStr)]
        public string BpeVocab;

        [MarshalAs(UnmanagedType.LPStr)]
        public string TeleSpeechCtc;

        public OfflineSenseVoiceModelConfig SenseVoice;
        public OfflineMoonshineModelConfig Moonshine;
        public OfflineFireRedAsrModelConfig FireRedAsr;
        public OfflineDolphinModelConfig Dolphin;
        public OfflineZipformerCtcModelConfig ZipformerCtc;
        public OfflineCanaryModelConfig Canary;
        public OfflineWenetCtcModelConfig WenetCtc;
        public OfflineOmnilingualAsrCtcModelConfig Omnilingual;
        public OfflineMedAsrCtcModelConfig MedAsr;
        public OfflineFunAsrNanoModelConfig FunAsrNano;
    }
}
