using UnitTests.Topological;

namespace UnitTests;

public class Topological_DFS_Test(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void RunTopologicalTest()
    {
        var workflow = new WorkflowDFS<int>();
        var context = new NodeExecutionContext();

        var intNode = new IntNode { Name = "IntNode", Description = "This is an integer node" };
        var stringNode = new StringNode { Name = "StringNode", Description = "This is a string node" };
        var anotherIntNode = new IntNode { Name = "AnotherIntNode", Description = "This is another integer node" };

        // 定义节点间的依赖关系  
        stringNode.Dependencies.Add(intNode);
        anotherIntNode.Dependencies.Add(stringNode);


        workflow.AddNode(intNode);
        workflow.AddNode(stringNode);
        workflow.AddNode(anotherIntNode);

        workflow.Execute(context);

        // 访问上下文中的数据  
        Console.WriteLine($"IntNodeResult: {context.Data["IntNodeResult"]}");
        Console.WriteLine($"StringNodeResult: {context.Data["StringNodeResult"]}");
    }
}
