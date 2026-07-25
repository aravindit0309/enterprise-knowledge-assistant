using EnterpriseKnowledgeAssistant.Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Persistence.Configuration
{
    public sealed class DocumentChunkConfiguration : IEntityTypeConfiguration<DocumentChunk>
    {
        public void Configure(EntityTypeBuilder<DocumentChunk> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Content).IsRequired();

            builder.Property(x => x.ChunkIndex).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasOne(x => x.Document).WithMany(x => x.Chunks).HasForeignKey(x => x.DocumentId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.DocumentId,
                x.ChunkIndex
            }).IsUnique();
        }
    }
}
