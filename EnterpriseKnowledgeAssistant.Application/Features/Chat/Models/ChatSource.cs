namespace EnterpriseKnowledgeAssistant.Application.Features.Chat.Models
{
    public sealed class ChatSource
    {
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public int ChunkIndex { get; set; }
    }
}
