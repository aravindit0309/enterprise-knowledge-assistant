namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public interface IAgentTool
    {
        string Name { get; }

        string Description { get; }

        Task<AgentToolResult> ExecuteAsync( string input, CancellationToken cancellationToken = default);
    }
}
