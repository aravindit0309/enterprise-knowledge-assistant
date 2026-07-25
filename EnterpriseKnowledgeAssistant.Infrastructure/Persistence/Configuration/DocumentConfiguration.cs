using EnterpriseKnowledgeAssistant.Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Persistence.Configuration
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();

            builder.Property(x => x.StoredFileName).HasMaxLength(255).IsRequired();

            builder.Property(x => x.ContentType).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Status).HasConversion<int>().IsRequired();

            builder.Property(x => x.FileSize).IsRequired();

            builder.Property(x => x.UploadedAt).IsRequired();
        }
    }
}
