/// Copyright (c)  2024  Xiaomi Corporation

using System.Runtime.InteropServices;

namespace SherpaOnnx
{

    [StructLayout(LayoutKind.Sequential)]
    public struct FastClusteringConfig
    {
        public static FastClusteringConfig Create()
        {
            return new FastClusteringConfig()
            {
                NumClusters = -1,
                Threshold = 0.5F
            };
        }
        public int NumClusters;
        public float Threshold;
    }
}