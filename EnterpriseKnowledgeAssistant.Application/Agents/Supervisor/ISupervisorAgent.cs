using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Application.Agents.Planning;

namespace EnterpriseKnowledgeAssistant.Application.Agents.Supervisor
{
    public interface ISupervisorAgent
    {
        Task<ExecutionPlan> CreatePlanAsync(
    AgentRequest request, CancellationToken cancellationToken = default);
    }
}
