namespace UnitTests.Topological;

public class Workflow<T>
{
    protected ITestOutputHelper Output { get; }

    public Workflow(ITestOutputHelper output)
    {
        this.Output = output;
    }

    private readonly List<INode<T>> _nodes = [];

    public void AddNode(INode<T> node)
    {
        _nodes.Add(node);
    }

    public void Execute(NodeExecutionContext context)
    {
        var sortedNodes = TopologicalSort(_nodes);

        foreach (var node in sortedNodes)
        {
            this.Output.WriteLine(node.Name + " = " + node.State.ToString());

            if (CanExecuteNode(node))
            {
                var result = node.Execute(context, () => Console.WriteLine($"Executing {node.Name}"));
                this.Output.WriteLine($"Node {node.Name} executed with result: {result.Result}, state: {result.State}, message: {result.Message}");
            }
        }
    }

    private List<INode<T>> TopologicalSort(List<INode<T>> nodes)
    {
        var sortedList = new List<INode<T>>();
        var inDegree = new Dictionary<INode<T>, int>();

        foreach (var node in nodes)
        {
            inDegree[node] = 0;
        }

        foreach (var node in nodes)
        {
            foreach (var dependency in node.Dependencies)
            {
                inDegree[dependency]++;
            }
        }

        var queue = new Queue<INode<T>>();
        foreach (var node in nodes)
        {
            if (inDegree[node] == 0)
            {
                queue.Enqueue(node);
            }
        }

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            sortedList.Add(node);

            foreach (var dependent in node.Dependencies)
            {
                inDegree[dependent]--;
                if (inDegree[dependent] == 0)
                {
                    queue.Enqueue(dependent);
                }
            }
        }

        if (sortedList.Count != nodes.Count)
        {
            throw new InvalidOperationException("Graph has at least one cycle.");
        }

        return sortedList;
    }

    private bool CanExecuteNode(INode<T> node)
    {
        return node.Dependencies.All(dependency => dependency.State == NodeState.Completed);
    }

}