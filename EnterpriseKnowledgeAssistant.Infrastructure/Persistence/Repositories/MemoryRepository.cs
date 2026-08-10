using EnterpriseKnowledgeAssistant.Application.Abstractions.Persistence;
using EnterpriseKnowledgeAssistant.Domain.Memory;
using Microsoft.EntityFrameworkCore;
using Pgvector;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Persistence.Repositories
{
    public class MemoryRepository : IMemoryRepository
    {
        private readonly AppDbContext _dbContext;

        public MemoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    
        public async Task AddAsync(MemoryRecord memory, CancellationToken cancellationToken)
        {
            await _dbContext.MemoryRecords.AddAsync(memory, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<MemoryRecord>> SearchSimilarAsync(float[] queryEmbedding, int limit, CancellationToken cancellationToken = default)
        {
            var queryVector = new Vector(queryEmbedding);

            return await _dbContext.MemoryRecords
                .FromSqlInterpolated($"""
                    SELECT *
                    FROM "MemoryRecords"
                    WHERE "Embedding" IS NOT NULL
                    ORDER BY "Embedding" <=> {queryVector}
                    LIMIT {limit}
                    """)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
