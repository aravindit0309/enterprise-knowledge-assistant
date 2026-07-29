namespace EnterpriseKnowledgeAssistant.Domain.Documents
{
    public interface IDocumentRepository
    {
        Task AddAsync(Document document, CancellationToken cancellationToken);
        Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
