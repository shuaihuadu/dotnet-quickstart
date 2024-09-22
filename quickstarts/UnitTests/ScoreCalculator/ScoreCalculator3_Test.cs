namespace UnitTests.ScoreCalculator;

public class ScoreCalculator3_Test(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {
        var result = SplitValue(2, 3);
        //var result = SplitValue(10, 5);
        //var result = SplitValue(3.2m, 3);

        foreach (var item in result)
        {
            this.Console.WriteLine(item);
        }
    }

    /// <summary>
    /// 将一个数值分成n份，如果能均分则均分，如果不能均分，则将不能均分的部分加到第一份
    /// </summary>
    /// <param name="value">要分割的数值，可能是小数也可能是整数。</param>
    /// <param name="n">要分成的份数。</param>
    /// <returns>分割后的数值列表。</returns>
    static List<decimal> SplitValue(decimal value, int n)
    {
        // 创建一个列表来存储分割后的结果  
        List<decimal> result = [];

        // 计算每一份的基本值，保留两位小数  
        decimal baseValue = Math.Floor(value / n * 10) / 10;

        // 计算剩余的部分，即不能均分的部分  
        decimal remainder = value - (baseValue * n);

        // 将基本值添加到结果列表中  
        for (int i = 0; i < n; i++)
        {
            result.Add(baseValue);
        }

        // 将不能均分的部分加到第一份  
        result[0] += remainder;

        return result;
    }
}
