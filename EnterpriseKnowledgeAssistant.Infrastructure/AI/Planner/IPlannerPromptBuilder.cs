using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Planner
{
    public interface IPlannerPromptBuilder
    {
        string Build(
            AgentRequest request,
            IReadOnlyCollection<AgentToolDefinition> tools);
    }
}
