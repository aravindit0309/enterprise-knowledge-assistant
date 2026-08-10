using EnterpriseKnowledgeAssistant.Domain.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pgvector;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Persistence.Configuration
{
    public sealed class MemoryRecordConfiguration
    : IEntityTypeConfiguration<MemoryRecord>
    {
        public void Configure(EntityTypeBuilder<MemoryRecord> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Content).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.Property(x => x.Embedding).HasConversion(
                    embedding => embedding == null ? null : new Vector(embedding),
                    vector => vector == null ? null : vector.ToArray())
                .HasColumnType("vector(256)");
        }
    }
}
