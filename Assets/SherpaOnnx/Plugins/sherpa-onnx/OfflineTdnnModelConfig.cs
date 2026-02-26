/// Copyright (c)  2024.5 by 东风破

using System.Runtime.InteropServices;

namespace SherpaOnnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OfflineTdnnModelConfig
    {
        public static OfflineTdnnModelConfig Creat()
        {
            return new OfflineTdnnModelConfig
            {
                Model = "",
            };
        }
        [MarshalAs(UnmanagedType.LPStr)]
        public string Model;
    }

}