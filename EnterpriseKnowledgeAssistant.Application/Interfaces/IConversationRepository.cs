using EnterpriseKnowledgeAssistant.Domain.Entities;

namespace EnterpriseKnowledgeAssistant.Application.Interfaces
{
    public interface IConversationRepository
    {
        Task<Conversation?> GetByIdAsync( Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Conversation conversation, CancellationToken cancellationToken = default);
        Task SaveChangesAsync( CancellationToken cancellationToken = default);
    }
}
