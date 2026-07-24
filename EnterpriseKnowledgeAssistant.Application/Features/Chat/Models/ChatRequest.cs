namespace EnterpriseKnowledgeAssistant.Application.Features.Chat.Models
{
    public class ChatRequest
    {
        public Guid? ConversationId { get; set; }
        public string? Message { get; set; }
    }
}
