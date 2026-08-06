using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Application.Agents.Planning;

namespace EnterpriseKnowledgeAssistant.Application.Agents.Supervisor
{
    public sealed class SupervisorAgent : ISupervisorAgent
    {
        private readonly IAgentPlanner _agentPlanner;
        private readonly IEnumerable<IAgentTool> _tools;


        public SupervisorAgent(IAgentPlanner agentPlanner, IEnumerable<IAgentTool> tools)
        {
            _agentPlanner = agentPlanner;
            _tools = tools;
        }

        public Task<ExecutionPlan> CreatePlanAsync(AgentRequest request, CancellationToken cancellationToken = default)
        {
            var toolDefinitions = _tools
             .Select(t => new AgentToolDefinition(
                 t.Name,
                 t.Description))
             .ToList();

            return _agentPlanner.PlanAsync(
                request,
                toolDefinitions,
                cancellationToken);
        }
    }
}
