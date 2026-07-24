namespace EnterpriseKnowledgeAssistant.Application.Features.Chat.Models
{
    public class ChatResponse
    {
        public Guid ConversationId { get; set; }
        public string? Response { get; set; }
        public string? ModelUsed { get; set; }
    }
}
