using UnitTests.Topological;

namespace UnitTests;

public class Topological_Kahn_Test(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void RunTopologicalTest()
    {
        var workflow = new Workflow<int>(this.Output);
        var context = new NodeExecutionContext();


        var nodeA = new IntNode { Name = "NodeA", Description = "This is node A" };
        var nodeB = new StringNode { Name = "NodeB", Description = "This is node B" };
        var nodeC = new IntNode { Name = "NodeC", Description = "This is node C" };
        var nodeD = new StringNode { Name = "NodeD", Description = "This is node D" };
        var nodeE = new IntNode { Name = "NodeE", Description = "This is node E" };
        var nodeF = new StringNode { Name = "NodeF", Description = "This is node F" };
        var nodeG = new IntNode { Name = "NodeG", Description = "This is node G" };
        var nodeH = new StringNode { Name = "NodeH", Description = "This is node H" };

        // 定义节点间的依赖关系  
        nodeB.Dependencies.Add(nodeA);
        nodeC.Dependencies.Add(nodeA);
        nodeD.Dependencies.Add(nodeB);
        nodeE.Dependencies.Add(nodeB);
        nodeF.Dependencies.Add(nodeC);
        nodeG.Dependencies.Add(nodeD);
        nodeH.Dependencies.Add(nodeE);
        nodeH.Dependencies.Add(nodeF);


        workflow.AddNode(nodeA);
        workflow.AddNode(nodeB);
        workflow.AddNode(nodeC);
        workflow.AddNode(nodeD);
        workflow.AddNode(nodeE);
        workflow.AddNode(nodeF);
        workflow.AddNode(nodeG);
        workflow.AddNode(nodeH);

        workflow.Execute(context);
    }
}
