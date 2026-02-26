/// Copyright (c)  2023  Xiaomi Corporation (authors: Fangjun Kuang)
/// Copyright (c)  2023 by manyeyes
/// Copyright (c)  2024.5 by 东风破

namespace SherpaOnnx
{
    internal static class Dll
    {
#if UNITY_IOS && !UNITY_EDITOR
        public const string Filename = "__internal";
#else
        public const string Filename = "sherpa-onnx-c-api";
#endif
    }
}