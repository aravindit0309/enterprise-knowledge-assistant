using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Application.Agents.Planning;
using EnterpriseKnowledgeAssistant.Application.Interfaces;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI
{
    public class AmazonNovaPlannerService : IPlannerService
    {
        public Task<ExecutionPlan> CreatePlanAsync(
        AgentRequest request,
        CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException(
            "LLM planner will be implemented in Sprint 6 Phase 5.");
        }
    

        //public Task<ExecutionPlan> CreatePlanAsync(AgentRequest request, IReadOnlyCollection<AgentToolDefinition> tools, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
