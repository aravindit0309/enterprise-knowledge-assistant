using EnterpriseKnowledgeAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Persistence.Configuration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Role).HasConversion<string>();

            builder.Property(x => x.Content).HasMaxLength(4000).IsRequired();

            builder.Property(x => x.CreatedAtUtc).IsRequired();
        }
    }
}
