
namespace UnitTests.WorkflowDesign;

public class Runner(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task RunAsync()
    {
        var startNode = new StartNode { Id = "start", Name = "Start Node" };
        var endNode = new EndNode { Id = "end", Name = "End Node" };
        var businessNode1 = new BusinessNode1 { Id = "business1", Name = "Business Node 1" };
        var businessNode2 = new BusinessNode2 { Id = "business2", Name = "Business Node 2" };
        var businessNode3 = new BusinessNode3 { Id = "business3", Name = "Business Node 3" };

        var workflow = new Workflow
        {
            //Nodes = new List<INode> { startNode, businessNode1, businessNode2, businessNode3, endNode }, WorkflowV1

            Nodes = new List<INode<IOutputData>> { startNode, businessNode1, businessNode2, businessNode3, endNode },
            Connections = new List<Connection>
            {
                new Connection { FromNodeId = "start", ToNodeId = "business1" },
                new Connection { FromNodeId = "business1", ToNodeId = "business2" },
                new Connection { FromNodeId = "business1", ToNodeId = "business3" },
                new Connection { FromNodeId = "business2", ToNodeId = "end" },
                new Connection { FromNodeId = "business3", ToNodeId = "end" }
            }
        };

        var context = new FlowExecutionContext();
        var result = await workflow.ExecuteAsync(context);

        // 输出执行结果  
        foreach (var executionResult in result.Results)
        {
            output.WriteLine($"Node executed with status:{executionResult.Data}, {executionResult.State}");
        }
    }
}
