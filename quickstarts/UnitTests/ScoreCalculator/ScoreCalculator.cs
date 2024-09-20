namespace UnitTests.ScoreCalculator;

internal class ScoreCalculator
{
    public static Dictionary<string, decimal> CalculateScores(Topic topic)
    {
        var result = new Dictionary<string, decimal>();

        // 计算核心参数和场景话术的总分  
        decimal totalCoreScore = (topic.TotalScore * 2) / 5;
        decimal totalSceneScore = topic.TotalScore - totalCoreScore;

        // 计算每个核心参数的分数  
        int coreCount = topic.CoreParameters.Count;
        decimal coreBaseScore = totalCoreScore / coreCount;
        decimal remainingCoreScore = totalCoreScore - coreBaseScore * coreCount;

        for (int i = 0; i < coreCount; i++)
        {
            result[topic.CoreParameters[i]] = coreBaseScore;
        }

        // 如果有余数，优先第一个考点分值更高  
        if (remainingCoreScore > 0)
        {
            result[topic.CoreParameters[0]] += remainingCoreScore;
        }

        // 计算每个场景话术的分数  
        int sceneCount = topic.SceneDialogues.Count;
        decimal sceneBaseScore = totalSceneScore / sceneCount;
        decimal remainingSceneScore = totalSceneScore - sceneBaseScore * sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            result[topic.SceneDialogues[i]] = sceneBaseScore;
        }

        // 如果有余数，优先第一个考点分值更高  
        if (remainingSceneScore > 0)
        {
            result[topic.SceneDialogues[0]] += remainingSceneScore;
        }

        return result;
    }
}
