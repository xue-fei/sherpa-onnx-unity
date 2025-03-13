using System.Runtime.InteropServices;

namespace SherpaOnnx;

public struct OnlineModelConfig
{
	public OnlineTransducerModelConfig Transducer;

	public OnlineParaformerModelConfig Paraformer;

	public OnlineZipformer2CtcModelConfig Zipformer2Ctc;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Tokens;

	public int NumThreads;

	[MarshalAs(UnmanagedType.LPStr)]
	public string Provider;

	public int Debug;

	[MarshalAs(UnmanagedType.LPStr)]
	public string ModelType;

	[MarshalAs(UnmanagedType.LPStr)]
	public string ModelingUnit;

	[MarshalAs(UnmanagedType.LPStr)]
	public string BpeVocab;

	[MarshalAs(UnmanagedType.LPStr)]
	public string TokensBuf;

	public int TokensBufSize;

	public OnlineModelConfig()
	{
		Transducer = new OnlineTransducerModelConfig();
		Paraformer = new OnlineParaformerModelConfig();
		Zipformer2Ctc = new OnlineZipformer2CtcModelConfig();
		Tokens = "";
		NumThreads = 1;
		Provider = "cpu";
		Debug = 0;
		ModelType = "";
		ModelingUnit = "cjkchar";
		BpeVocab = "";
		TokensBuf = "";
		TokensBufSize = 0;
	}
}
