namespace UnitTests.Topological;


public class NodeExecutionContext
{
    public Dictionary<string, object> Data { get; set; } = [];
}

public class NodeExecutionResult<T>
{
    public T Result { get; set; }
    public NodeState State { get; set; }
    public string Message { get; set; } // 可选的消息，例如错误信息或日志  
}
