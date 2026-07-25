namespace EnterpriseKnowledgeAssistant.Domain.Documents
{
    public class DocumentChunk
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public int ChunkIndex { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Document Document { get; set; } = null!;
    }
}
