using EnterpriseKnowledgeAssistant.Application.Agents.Planning;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public interface IAgentOrchestrator
    {
        Task<AgentResult> ExecuteAsync(ExecutionPlan executionPlan, AgentRequest request, CancellationToken cancellationToken = default);
    }
}
