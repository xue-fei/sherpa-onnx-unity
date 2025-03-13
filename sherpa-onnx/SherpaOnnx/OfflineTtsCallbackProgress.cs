using System;

namespace SherpaOnnx;

public delegate int OfflineTtsCallbackProgress(IntPtr samples, int n, float progress);
