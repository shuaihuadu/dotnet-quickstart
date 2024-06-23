namespace MCS.Library.Common.Tests;

public class Workflow_Test2
{
    [Fact]
    public void Run()
    {
        var node1 = new WorkflowNode("1");
        var node2 = new WorkflowNode("2");
        var node3 = new WorkflowNode("3");
        var node4 = new WorkflowNode("4");
        var node5 = new WorkflowNode("5");
        var node6 = new WorkflowNode("6");
        var node7 = new WorkflowNode("7");
        var node8 = new WorkflowNode("8");

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

        workflow.Execute();
    }
}


public enum NodeStatus
{
    NotStarted,
    InProgress,
    Completed,
    Failed
}

public class WorkflowNode
{
    public string Id { get; set; }
    public NodeStatus Status { get; set; }
    public List<WorkflowNode> Dependencies { get; set; }

    public WorkflowNode(string id)
    {
        Id = id;
        Status = NodeStatus.NotStarted;
        Dependencies = [];
    }

    public void Execute()
    {
        try
        {
            Console.WriteLine($"Executing Node {Id}");
            Status = NodeStatus.InProgress;
            // Simulate a failure for demonstration purposes  
            if (Id == "0")
            {
                throw new Exception("Simulated failure");
            }
            Status = NodeStatus.Completed;
            Console.WriteLine($"Node {Id} Completed");
        }
        catch (Exception ex)
        {
            Status = NodeStatus.Failed;
            Console.WriteLine($"Node {Id} Failed: {ex.Message}");
        }
    }
}

public class Workflow
{
    public List<WorkflowNode> Nodes { get; set; }

    public Workflow()
    {
        Nodes = [];
    }

    public void AddNode(WorkflowNode node)
    {
        Nodes.Add(node);
    }

    public void Execute()
    {
        var sortedNodes = TopologicalSort();
        foreach (var node in sortedNodes)
        {
            if (Nodes.Exists(n => n.Status == NodeStatus.Failed))
            {
                Console.WriteLine("Execution halted due to a failed node.");
                break;
            }
            ExecuteNode(node);
        }
    }

    private bool ExecuteNode(WorkflowNode node)
    {
        // If any node has failed, stop execution  
        if (Nodes.Exists(n => n.Status == NodeStatus.Failed))
        {
            return false;
        }

        // Check dependencies  
        foreach (var dependency in node.Dependencies)
        {
            if (dependency.Status != NodeStatus.Completed && !ExecuteNode(dependency))
            {
                return false;
            }
        }

        // Execute the current node if it has not started yet  
        if (node.Status == NodeStatus.NotStarted)
        {
            node.Execute();
        }

        return node.Status != NodeStatus.Failed;
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
