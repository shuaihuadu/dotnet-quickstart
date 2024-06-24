namespace UnitTests.WorkflowDesign;
public class WorkflowV1
{
    public List<INode> Nodes { get; set; } = new List<INode>();
    public List<Connection> Connections { get; set; } = new List<Connection>();

    public async Task<FlowExecutionResult> ExecuteAsync(FlowExecutionContext context)
    {
        var executionResults = new List<NodeExecutionResult<object>>();
        var nodeDict = Nodes.ToDictionary(n => n.Id);
        var adjacencyList = new Dictionary<string, List<string>>();
        var inDegree = new Dictionary<string, int>();

        // 构建邻接表和入度表  
        foreach (var node in Nodes)
        {
            adjacencyList[node.Id] = new List<string>();
            inDegree[node.Id] = 0;
        }

        foreach (var connection in Connections)
        {
            adjacencyList[connection.FromNodeId].Add(connection.ToNodeId);
            inDegree[connection.ToNodeId]++;
        }

        var readyNodes = new Queue<string>();
        foreach (var node in Nodes)
        {
            if (inDegree[node.Id] == 0)
            {
                readyNodes.Enqueue(node.Id);
            }
        }

        var tasks = new List<Task<NodeExecutionResult<object>>>();

        while (readyNodes.Count > 0)
        {
            var currentNodeId = readyNodes.Dequeue();
            var currentNode = nodeDict[currentNodeId];

            tasks.Add(ExecuteNodeAsync(currentNode, context));

            foreach (var neighbor in adjacencyList[currentNodeId])
            {
                inDegree[neighbor]--;
                if (inDegree[neighbor] == 0)
                {
                    readyNodes.Enqueue(neighbor);
                }
            }

            if (readyNodes.Count == 0)
            {
                var results = await Task.WhenAll(tasks);
                executionResults.AddRange(results);
                tasks.Clear();
            }
        }

        return new FlowExecutionResult
        {
            Results = executionResults,
            State = NodeState.Successed,
            Errors = null
        };
    }

    private async Task<NodeExecutionResult<object>> ExecuteNodeAsync(INode node, FlowExecutionContext context)
    {
        // 根据节点类型执行相应的逻辑  
        // 这里简化为模拟异步操作  
        await Task.Delay(100);
        return new NodeExecutionResult<object>
        {
            Data = new { Message = $"{node.Name} executed" },
            State = NodeState.Successed,
            Message = null
        };
    }
}


public class Connection
{
    public string FromNodeId { get; set; }
    public string ToNodeId { get; set; }
}
public class FlowExecutionContext
{
    // 根据实际需求定义上下文内容  
}

public class FlowExecutionResult
{
    public List<NodeExecutionResult<object>> Results { get; set; }
    public NodeState State { get; set; }
    public List<string> Errors { get; set; }
}
