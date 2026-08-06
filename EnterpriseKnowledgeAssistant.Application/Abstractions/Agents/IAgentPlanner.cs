using EnterpriseKnowledgeAssistant.Application.Agents.Planning;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public interface IAgentPlanner
    {
        Task<ExecutionPlan> PlanAsync(
           AgentRequest agentRequest,
            IReadOnlyCollection<AgentToolDefinition> tools,
            CancellationToken cancellationToken = default);
    }
}
