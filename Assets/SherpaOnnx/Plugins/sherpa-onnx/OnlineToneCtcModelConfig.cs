/// Copyright (c)  2025  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OnlineToneCtcModelConfig
    {
        public static OnlineToneCtcModelConfig Creat()
        {
            return new OnlineToneCtcModelConfig
            {
                Model = "",
            };
        }

        [MarshalAs(UnmanagedType.LPStr)]
        public string Model;
    }
}
