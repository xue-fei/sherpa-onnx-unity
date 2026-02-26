/// Copyright (c)  2025  Xiaomi Corporation (authors: Fangjun Kuang)

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OnlineNemoCtcModelConfig
    {
        public static OnlineNemoCtcModelConfig Creat()
        {
            return new OnlineNemoCtcModelConfig
            {
                Model = "",
            };
        }

        [MarshalAs(UnmanagedType.LPStr)]
        public string Model;
    }
}
