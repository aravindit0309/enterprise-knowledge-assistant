using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.Tools;
using EnterpriseKnowledgeAssistant.Domain.Entities;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public interface IAgentDecisionService
    {
        Task<AgentDecision> DecideAsync(
            string userMessage,
            IReadOnlyCollection<Message> conversationMessages,
            IReadOnlyCollection<AgentToolDefinition> tools,
            CancellationToken cancellationToken = default);
    }
}
