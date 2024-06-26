namespace UnitTests.WorkflowDesign.Design1;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// 定义IOutputData接口  
public interface IOutputData
{
    void Display();
}

// 定义Result类来封装执行结果和错误信息  
public class Result<T> where T : IOutputData
{
    public T Data { get; set; }
    public Exception Error { get; set; }
    public bool IsSuccess => Error == null;

    public static Result<T> Success(T data) => new Result<T> { Data = data };
    public static Result<T> Failure(Exception error) => new Result<T> { Error = error };
}

// 定义非泛型的基接口  
public interface INode
{
    Task<Result<IOutputData>> ExecuteAsync();
}

// 定义泛型接口，继承非泛型基接口  
public interface INode<T> : INode where T : IOutputData
{
    new Task<Result<T>> ExecuteAsync();
}

// 实现一个具体的输出数据类  
public class ConcreteOutputData : IOutputData
{
    public string Data { get; set; }

    public void Display()
    {
        Console.WriteLine($"Output Data: {Data}");
    }
}

// 实现一个具体的节点类  
public class ConcreteNode : INode<ConcreteOutputData>
{
    public async Task<Result<ConcreteOutputData>> ExecuteAsync()
    {
        try
        {
            // 模拟异步操作  
            await Task.Delay(100);
            return Result<ConcreteOutputData>.Success(new ConcreteOutputData { Data = "Concrete Node Data" });
        }
        catch (Exception ex)
        {
            return Result<ConcreteOutputData>.Failure(ex);
        }
    }

    // 显式实现非泛型接口的方法  
    async Task<Result<IOutputData>> INode.ExecuteAsync()
    {
        var result = await ExecuteAsync();
        return Result<IOutputData>.Success(result.Data);
    }
}

// 实现工作流节点类  
public class WorkflowNode : INode<ConcreteOutputData>
{
    private readonly List<INode> _nodes = new List<INode>();

    public void AddNode(INode node)
    {
        _nodes.Add(node);
    }

    public async Task<Result<ConcreteOutputData>> ExecuteAsync()
    {
        foreach (var node in _nodes)
        {
            var result = await node.ExecuteAsync();
            if (result.IsSuccess)
            {
                result.Data.Display();
            }
            else
            {
                Console.WriteLine($"Error: {result.Error.Message}");
                // 可以选择在这里中断执行或继续执行其他节点  
            }
        }

        return Result<ConcreteOutputData>.Success(new ConcreteOutputData { Data = "Workflow Node Data" });
    }

    // 显式实现非泛型接口的方法  
    async Task<Result<IOutputData>> INode.ExecuteAsync()
    {
        var result = await ExecuteAsync();
        return Result<IOutputData>.Success(result.Data);
    }
}

// 示例程序  
public class Run(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task Main()
    {
        // 创建具体节点  
        INode<ConcreteOutputData> node1 = new ConcreteNode();
        INode<ConcreteOutputData> node2 = new ConcreteNode();

        // 创建工作流节点  
        WorkflowNode workflow = new WorkflowNode();
        workflow.AddNode(node1);
        workflow.AddNode(node2);

        // 执行工作流  
        var workflowResult = await workflow.ExecuteAsync();
        if (workflowResult.IsSuccess)
        {
            workflowResult.Data.Display();
        }
        else
        {
            Console.WriteLine($"Workflow Error: {workflowResult.Error.Message}");
        }
    }
}