namespace UnitTests.Topological;

public class Workflow<T>
{
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
            if (CanExecuteNode(node))
            {
                var result = node.Execute(context, () => Console.WriteLine($"Executing {node.Name}"));
                Console.WriteLine($"Node {node.Name} executed with result: {result.Result}, state: {result.State}, message: {result.Message}");
            }
        }
    }

    private List<INode<T>> TopologicalSort(List<INode<T>> nodes)
    {
        var sortedList = new List<INode<T>>();
        var visited = new HashSet<INode<T>>();
        var visiting = new HashSet<INode<T>>();

        foreach (var node in nodes)
        {
            if (!visited.Contains(node))
            {
                TopologicalSortVisit(node, visited, visiting, sortedList);
            }
        }

        return sortedList;
    }

    private void TopologicalSortVisit(INode<T> node, HashSet<INode<T>> visited, HashSet<INode<T>> visiting, List<INode<T>> sortedList)
    {
        if (visiting.Contains(node))
        {
            throw new InvalidOperationException("Graph has at least one cycle.");
        }

        if (!visited.Contains(node))
        {
            visiting.Add(node);
            foreach (var dependency in node.Dependencies)
            {
                TopologicalSortVisit(dependency, visited, visiting, sortedList);
            }
            visiting.Remove(node);
            visited.Add(node);
            sortedList.Add(node);
        }
    }

    private bool CanExecuteNode(INode<T> node)
    {
        return node.Dependencies.All(dependency => dependency.State == NodeState.Completed);
    }
}