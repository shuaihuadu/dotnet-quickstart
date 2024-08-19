namespace UnitTests.ConditionBranchFormV1;

public class ConditionBranch
{
    public int Priority { get; set; }
    public ConditionGroup IfCondition { get; set; }
    public List<ConditionGroup> ElseIfConditions { get; set; } = new List<ConditionGroup>();
    public ConditionGroup ElseCondition { get; set; }

    public string Evaluate(Dictionary<string, string> fieldValues, out List<ConditionGroupEvaluationResult> evaluatedConditions)
    {
        evaluatedConditions = new List<ConditionGroupEvaluationResult>();

        var ifResult = IfCondition.Evaluate(fieldValues);
        evaluatedConditions.Add(ifResult);
        if (ifResult.FinalResult)
        {
            return "If condition met";
        }

        foreach (var elseIfCondition in ElseIfConditions)
        {
            var elseIfResult = elseIfCondition.Evaluate(fieldValues);
            evaluatedConditions.Add(elseIfResult);
            if (elseIfResult.FinalResult)
            {
                return "ElseIf condition met";
            }
        }

        if (ElseCondition != null)
        {
            var elseResult = ElseCondition.Evaluate(fieldValues);
            evaluatedConditions.Add(elseResult);
            if (elseResult.FinalResult)
            {
                return "Else condition met";
            }
        }

        return "No condition met";
    }
}
