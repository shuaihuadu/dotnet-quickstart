namespace UnitTests.ConditionBranchFormV1;

public class Form
{
    public List<ConditionBranch> ConditionBranches { get; set; } = new List<ConditionBranch>();

    public string Evaluate(Dictionary<string, string> fieldValues, out List<ConditionGroupEvaluationResult> evaluatedConditions)
    {
        evaluatedConditions = new List<ConditionGroupEvaluationResult>();

        foreach (var branch in ConditionBranches.OrderBy(b => b.Priority))
        {
            var result = branch.Evaluate(fieldValues, out var branchEvaluatedConditions);
            evaluatedConditions.AddRange(branchEvaluatedConditions);
            if (result != "No condition met")
            {
                evaluatedConditions.Add(new ConditionGroupEvaluationResult
                {
                    FinalResult = true,
                    ConditionResults = new List<ConditionEvaluationResult>
                    {
                        new ConditionEvaluationResult { Condition = new Condition { Field = "Branch", Type = ConditionType.Equals, Value = branch.Priority.ToString() }, Result = true }
                    }
                });
                return result;
            }
        }

        return "No condition met in any branch";
    }
}

public class ConditionBranchFormTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {

        var form = new Form();

        var ifCondition1 = new ConditionGroup
        {
            Conditions = new List<Condition>
            {
                new Condition { Field = "Name", Type = ConditionType.Equals, Value = "John" }
            },
            IsAndGroup = true
        };

        var elseIfCondition1 = new ConditionGroup
        {
            Conditions = new List<Condition>
            {
                new Condition { Field = "Age", Type = ConditionType.LengthGreaterThan, Value = "18" }
            },
            IsAndGroup = true
        };

        var elseCondition1 = new ConditionGroup
        {
            Conditions = new List<Condition>
            {
                new Condition { Field = "Country", Type = ConditionType.Equals, Value = "USA" }
            },
            IsAndGroup = true
        };

        var branch1 = new ConditionBranch
        {
            Priority = 1,
            IfCondition = ifCondition1,
            ElseIfConditions = new List<ConditionGroup> { elseIfCondition1 },
            ElseCondition = elseCondition1
        };

        var ifCondition2 = new ConditionGroup
        {
            Conditions = new List<Condition>
            {
                new Condition { Field = "Name", Type = ConditionType.Equals, Value = "Alice" }
            },
            IsAndGroup = true
        };

        var elseIfCondition2 = new ConditionGroup
        {
            Conditions = new List<Condition>
            {
                new Condition { Field = "Age", Type = ConditionType.LengthGreaterThan, Value = "25" }
            },
            IsAndGroup = true
        };

        var elseCondition2 = new ConditionGroup
        {
            Conditions = new List<Condition>
            {
                new Condition { Field = "Country", Type = ConditionType.Equals, Value = "Canada" }
            },
            IsAndGroup = true
        };

        var branch2 = new ConditionBranch
        {
            Priority = 2,
            IfCondition = ifCondition2,
            ElseIfConditions = new List<ConditionGroup> { elseIfCondition2 },
            ElseCondition = elseCondition2
        };

        form.ConditionBranches.Add(branch1);
        form.ConditionBranches.Add(branch2);

        var fieldValues = new Dictionary<string, string>
        {
            { "Name", "Alice" },
            { "Age", "20" },
            { "Country", "Canada" }
        };

        var result = form.Evaluate(fieldValues, out var evaluatedConditions);
        Console.WriteLine(result); // Output: If condition met (from branch2 because it has higher priority)  
        Console.WriteLine("Evaluated Conditions:");
        foreach (var condition in evaluatedConditions)
        {
            Console.WriteLine(condition);
        }

    }
}
