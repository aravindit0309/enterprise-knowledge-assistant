using EnterpriseKnowledgeAssistant.Domain.Documents;

namespace EnterpriseKnowledgeAssistant.Application.Common.Interfaces
{
    public interface IDocumentChunkRepository
    {
        Task AddRangeAsync(IEnumerable<DocumentChunk> chunks, CancellationToken cancellationToken);
        Task DeleteByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken);
    }
}
