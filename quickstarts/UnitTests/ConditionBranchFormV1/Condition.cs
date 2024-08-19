namespace UnitTests.ConditionBranchFormV1;
public class Condition
{
    public string Field { get; set; }
    public ConditionType Type { get; set; }
    public string Value { get; set; }

    public bool Evaluate(string fieldValue)
    {
        switch (Type)
        {
            case ConditionType.Equals:
                return fieldValue == Value;
            case ConditionType.NotEquals:
                return fieldValue != Value;
            case ConditionType.LengthGreaterThan:
                return fieldValue.Length > int.Parse(Value);
            case ConditionType.LengthGreaterThanOrEqual:
                return fieldValue.Length >= int.Parse(Value);
            case ConditionType.LengthLessThan:
                return fieldValue.Length < int.Parse(Value);
            case ConditionType.LengthLessThanOrEqual:
                return fieldValue.Length <= int.Parse(Value);
            case ConditionType.Contains:
                return fieldValue.Contains(Value);
            case ConditionType.NotContains:
                return !fieldValue.Contains(Value);
            case ConditionType.IsEmpty:
                return string.IsNullOrEmpty(fieldValue);
            case ConditionType.IsNotEmpty:
                return !string.IsNullOrEmpty(fieldValue);
            default:
                throw new NotSupportedException($"Condition type {Type} is not supported.");
        }
    }

    public override string ToString()
    {
        return $"{Field} {Type} {Value}";
    }
}
