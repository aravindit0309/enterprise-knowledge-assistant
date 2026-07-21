using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;

namespace EnterpriseKnowledgeAssistant.Application.Features.Chat
{
    public interface IChatService
    {
        Task<ChatResponse> GetChatResponseAsync(ChatRequest request, CancellationToken cancellationToken = default);
    }
}
