using EnterpriseKnowledgeAssistant.Application.Interfaces;
using EnterpriseKnowledgeAssistant.Domain.Entities;
using EnterpriseKnowledgeAssistant.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Repositories
{
    public class ConversationRepository  : IConversationRepository
    {
        private readonly AppDbContext _dbContext;

        public ConversationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Conversation conversation, CancellationToken cancellationToken = default)
        {
            await _dbContext.Conversations.AddAsync(conversation, cancellationToken);
        }

        public async Task<Conversation?> GetByIdAsync( Guid id,CancellationToken cancellationToken = default)
        {
            return await _dbContext.Conversations.Include(x => x.Messages)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // TODO:
                // EF Core is tracking newly added Message entities as Modified instead of Added
                // when appended to an existing Conversation aggregate.
                // Workaround: explicitly mark Message entities as Added.
                // Revisit after Sprint 5 or when upgrading EF Core/Npgsql.

                foreach (var conversationEntry in _dbContext.ChangeTracker.Entries<Conversation>())
                {
                    foreach (var message in conversationEntry.Entity.Messages)
                    {
                        var entry = _dbContext.Entry(message);

                        if (entry.State == EntityState.Modified)
                        {
                            entry.State = EntityState.Added;
                        }
                    }
                }


                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
        }
    }
}
