using EnterpriseKnowledgeAssistant.Domain.Entities;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public sealed record AgentRequest( Guid ConversationId, string UserMessage, IReadOnlyCollection<Message> Messages);
}
