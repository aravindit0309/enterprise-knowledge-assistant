using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using EnterpriseKnowledgeAssistant.Domain.Documents;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Persistence.Repositories
{
    public sealed class DocumentChunkRepository : IDocumentChunkRepository
    {
        private readonly AppDbContext _dbContext;

        public DocumentChunkRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(IEnumerable<DocumentChunk> chunks, CancellationToken cancellationToken)
        {
            await _dbContext.DocumentChunks.AddRangeAsync(chunks,cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken)
        {
            var chunks = await _dbContext.DocumentChunks.Where(x => x.DocumentId == documentId).ToListAsync(cancellationToken);

            if (chunks.Count == 0)          {
                return;
            }

            _dbContext.DocumentChunks.RemoveRange(chunks);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
