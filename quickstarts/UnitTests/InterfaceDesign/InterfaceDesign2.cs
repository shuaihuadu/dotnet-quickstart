namespace UnitTests.InterfaceDesign2;

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

public abstract class NodeBase : INode
{
    public NodeExecutionResult<object> ExecuteAsync(NodeExecutionContext context)
    {
        try
        {
            // 调用模板方法，执行具体的节点逻辑  
            var result = ExecuteInternal(context);
            return new NodeExecutionResult<object>
            {
                Result = result,
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new NodeExecutionResult<object>
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    // 模板方法，子类需要实现具体的逻辑  
    protected abstract object ExecuteInternal(NodeExecutionContext context);
}

public abstract class NodeBase<T> : NodeBase, INode<T>
{
    public new virtual NodeExecutionResult<T> ExecuteAsync(NodeExecutionContext context)
    {
        try
        {
            // 调用模板方法，执行具体的节点逻辑  
            var result = ExecuteInternalTyped(context);
            return new NodeExecutionResult<T>
            {
                Result = result,
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new NodeExecutionResult<T>
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    // 重载模板方法，子类需要实现具体的逻辑  
    protected abstract T ExecuteInternalTyped(NodeExecutionContext context);

    // 覆盖基类的模板方法，调用泛型版本  
    protected override object ExecuteInternal(NodeExecutionContext context)
    {
        return ExecuteInternalTyped(context);
    }
}
