using Microsoft.CognitiveServices.Speech;

namespace SpeechToTextExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string subscriptionKey = "";
            string serviceRegion = "";

            var config = SpeechConfig.FromSubscription(subscriptionKey, serviceRegion);

            config.SpeechRecognitionLanguage = "zh-CN";

            using var recognizer = new SpeechRecognizer(config);

            Console.WriteLine("请说话...");

            recognizer.Recognizing += (s, e) =>
            {
                Console.WriteLine($"正在识别: {e.Result.Text}");
            };

            recognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"已识别的内容: {e.Result.Text}");
                }
                else if (e.Result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine("未识别到语音内容.");
                }
            };

            recognizer.Canceled += (s, e) =>
            {
                Console.WriteLine($"识别已取消：Reason={e.Reason}");

                if (e.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"错误信息: {e.ErrorDetails}");
                }
            };

            recognizer.SessionStarted += (s, e) =>
            {
                Console.WriteLine("会话开始.");
            };

            recognizer.SessionStopped += (s, e) =>
            {
                Console.WriteLine("会话结束.");
                Console.WriteLine("Press any key to exit...");
            };

            await recognizer.StartContinuousRecognitionAsync();

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            await recognizer.StopContinuousRecognitionAsync();
        }
    }
}