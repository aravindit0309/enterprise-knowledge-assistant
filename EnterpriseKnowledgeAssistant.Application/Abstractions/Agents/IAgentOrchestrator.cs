namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public interface IAgentOrchestrator
    {
        Task<AgentResult> ExecuteAsync(  AgentRequest request, CancellationToken cancellationToken = default);
    }
}
