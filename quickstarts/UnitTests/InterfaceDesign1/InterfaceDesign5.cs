namespace UnitTests.InterfaceDesign5;

// 定义 NodeExecutionResult 和 NodeExecutionContext 类  
public class NodeExecutionResult<T>
{
    public bool Success { get; set; }
    public T Output { get; set; }
    // 其他属性和方法  
}

public class NodeExecutionContext
{
    // 上下文属性和方法  
}

public enum NodeStatus
{
    Saved = 1,
    Published = 2,
    CancelPublished = 3
}
public enum NodeType
{
    None
}

/// <summary>
/// 通用的节点接口，所有节点需要实现这个接口
/// </summary>
public interface INode
{
    string Id { get; set; }
    string Name { get; set; }
    NodeType Type { get; set; }
    NodeStatus State { get; set; }
    Task<NodeExecutionResult<object>> ExecuteAsync(NodeExecutionContext context);
}
/// <summary>
/// <inheritdoc />
/// </summary>
/// <typeparam name="TOutput">节点输出数据的类型</typeparam>
public interface INode<TOutput>
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns>节点执行结果</returns>
    new Task<NodeExecutionResult<TOutput>> ExecuteAsync(NodeExecutionContext context);
}
public abstract class NodeBase : INode
{
    public string Id { get; set; }
    public string Name { get; set; }
    public NodeType Type { get; set; }
    public NodeStatus State { get; set; }
    public double Version { get; set; }
    public abstract Task<NodeExecutionResult<object>> ExecuteAsync(NodeExecutionContext context);
}

public abstract class NodeBase<TOutput> : NodeBase, INode<TOutput>
{
    public abstract Task<NodeExecutionResult<TOutput>> ExecuteNodeAsync(NodeExecutionContext context);

    public override async Task<NodeExecutionResult<object>> ExecuteAsync(NodeExecutionContext context)
    {
        var result = await ExecuteNodeAsync(context);
        return new NodeExecutionResult<object>
        {
            Success = result.Success,
            Output = result.Output
        };
    }

    Task<NodeExecutionResult<TOutput>> INode<TOutput>.ExecuteAsync(NodeExecutionContext context)
    {
        return ExecuteNodeAsync(context);
    }
}
public class CustomNode : NodeBase
{
    public override Task<NodeExecutionResult<object>> ExecuteAsync(NodeExecutionContext context)
    {
        // 实现节点的具体执行逻辑  
        // 这里只是一个示例，需要根据实际需求进行实现  
        return Task.FromResult(new NodeExecutionResult<object>
        {
            Success = true,
            Output = new object()
        });
    }
}

public class CustomNode<TOutput> : NodeBase<TOutput> where TOutput : new()
{
    public override Task<NodeExecutionResult<TOutput>> ExecuteNodeAsync(NodeExecutionContext context)
    {
        // 实现节点的具体执行逻辑  
        // 这里只是一个示例，需要根据实际需求进行实现  
        return Task.FromResult(new NodeExecutionResult<TOutput>
        {
            Success = true,
            Output = new TOutput()
        });
    }
}

//[Fact]
//public void Run()
//{
//    var customNode = new CustomNode
//    {
//        Id = "1",
//        Name = "Sample Node",
//        Type = NodeType.None,
//        State = NodeStatus.CancelPublished,
//        Version = 1.0
//    };

//    var context = new NodeExecutionContext();
//    var result = await customNode.ExecuteAsync(context);
//    Console.WriteLine($"Execution success: {result.Success}");

//    var customGenericNode = new CustomNode<SampleOutputData>
//    {
//        Id = "2",
//        Name = "Sample Generic Node",
//        Type = NodeType.None,
//        State = NodeStatus.CancelPublished,
//        Input = new NodeInputParameters(),
//        Variables = new NodeVariables(),
//        Version = 1.1
//    };

//    var genericResult = await customGenericNode.ExecuteAsync(context);
//    Console.WriteLine($"Execution success: {genericResult.Success}");
//}

