using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Application.Agents.Planning;

namespace EnterpriseKnowledgeAssistant.Application.Interfaces
{
    public interface IPlannerService
    {
        Task<ExecutionPlan> CreatePlanAsync(
            AgentRequest request,
            CancellationToken cancellationToken = default);
    }
}
