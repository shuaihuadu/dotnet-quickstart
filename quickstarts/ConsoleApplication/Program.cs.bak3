using CSCore;
using CSCore.Codecs.WAV;

class Program
{
    static void Main()
    {
        // 假设你的音频数据存储在 byte[] audioData 中  
        byte[] audioData = GetAudioData(); // 你需要实现这个方法来获取你的音频数据  

        // 设置音频格式  
        int samplesPerSecond = 16000;
        int bitsPerSample = 16;
        int channels = 1;

        // 创建 WaveFormat  
        WaveFormat waveFormat = new WaveFormat(samplesPerSecond, bitsPerSample, channels);

        // 创建 MemoryStream 并写入音频数据  
        using (MemoryStream memoryStream = new MemoryStream())
        {
            // 创建 WaveWriter  
            using (WaveWriter waveWriter = new WaveWriter(memoryStream, waveFormat))
            {
                // 写入音频数据  
                waveWriter.Write(audioData, 0, audioData.Length);
            }

            // 将 MemoryStream 转换成 byte[]  
            byte[] wavData = memoryStream.ToArray();

            // 将 byte[] 写入文件  
            File.WriteAllBytes("output.wav", wavData);
        }

        Console.WriteLine("WAV 文件已成功创建！");
    }

    static byte[] GetAudioData()
    {
        // 这里你需要实现获取音频数据的逻辑  
        // 例如，从文件读取或从网络获取  
        // 这里仅作为示例返回一个空数组  
        return new byte[0];
    }
}
