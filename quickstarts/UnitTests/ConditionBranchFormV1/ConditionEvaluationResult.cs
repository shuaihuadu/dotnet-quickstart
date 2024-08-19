namespace UnitTests.ConditionBranchFormV1;

public class ConditionEvaluationResult
{
    public Condition Condition { get; set; }
    public bool Result { get; set; }

    public override string ToString()
    {
        return $"{Condition} => {Result}";
    }
}
