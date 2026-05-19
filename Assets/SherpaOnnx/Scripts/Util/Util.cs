using System;
using System.IO;
using SherpaOnnx;
using UnityEngine;

public static class Util
{
    public static string GetPath()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor
            || Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.LinuxEditor
             || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            return Application.streamingAssetsPath;
        }
        else if (Application.platform == RuntimePlatform.Android
                  || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return Application.persistentDataPath;
        }
        else
        {
            return Application.streamingAssetsPath;
        }
    }

    public static void SaveClip(int channels, int frequency, float[] data, string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                // 写入RIFF头部标识
                writer.Write("RIFF".ToCharArray());
                // 写入文件总长度（后续填充）
                writer.Write(0);
                writer.Write("WAVE".ToCharArray());
                // 写入fmt子块
                writer.Write("fmt ".ToCharArray());
                writer.Write(16); // PCM格式块长度
                writer.Write((short)1); // PCM编码类型
                writer.Write((short)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2); // 字节率
                writer.Write((short)(channels * 2)); // 块对齐
                writer.Write((short)16); // 位深度
                                         // 写入data子块
                writer.Write("data".ToCharArray());
                writer.Write(data.Length * 2); // 音频数据字节数
                                               // 写入PCM数据（float转为short）
                foreach (float sample in data)
                {
                    writer.Write((short)(sample * 32767));
                }
                // 返回填充文件总长度
                fileStream.Position = 4;
                writer.Write((int)(fileStream.Length - 8));
            }
        }
    }

    public static float[] ComputeEmbedding(SpeakerEmbeddingExtractor extractor, int sample, float[] data)
    {
        var stream = extractor.CreateStream();
        stream.AcceptWaveform(sample, data);
        stream.InputFinished();
        var embedding = extractor.Compute(stream);
        return embedding;
    }

    public static float[] BytesToFloat(byte[] byteArray)
    {
        float[] sounddata = new float[byteArray.Length / 2];
        for (int i = 0; i < sounddata.Length; i++)
        {
            sounddata[i] = BytesToFloat(byteArray[i * 2], byteArray[i * 2 + 1]);
        }
        return sounddata;
    }

    private static float BytesToFloat(byte firstByte, byte secondByte)
    {
        //小端和大端顺序要调整
        short s;
        if (BitConverter.IsLittleEndian)
        {
            s = (short)((secondByte << 8) | firstByte);
        }
        else
        {
            s = (short)((firstByte << 8) | secondByte);
        }
        // convert to range from -1 to (just below) 1
        return s / 32768.0F;
    }

    public static float[] ReadMono16kWavToFloat(string filePath)
    {
        using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new BinaryReader(fs);

        string riff = new string(reader.ReadChars(4));
        int fileSize = reader.ReadInt32();
        string wave = new string(reader.ReadChars(4));
        string fmt = new string(reader.ReadChars(4));
        int fmtSize = reader.ReadInt32();

        short audioFormat = reader.ReadInt16();
        short numChannels = reader.ReadInt16();
        int fileSampleRate = reader.ReadInt32();
        int byteRate = reader.ReadInt32();
        short blockAlign = reader.ReadInt16();
        short bitsPerSample = reader.ReadInt16();

        if (riff != "RIFF" || wave != "WAVE" || fmt != "fmt ")
            throw new Exception("无效的WAV文件头");
        if (fmtSize > 16)
            reader.ReadBytes(fmtSize - 16);

        string dataChunkId;
        do
        {
            dataChunkId = new string(reader.ReadChars(4));
            if (dataChunkId != "data")
                reader.ReadBytes(reader.ReadInt32());
        } while (dataChunkId != "data");

        int dataSize = reader.ReadInt32();

        if (audioFormat != 1) throw new Exception("仅支持PCM格式");
        if (numChannels != 1) throw new Exception("仅支持单声道音频");
        if (fileSampleRate != 16000) throw new Exception("仅支持16kHz采样率");
        if (bitsPerSample != 16) throw new Exception("仅支持16位采样深度");

        int sampleCount = dataSize / 2;
        float[] floatData = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            byte lo = reader.ReadByte();
            byte hi = reader.ReadByte();
            short pcm = (short)((hi << 8) | lo);
            floatData[i] = pcm / 32768.0f;
        }

        return floatData;
    }

    public static float[] ReadMono24kWavToFloat(string filePath)
    {
        using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new BinaryReader(fs);

        string riff = new string(reader.ReadChars(4));
        int fileSize = reader.ReadInt32();
        string wave = new string(reader.ReadChars(4));
        string fmt = new string(reader.ReadChars(4));
        int fmtSize = reader.ReadInt32();

        short audioFormat = reader.ReadInt16();
        short numChannels = reader.ReadInt16();
        int fileSampleRate = reader.ReadInt32();
        int byteRate = reader.ReadInt32();
        short blockAlign = reader.ReadInt16();
        short bitsPerSample = reader.ReadInt16();

        if (riff != "RIFF" || wave != "WAVE" || fmt != "fmt ")
            throw new Exception("无效的WAV文件头");
        if (fmtSize > 16)
            reader.ReadBytes(fmtSize - 16);

        string dataChunkId;
        do
        {
            dataChunkId = new string(reader.ReadChars(4));
            if (dataChunkId != "data")
                reader.ReadBytes(reader.ReadInt32());
        } while (dataChunkId != "data");

        int dataSize = reader.ReadInt32();

        // 修改点1：允许采样率为 24000
        if (audioFormat != 1) throw new Exception("仅支持PCM格式");
        if (numChannels != 1) throw new Exception("仅支持单声道音频");
        if (fileSampleRate != 24000) throw new Exception("仅支持24kHz采样率");
        if (bitsPerSample != 16) throw new Exception("仅支持16位采样深度");

        int sampleCount = dataSize / 2;
        float[] floatData = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            byte lo = reader.ReadByte();
            byte hi = reader.ReadByte();
            short pcm = (short)((hi << 8) | lo);
            floatData[i] = pcm / 32768.0f;
        }

        return floatData;
    }
}