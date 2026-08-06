using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Application.Agents.Planning;
using EnterpriseKnowledgeAssistant.Application.Interfaces;

namespace EnterpriseKnowledgeAssistant.Application.Agents.Supervisor
{
    public sealed class SupervisorAgent : ISupervisorAgent
    {
        private readonly IPlannerService _plannerService;

        public SupervisorAgent(IPlannerService plannerService)
        {
            _plannerService = plannerService;
        }

        public Task<ExecutionPlan> CreatePlanAsync(AgentRequest request,CancellationToken cancellationToken = default)
        {
            return _plannerService.CreatePlanAsync(
                request, cancellationToken);
        }
    }
}
