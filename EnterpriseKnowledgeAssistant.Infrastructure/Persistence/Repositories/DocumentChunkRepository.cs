using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using EnterpriseKnowledgeAssistant.Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Pgvector;

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

        public async Task<IReadOnlyList<DocumentChunk>> SearchSimilarAsync(float[] queryEmbedding, int limit, CancellationToken cancellationToken = default)
        {
            var queryVector = new Vector(queryEmbedding);

            return await _dbContext.DocumentChunks.FromSqlInterpolated($"""
            SELECT *
            FROM "DocumentChunks"
            WHERE "Embedding" IS NOT NULL
            ORDER BY "Embedding" <=> {queryVector}
            LIMIT {limit}
            """).AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
