namespace UnitTests.ConditionBranchFormV2;

/// <summary>
/// 表示各种比较操作的枚举
/// </summary>
public enum ComparisonOperation
{
    /// <summary>
    /// 等于
    /// </summary>
    Equals,
    /// <summary>
    /// 不等于
    /// </summary>
    NotEquals,
    /// <summary>
    /// 长度大于
    /// </summary>
    LengthGreaterThan,
    /// <summary>
    /// 长度大于等于
    /// </summary>
    LengthGreaterThanOrEqual,
    /// <summary>
    /// 长度小于
    /// </summary>
    LengthLessThan,
    /// <summary>
    /// 长度小于等于
    /// </summary>
    LengthLessThanOrEqual,
    /// <summary>
    /// 包含
    /// </summary>
    Contains,
    /// <summary>
    /// 不包含
    /// </summary>
    NotContains,
    /// <summary>
    /// 为空
    /// </summary>
    IsEmpty,
    /// <summary>
    /// 不为空
    /// </summary>
    IsNotEmpty
}

/// <summary>
/// 表示逻辑操作（AND, OR）的枚举
/// </summary>
public enum LogicalOperation
{
    And,
    Or
}

/// <summary>
/// 表示分支评估结果的类
/// </summary>
public class ConditionBranchEvaluationResult
{
    /// <summary>
    /// 获取或设置分支评估的结果
    /// </summary>
    public bool BranchResult { get; set; }

    /// <summary>
    /// 获取或设置分支中的条件结果列表
    /// </summary>
    public List<ConditionResult> ConditionResults { get; set; } = new List<ConditionResult>();
}

/// <summary>
/// 条件评估结果
/// </summary>
public class ConditionResult
{
    /// <summary>
    /// 获取或设置条件评估的结果
    /// </summary>
    public bool Result { get; set; }

    /// <summary>
    /// 获取或设置条件评估的描述
    /// </summary>
    public string Description { get; set; } = null!;
}

/// <summary>
/// 要评估的条件
/// </summary>
public class Condition
{
    /// <summary>
    /// 获取或设置要评估的变量
    /// </summary>
    public string Left { get; set; } = null!;

    /// <summary>
    /// 获取或设置要执行的比较操作
    /// </summary>
    public ComparisonOperation Operation { get; set; }

    /// <summary>
    /// 获取或设置要比较的值
    /// </summary>
    public string Right { get; set; } = null!;

    /// <summary>
    /// 根据提供的变量评估条件
    /// </summary>
    /// <returns>包含评估结果和描述的 ConditionResult 对象</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ConditionResult Evaluate()
    {
        var result = Operation switch
        {
            ComparisonOperation.Equals => string.Equals(Left, Right, StringComparison.OrdinalIgnoreCase),
            ComparisonOperation.NotEquals => !string.Equals(Left, Right, StringComparison.OrdinalIgnoreCase),
            ComparisonOperation.LengthGreaterThan => Right.IsInt32() && Left.Length > int.Parse(Right),
            ComparisonOperation.LengthGreaterThanOrEqual => Right.IsInt32() && Left.Length >= int.Parse(Right),
            ComparisonOperation.LengthLessThan => Right.IsInt32() && Left.Length < int.Parse(Right),
            ComparisonOperation.LengthLessThanOrEqual => Right.IsInt32() && Left.Length <= int.Parse(Right),
            ComparisonOperation.Contains => Left.IndexOf(Right, StringComparison.OrdinalIgnoreCase) >= 0,
            ComparisonOperation.NotContains => Left.IndexOf(Right, StringComparison.OrdinalIgnoreCase) < 0,
            ComparisonOperation.IsEmpty => string.IsNullOrEmpty(Left),
            ComparisonOperation.IsNotEmpty => !string.IsNullOrEmpty(Left),
            _ => throw new InvalidOperationException("未知的比较操作"),
        };

        return new ConditionResult
        {
            Result = result,
            Description = $"判断条件: {Left} {Operation} {Right}, 结果: {result}"
        };
    }
}

/// <summary>
/// 表示要评估的条件分支
/// </summary>
public class ConditionBranch
{
    /// <summary>
    /// 获取或设置分支的优先级
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// 获取或设置分支中的条件列表
    /// </summary>
    public List<Condition> Conditions { get; set; } = [];

    /// <summary>
    /// 获取或设置用于组合条件的逻辑操作
    /// </summary>
    public LogicalOperation LogicalOperation { get; set; }

    public ConditionBranchEvaluationResult Evaluate()
    {
        var results = new List<ConditionResult>();
        bool branchResult;

        if (LogicalOperation == LogicalOperation.And)
        {
            branchResult = Conditions.TrueForAll(condition =>
            {
                ConditionResult conditionResult = condition.Evaluate();
                results.Add(conditionResult);
                return conditionResult.Result;
            });
        }
        else //"或"逻辑处理
        {
            branchResult = Conditions.Exists(condition =>
            {
                ConditionResult conditionResult = condition.Evaluate();
                results.Add(conditionResult);
                return conditionResult.Result;
            });
        }

        return new ConditionBranchEvaluationResult
        {
            BranchResult = branchResult,
            ConditionResults = results
        };
    }
}

/// <summary>
/// 条件分支评估结果
/// </summary>
public class EvaluationResult
{
    /// <summary>
    /// 获取或设置分支的优先级
    /// </summary>
    public int BranchPriority { get; set; }

    /// <summary>
    /// 获取或设置分支评估的结果
    /// </summary>
    public bool BranchResult { get; set; }

    /// <summary>
    /// 获取或设置分支中的条件结果列表
    /// </summary>
    public List<ConditionResult> ConditionResults { get; set; } = [];

    /// <summary>
    /// 获取或设置分支的逻辑操作
    /// </summary>
    public LogicalOperation LogicalOperation { get; set; }
}

/// <summary>
/// 负责评估多个条件分支
/// </summary>
public class ConditionEvaluator
{
    private readonly List<ConditionBranch> _branches = [];

    /// <summary>
    /// 向评估器添加条件分支
    /// </summary>
    /// <param name="branch">要添加的条件分支</param>
    public void AddBranch(ConditionBranch branch)
    {
        this._branches.Add(branch);
    }

    /// <summary>
    /// 评估所有条件分支
    /// </summary>
    /// <returns>包含分支评估结果的 EvaluationResult 对象列表。</returns>
    public List<EvaluationResult> Evaluate()
    {
        List<EvaluationResult> results = [];

        foreach (var branch in this._branches.OrderBy(b => b.Priority))
        {
            ConditionBranchEvaluationResult branchResult = branch.Evaluate();

            results.Add(new EvaluationResult
            {
                BranchPriority = branch.Priority,
                BranchResult = branchResult.BranchResult,
                ConditionResults = branchResult.ConditionResults,
                LogicalOperation = branch.LogicalOperation
            });

            if (branchResult.BranchResult)
            {
                break;
            }
        }

        return results;
    }
}

public static class StringExtensions
{
    public static bool IsInt32(this string value)
    {
        return int.TryParse(value, out _);
    }
}