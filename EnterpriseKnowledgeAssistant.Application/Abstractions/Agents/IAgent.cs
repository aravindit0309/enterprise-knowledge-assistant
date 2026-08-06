namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public interface IAgent
    {
        AgentType Type { get; }

        Task<AgentResult> ExecuteAsync(AgentRequest request, CancellationToken cancellationToken = default);
    }
}
