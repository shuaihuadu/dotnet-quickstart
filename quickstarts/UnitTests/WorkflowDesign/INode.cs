namespace UnitTests.WorkflowDesign;

public interface INode
{
    string Id { get; set; }
    string Name { get; set; }
    NodeState State { get; set; }
    double Version { get; set; }
    Task<NodeExecutionResult<object>> ExecuteAsync(NodeExecutionContext context);
}
public interface INode<T> : INode where T : IOutputData
{
    new Task<NodeExecutionResult<T>> ExecuteAsync(NodeExecutionContext context);
}

public interface IOutputData { }

public enum NodeState
{
    NotStarted = 0,
    InProgress = 1,
    Successed = 2,
    Failed = 3
}

public class NodeExecutionContext
{
    public Dictionary<string, object> Data { get; set; } = [];
}

public class NodeExecutionResult<T>
{
    public T Data { get; set; }
    public NodeState State { get; set; }
    public string Message { get; set; }
}
