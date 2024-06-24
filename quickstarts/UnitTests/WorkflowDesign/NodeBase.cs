namespace UnitTests.WorkflowDesign;
public abstract class NodeBase : INode
{
    public string Id { get; set; }
    public string Name { get; set; }
    public NodeState State { get; set; }
    public double Version { get; set; }

    public abstract Task<NodeExecutionResult<object>> ExecuteAsync(NodeExecutionContext context);
}
public abstract class NodeBase<T> : NodeBase, INode<T> where T : IOutputData
{
    async Task<NodeExecutionResult<T>> INode<T>.ExecuteAsync(NodeExecutionContext context)
    {
        return await ExecuteNodeAsync(context);
    }

    // 由于 ExecuteAsync 返回类型不同，NodeBase 中的 ExecuteAsync 需要实现  
    public override async Task<NodeExecutionResult<object>> ExecuteAsync(NodeExecutionContext context)
    {
        NodeExecutionResult<T> result = await this.ExecuteNodeAsync(context);

        return new NodeExecutionResult<object>();
    }

    protected abstract Task<NodeExecutionResult<T>> ExecuteNodeAsync(NodeExecutionContext context);


}
