using EnterpriseKnowledgeAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Conversation> Conversations => Set<Conversation>();

        public DbSet<EnterpriseKnowledgeAssistant.Domain.Entities.Message> Messages => Set<EnterpriseKnowledgeAssistant.Domain.Entities.Message>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
