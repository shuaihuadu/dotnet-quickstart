namespace UnitTests.InterfaceDesign1;
public interface INode
{
    NodeExecutionResult<object> ExecuteAsync(NodeExecutionContext context);
}

public interface INode<T> : INode
{
    new NodeExecutionResult<T> ExecuteAsync(NodeExecutionContext context);
}
public class NodeExecutionResult<T>
{
    public T Result { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
}

public class NodeExecutionContext
{
    // 定义上下文属性  
}
public abstract class NodeBase<T> : INode<T>
{
    public abstract NodeExecutionResult<T> ExecuteAsync(NodeExecutionContext context);

    NodeExecutionResult<object> INode.ExecuteAsync(NodeExecutionContext context)
    {
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

public class DesignTest
{
    [Fact]
    public void Run()
    {
        INode<string> concreteNode = new ConcreteNode();
        INode node = concreteNode; // 由于 ConcreteNode 继承自 NodeBase<string>，它同时实现了 INode<string> 和 INode  

        NodeExecutionContext context = new NodeExecutionContext();

        // 使用泛型接口  
        NodeExecutionResult<string> genericResult = concreteNode.ExecuteAsync(context);
        Console.WriteLine($"Generic Result: {genericResult.Result}");

        // 使用非泛型接口  
        NodeExecutionResult<object> result = node.ExecuteAsync(context);
        Console.WriteLine($"Non-Generic Result: {result.Result}");
    }
}