namespace UnitTests.Topological;
public enum NodeState
{
    Pending,
    Running,
    Completed,
    Failed
}

public interface INode<T>
{
    string Name { get; set; }
    string Description { get; set; }
    IList<INode<T>> Dependencies { get; }
    IList<INode<T>> Dependents { get; }
    NodeState State { get; set; }
    new NodeExecutionResult<T> Execute(NodeExecutionContext context, Action action);
}
public abstract class NodeBase<T> : INode<T>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<INode<T>> Dependencies { get; } = [];
    public IList<INode<T>> Dependents { get; } = [];
    public NodeState State { get; set; } = NodeState.Pending;

    public abstract NodeExecutionResult<T> Execute(NodeExecutionContext context, Action action);
}
public class IntNode : NodeBase<int>
{
    public override NodeExecutionResult<int> Execute(NodeExecutionContext context, Action action)
    {
        State = NodeState.Running;
        action();
        int result = 42; // 具体逻辑，返回整数  
        context.Data["IntNodeResult"] = result;
        State = NodeState.Completed;
        return new NodeExecutionResult<int>
        {
            Result = result,
            State = State,
            Message = "Execution successful"
        };
    }
}

public class StringNode : NodeBase<int>
{
    public override NodeExecutionResult<int> Execute(NodeExecutionContext context, Action action)
    {
        State = NodeState.Running;
        action();
        string result = "Hello, World!"; // 具体逻辑，返回字符串  
        context.Data["StringNodeResult"] = result;
        State = NodeState.Completed;
        return new NodeExecutionResult<int>
        {
            Result = 100,
            State = State,
            Message = "Execution successful"
        };
    }
}


//public class NodeWrapper<T> : INode
//{
//    private readonly INode<T> _node;

//    public NodeWrapper(INode<T> node)
//    {
//        _node = node;
//    }

//    public string Name
//    {
//        get => _node.Name;
//        set => _node.Name = value;
//    }

//    public string Description
//    {
//        get => _node.Description;
//        set => _node.Description = value;
//    }

//    public IList<INodeBase> Dependencies => _node.Dependencies;

//    public NodeState State
//    {
//        get => _node.State;
//        set => _node.State = value;
//    }

//    public ExecutionResult<object> Execute(ExecutionContext context, Action action)
//    {
//        var result = _node.Execute(context, action);
//        return new ExecutionResult<object>
//        {
//            Result = result.Result,
//            State = result.State,
//            Message = result.Message
//        };
//    }
//}