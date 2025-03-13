using System;

namespace SherpaOnnx;

public delegate int OfflineSpeakerDiarizationProgressCallback(int numProcessedChunks, int numTotalChunks, IntPtr arg);
