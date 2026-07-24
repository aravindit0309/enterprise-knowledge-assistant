using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;
using EnterpriseKnowledgeAssistant.Domain.Entities;

namespace EnterpriseKnowledgeAssistant.Application.Features.Chat
{
    public interface IChatService
    {
        Task<ChatResponse> SendAsync(IReadOnlyCollection<Message> messages, CancellationToken cancellationToken = default);        
    }
}
