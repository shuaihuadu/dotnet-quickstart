namespace UnitTests.ScoreCalculator;

public class ScoreCalculator_Test(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {
        // 示例题目  
        var topic = new Topic
        {
            TotalScore = 10,
            CoreParameters = new List<string> { "核心1", "核心2", "核心3", "核心4", "核心5" },
            SceneDialogues = new List<string> { "场景1", "场景2", "场景3", "场景4", "场景5", "场景6", "场景7" }
        };

        var scores = ScoreCalculator.CalculateScores(topic);

        // 打印结果  
        foreach (var score in scores)
        {
            Console.WriteLine($"{score.Key}: {score.Value:F1} 分");
        }
    }
}
