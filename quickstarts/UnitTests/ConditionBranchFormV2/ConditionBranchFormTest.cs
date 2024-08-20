namespace UnitTests.ConditionBranchFormV2;

public class ConditionBranchFormTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {
        ConditionEvaluator evaluator = new();

        ConditionBranch branch1 = new()
        {
            Priority = 1,
            LogicalOperation = LogicalOperation.And,
            Conditions =
            [
                new Condition { Left = "John", Operation = ComparisonOperation.Equals, Right = "John" },
                new Condition { Left = "Age", Operation = ComparisonOperation.LengthGreaterThan, Right = "a20" }
            ]
        };

        ConditionBranch branch2 = new()
        {
            Priority = 2,
            LogicalOperation = LogicalOperation.Or,
            Conditions =
            [
                new Condition { Left = "Doe", Operation = ComparisonOperation.Contains, Right = "Doe" },
                new Condition { Left = "Age", Operation = ComparisonOperation.LengthLessThan, Right = "a23" }
            ]
        };

        ConditionBranch falseBranch = new()
        {
            Priority = 3,
            LogicalOperation = LogicalOperation.Or,
            Conditions =
            [
                new Condition{ Left = "1",Operation = ComparisonOperation.Equals,Right = "1" }
            ]
        };

        evaluator.AddBranch(branch1);
        evaluator.AddBranch(branch2);
        evaluator.AddBranch(falseBranch);

        var results = evaluator.Evaluate();

        foreach (var result in results)
        {
            Console.WriteLine($"优先级为[{result.BranchPriority}]的条件分支，逻辑操作：[{result.LogicalOperation}]，执行结果为：[{result.BranchResult}]");

            foreach (var conditionResult in result.ConditionResults)
            {
                Console.WriteLine(conditionResult.Description);
            }

            if (result.BranchResult)
            {
                Console.WriteLine($"优先级为[{result.BranchPriority}]的条件分支被执行");
            }
        }
    }
}