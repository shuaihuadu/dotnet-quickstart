namespace UnitTests.WorkflowDesign;

public class StartNode : NodeBase<StartOutputData>
{
    protected override async Task<NodeExecutionResult<StartOutputData>> ExecuteNodeAsync(NodeExecutionContext context)
    {
        await Task.Delay(100); // 模拟异步操作  
        var outputData = new StartOutputData { Message = "Start node executed" };

        return new NodeExecutionResult<StartOutputData>
        {
            Data = outputData,
            State = NodeState.Successed,
            Message = string.Empty
        };
    }
}

public class EndNode : NodeBase<EndOutputData>
{
    protected override async Task<NodeExecutionResult<EndOutputData>> ExecuteNodeAsync(NodeExecutionContext context)
    {
        await Task.Delay(100); // 模拟异步操作  
        var outputData = new EndOutputData { Message = "End node executed" };

        return new NodeExecutionResult<EndOutputData>
        {
            Data = outputData,
            State = NodeState.Successed,
            Message = string.Empty
        };
    }
}

public class BusinessNode1 : NodeBase<BusinessOutputData1>
{
    protected override async Task<NodeExecutionResult<BusinessOutputData1>> ExecuteNodeAsync(NodeExecutionContext context)
    {
        await Task.Delay(100); // 模拟异步操作  
        var outputData = new BusinessOutputData1 { Result = "Business node 1 executed" };

        return new NodeExecutionResult<BusinessOutputData1>
        {
            Data = outputData,
            State = NodeState.Successed,
            Message = string.Empty
        };
    }
}

public class BusinessNode2 : NodeBase<BusinessOutputData2>
{
    protected override async Task<NodeExecutionResult<BusinessOutputData2>> ExecuteNodeAsync(NodeExecutionContext context)
    {
        await Task.Delay(100); // 模拟异步操作  
        var outputData = new BusinessOutputData2 { Result = "Business node 2 executed" };

        return new NodeExecutionResult<BusinessOutputData2>
        {
            Data = outputData,
            State = NodeState.Successed,
            Message = string.Empty
        };
    }
}

public class BusinessNode3 : NodeBase<BusinessOutputData3>
{
    protected override async Task<NodeExecutionResult<BusinessOutputData3>> ExecuteNodeAsync(NodeExecutionContext context)
    {
        await Task.Delay(100); // 模拟异步操作  
        var outputData = new BusinessOutputData3 { Result = "Business node 3 executed" };

        return new NodeExecutionResult<BusinessOutputData3>
        {
            Data = outputData,
            State = NodeState.Successed,
            Message = string.Empty
        };
    }
}

public class StartOutputData : IOutputData
{
    public string Message { get; set; }
}

public class EndOutputData : IOutputData
{
    public string Message { get; set; }
}

public class BusinessOutputData1 : IOutputData
{
    public string Result { get; set; }
}

public class BusinessOutputData2 : IOutputData
{
    public string Result { get; set; }
}

public class BusinessOutputData3 : IOutputData
{
    public string Result { get; set; }
}
