using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;

namespace EnterpriseKnowledgeAssistant.Application.Features.Chat.Commands.SendMessage
{
    public sealed record SendMessageCommand(ChatRequest Request);
}
