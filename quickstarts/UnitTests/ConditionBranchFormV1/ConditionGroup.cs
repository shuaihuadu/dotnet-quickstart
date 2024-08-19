namespace UnitTests.ConditionBranchFormV1;

public class ConditionGroup
{
    public List<Condition> Conditions { get; set; } = new List<Condition>();
    public bool IsAndGroup { get; set; } = true;

    public ConditionGroupEvaluationResult Evaluate(Dictionary<string, string> fieldValues)
    {
        var result = new ConditionGroupEvaluationResult();

        if (IsAndGroup)
        {
            result.FinalResult = Conditions.All(c =>
            {
                var conditionResult = c.Evaluate(fieldValues[c.Field]);
                result.ConditionResults.Add(new ConditionEvaluationResult { Condition = c, Result = conditionResult });
                return conditionResult;
            });
        }
        else
        {
            result.FinalResult = Conditions.Any(c =>
            {
                var conditionResult = c.Evaluate(fieldValues[c.Field]);
                result.ConditionResults.Add(new ConditionEvaluationResult { Condition = c, Result = conditionResult });
                return conditionResult;
            });
        }

        return result;
    }
}
