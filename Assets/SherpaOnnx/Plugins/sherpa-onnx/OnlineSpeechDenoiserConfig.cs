/// Copyright (c)  2026  Xiaomi Corporation (authors: Fangjun Kuang)

namespace SherpaOnnx
{
    public struct OnlineSpeechDenoiserConfig
    {
        public static OnlineSpeechDenoiserConfig Creat()
        {
            return new OnlineSpeechDenoiserConfig
            {
                Model = new OfflineSpeechDenoiserModelConfig(),
            };
        }

        public OfflineSpeechDenoiserModelConfig Model;
    }
}