/*
 好的，我可以帮助你创建一个抽象类 `NodeBase` 和一个带泛型的抽象类 `NodeBase<TOutput>`，并提供它们的派生类以及调用示例。下面是具体的代码：  
   
### 1. NodeBase和NodeBase<TOutput>抽象类  
   
```csharp  
using System;  
using System.Threading.Tasks;  
   
namespace IdeaTech.FlowCode  
{  
    public abstract class NodeBase : INode  
    {  
        public string Id { get; set; }  
        public string Name { get; set; }  
        public NodeType Type { get; set; }  
        public NodeState State { get; set; }  
        public NodeInputParameters Input { get; set; }  
        public NodeVariables Variables { get; set; }  
        public double Version { get; set; }  
  
        public NodeInputParameters GetInputReferencedParameters()  
        {  
            // 实现获取引用了别的参数的输入参数的逻辑  
            // 这里只是一个示例，需要根据实际需求进行实现  
            return new NodeInputParameters();  
        }  
  
        public abstract Task<NodeExecutionResult<object>> ExecuteAsync(NodeExecutionContext context);  
    }  
  
    public abstract class NodeBase<TOutput> : NodeBase, INode<TOutput> where TOutput : IOutputData  
    {  
        public abstract Task<NodeExecutionResult<TOutput>> ExecuteNodeAsync(NodeExecutionContext context);  
  
        public override async Task<NodeExecutionResult<object>> ExecuteAsync(NodeExecutionContext context)  
        {  
            var result = await ExecuteNodeAsync(context);  
            return new NodeExecutionResult<object>  
            {  
                Success = result.Success,  
                Output = result.Output  
            };  
        }  
  
        Task<NodeExecutionResult<TOutput>> INode<TOutput>.ExecuteAsync(NodeExecutionContext context)  
        {  
            return ExecuteNodeAsync(context);  
        }  
    }  
}  
```  
   
### 2. NodeBase和NodeBase<TOutput>抽象类的派生类  
   
```csharp  
using System;  
using System.Threading.Tasks;  
   
namespace IdeaTech.FlowCode  
{  
    public class CustomNode : NodeBase  
    {  
        public override Task<NodeExecutionResult<object>> ExecuteAsync(NodeExecutionContext context)  
        {  
            // 实现节点的具体执行逻辑  
            // 这里只是一个示例，需要根据实际需求进行实现  
            return Task.FromResult(new NodeExecutionResult<object>  
            {  
                Success = true,  
                Output = new object()  
            });  
        }  
    }  
  
    public class CustomNode<TOutput> : NodeBase<TOutput> where TOutput : IOutputData, new()  
    {  
        protected override Task<NodeExecutionResult<TOutput>> ExecuteNodeAsync(NodeExecutionContext context)  
        {  
            // 实现节点的具体执行逻辑  
            // 这里只是一个示例，需要根据实际需求进行实现  
            return Task.FromResult(new NodeExecutionResult<TOutput>  
            {  
                Success = true,  
                Output = new TOutput()  
            });  
        }  
    }  
}  
```  
   
### 3. 调用示例  
   
```csharp  
using System;  
using System.Threading.Tasks;  
   
namespace IdeaTech.FlowCode  
{  
    class Program  
    {  
        static async Task Main(string[] args)  
        {  
            var customNode = new CustomNode  
            {  
                Id = "1",  
                Name = "Sample Node",  
                Type = NodeType.SomeType,  
                State = NodeState.Active,  
                Input = new NodeInputParameters(),  
                Variables = new NodeVariables(),  
                Version = 1.0  
            };  
  
            var context = new NodeExecutionContext();  
            var result = await customNode.ExecuteAsync(context);  
            Console.WriteLine($"Execution success: {result.Success}");  
  
            var customGenericNode = new CustomNode<SampleOutputData>  
            {  
                Id = "2",  
                Name = "Sample Generic Node",  
                Type = NodeType.AnotherType,  
                State = NodeState.Inactive,  
                Input = new NodeInputParameters(),  
                Variables = new NodeVariables(),  
                Version = 1.1  
            };  
  
            var genericResult = await customGenericNode.ExecuteAsync(context);  
            Console.WriteLine($"Execution success: {genericResult.Success}");  
        }  
    }  
  
    public class SampleOutputData : IOutputData  
    {  
        // 实现 IOutputData 的具体逻辑  
    }  
}  
```  
   
请根据你的实际需求和具体逻辑来填充 `ExecuteAsync` 和 `ExecuteNodeAsync` 方法的实现细节。以上代码应该可以无编译错误地运行，并且提供了一个如何调用这些类的示例。
 
 */