namespace UnitTests.InterfaceDesign3;
public interface INode
{
    NodeExecutionResult<object> ExecuteAsync(NodeExecutionContext context);
}

public interface INode<T>
{
    NodeExecutionResult<T> ExecuteAsync(NodeExecutionContext context);
}
public abstract class NodeBase<T> : INode, INode<T>
{
    // 实现 INode<T> 接口的方法  
    public abstract NodeExecutionResult<T> ExecuteAsync(NodeExecutionContext context);

    // 实现 INode 接口的方法  
    NodeExecutionResult<object> INode.ExecuteAsync(NodeExecutionContext context)
    {
        // 调用泛型方法并将结果转换为 object  
        var result = ExecuteAsync(context);
        return new NodeExecutionResult<object>
        {
            Result = result.Result,
            Success = result.Success,
            ErrorMessage = result.ErrorMessage
        };
    }
}
public class ConcreteNode : NodeBase<string>
{
    public override NodeExecutionResult<string> ExecuteAsync(NodeExecutionContext context)
    {
        // 实现具体的逻辑  
        return new NodeExecutionResult<string>
        {
            Result = "Hello, World!",
            Success = true,
            ErrorMessage = null
        };
    }
}

public class NodeExecutionResult<T>
{
    public T Result { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
}

public class NodeExecutionContext
{
    // 定义上下文的属性和方法  
}
