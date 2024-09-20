namespace UnitTests.ScoreCalculator;

public class ScoreCalculator2_Test(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {
        decimal inputNumber = 102.4m;
        // 按照2:3的比例分成两个数  
        decimal part1 = inputNumber * 2 / 5;
        decimal part2 = inputNumber * 3 / 5;

        // 输出结果  
        Console.WriteLine($"按照2:3的比例分成两个数: {part1} 和 {part2}");
    }

    [Fact]
    public void RunDivideScore()
    {
        decimal score = 7.3m;
        int n = 3;

        List<decimal> dividedScores = DivideScore(score, n);

        Console.WriteLine("分数被分为以下部分：");

        foreach (var part in dividedScores)
        {
            Console.WriteLine(part);
        }
    }

    /// <summary>
    /// 实现功能：将一个小数分为n(整数)部分
    /// 如果能完整均分，则完整均分
    /// 如果不能完整均分，则将均分后的余数追加到均分后的第一个结果中
    /// 实现思路：
    /// 1. 将小数转换为整数（使用乘以10或者10的倍数的方法）
    /// 2.然后使用转换后的整数对n取模
    /// 3.如果取模为0则表示可以均分，否则不能均分，就需要将余数追加
    /// 4.再将对应的结果转换成小数
    /// </summary>
    /// <param name="score"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static List<decimal> DivideScore(decimal score, int n)
    {
        // 将小数转换为整数
        int multiplier = 1;
        while (score * multiplier % 1 != 0)
        {
            multiplier *= 10;
        }
        int intScore = (int)(score * multiplier);

        // 计算每部分的整数值和余数
        int quotient = intScore / n;
        int remainder = intScore % n;

        // 创建结果列表
        List<decimal> result = [];

        // 如果能完整均分
        if (remainder == 0)
        {
            for (int i = 0; i < n; i++)
            {
                result.Add((decimal)quotient / multiplier);
            }
        }
        else
        {
            // 不能完整均分，将余数追加到第一个结果中
            result.Add((decimal)(quotient + remainder) / multiplier);
            for (int i = 1; i < n; i++)
            {
                result.Add((decimal)quotient / multiplier);
            }
        }

        return result;
    }
}
