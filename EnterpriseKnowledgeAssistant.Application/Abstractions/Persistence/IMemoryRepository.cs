using EnterpriseKnowledgeAssistant.Domain.Memory;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Persistence
{
    public interface IMemoryRepository
    {
        Task AddAsync(
            MemoryRecord memory,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<MemoryRecord>> SearchSimilarAsync(
            float[] queryEmbedding,
            int limit,
            CancellationToken cancellationToken = default);
    }
}
