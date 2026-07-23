using EnterpriseKnowledgeAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Persistence.Configuration
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("Conversations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAtUtc).IsRequired();

            builder.Property(x => x.UpdatedAtUtc);

            builder.HasMany(x => x.Messages).WithOne().HasForeignKey(x => x.ConversationId).IsRequired();
        }
    }
}
