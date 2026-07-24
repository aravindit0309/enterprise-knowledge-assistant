using EnterpriseKnowledgeAssistant.Domain.Common;
using EnterpriseKnowledgeAssistant.Domain.Enums;

namespace EnterpriseKnowledgeAssistant.Domain.Entities
{
    public class Message : BaseEntity
    {
        public Guid ConversationId { get; private set; }

        public MessageRole Role { get; private set; }

        public string Content { get; private set; }

        private Message()
        {
            Content = string.Empty;
        }

        public Message( Guid conversationId, MessageRole role, string content)
        {
            ConversationId = conversationId;
            Role = role;
            Content = content;
        }
    }
}
