namespace EnterpriseKnowledgeAssistant.Domain.Memory
{
    public sealed class MemoryRecord
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public float[]? Embedding { get; set; }
    }
}
