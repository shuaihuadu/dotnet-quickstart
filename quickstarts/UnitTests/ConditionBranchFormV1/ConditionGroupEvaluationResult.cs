namespace UnitTests.ConditionBranchFormV1;

public class ConditionGroupEvaluationResult
{
    public List<ConditionEvaluationResult> ConditionResults { get; set; } = new List<ConditionEvaluationResult>();
    public bool FinalResult { get; set; }

    public override string ToString()
    {
        return $"Final Result: {FinalResult}, Conditions: [{string.Join(", ", ConditionResults)}]";
    }
}