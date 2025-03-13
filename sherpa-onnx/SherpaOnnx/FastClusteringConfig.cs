namespace SherpaOnnx;

public struct FastClusteringConfig
{
	public int NumClusters;

	public float Threshold;

	public FastClusteringConfig()
	{
		NumClusters = -1;
		Threshold = 0.5f;
	}
}
