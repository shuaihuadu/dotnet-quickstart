
namespace UnitTests.WorkFlowTest;

public class WorkflowTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {
        var node1 = new WorkflowNode("1", output);
        var node2 = new WorkflowNode("2", output);
        var node3 = new WorkflowNode("3", output);
        var node4 = new WorkflowNode("4", output);
        var node5 = new WorkflowNode("5", output);
        var node6 = new WorkflowNode("6", output);
        var node7 = new WorkflowNode("7", output);
        var node8 = new WorkflowNode("8", output);

        node2.Dependencies.Add(node1);
        node3.Dependencies.Add(node1);
        node4.Dependencies.Add(node2);
        node4.Dependencies.Add(node3);
        node5.Dependencies.Add(node4);
        node6.Dependencies.Add(node4);
        node7.Dependencies.Add(node4);
        node8.Dependencies.Add(node5);
        node8.Dependencies.Add(node6);
        node8.Dependencies.Add(node7);

        var workflow = new Workflow();
        workflow.AddNode(node1);
        workflow.AddNode(node2);
        workflow.AddNode(node3);
        workflow.AddNode(node4);
        workflow.AddNode(node5);
        workflow.AddNode(node6);
        workflow.AddNode(node7);
        workflow.AddNode(node8);

        workflow.Execute(output);
    }


}

public enum NodeStatus
{
    NotStarted,
    InProgress,
    Completed,
    //Faild
}

public class WorkflowNode
{
    private readonly ITestOutputHelper _output;

    public string Id { get; set; }
    public NodeStatus Status { get; set; }
    public List<WorkflowNode> Dependencies { get; set; }

    public WorkflowNode(string id, ITestOutputHelper output)
    {
        Id = id;
        Status = NodeStatus.NotStarted;
        Dependencies = new List<WorkflowNode>();

        this._output = output;
    }

    public void Execute()
    {
        Console.WriteLine($"Executing Node {Id}");
        Status = NodeStatus.InProgress;
        System.Threading.Thread.Sleep(300); // Simulate work  
        Status = NodeStatus.Completed;
        this._output.WriteLine($"Node {Id} Completed");
    }
}

public class Workflow
{
    public List<WorkflowNode> Nodes { get; set; }

    public Workflow()
    {
        Nodes = new List<WorkflowNode>();
    }

    public void AddNode(WorkflowNode node)
    {
        Nodes.Add(node);
    }

    public void Execute(ITestOutputHelper output)
    {
        var sortedNodes = TopologicalSort();

        foreach (var node in sortedNodes)
        {
            ExecuteNode(node, output);
        }
    }

    private void ExecuteNode(WorkflowNode node, ITestOutputHelper output)
    {
        foreach (var dependency in node.Dependencies)
        {
            if (dependency.Status != NodeStatus.Completed)
            {
                ExecuteNode(dependency, output);
            }
        }

        if (node.Status == NodeStatus.NotStarted)
        {
            output.WriteLine(node.Id + " = " + node.Status);

            node.Execute();

            output.WriteLine("After execute:" + node.Id + " = " + node.Status);
        }
    }

    private List<WorkflowNode> TopologicalSort()
    {
        var inDegree = new Dictionary<WorkflowNode, int>();
        foreach (var node in Nodes)
        {
            inDegree[node] = 0;
        }

        foreach (var node in Nodes)
        {
            foreach (var dependency in node.Dependencies)
            {
                inDegree[dependency]++;
            }
        }

        var queue = new Queue<WorkflowNode>();
        foreach (var node in Nodes)
        {
            if (inDegree[node] == 0)
            {
                queue.Enqueue(node);
            }
        }

        var sortedList = new List<WorkflowNode>();
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            sortedList.Add(node);
            foreach (var dependency in node.Dependencies)
            {
                inDegree[dependency]--;
                if (inDegree[dependency] == 0)
                {
                    queue.Enqueue(dependency);
                }
            }
        }

        if (sortedList.Count != Nodes.Count)
        {
            throw new InvalidOperationException("The graph has at least one cycle.");
        }

        return sortedList;
    }
}