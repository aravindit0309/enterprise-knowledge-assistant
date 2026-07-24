using EnterpriseKnowledgeAssistant.Domain.Common;
using EnterpriseKnowledgeAssistant.Domain.Enums;

namespace EnterpriseKnowledgeAssistant.Domain.Entities
{
    public class Conversation : BaseEntity
    {
        public List<Message> Messages { get; private set; } = new ();

        public void AddUserMessage(string content)
        {
            AddMessage(MessageRole.User, content);
        }

        public void AddAssistantMessage(string content)
        {
            AddMessage(MessageRole.Assistant, content);
        }

        public void AddSystemMessage(string content)
        {
            AddMessage(MessageRole.System, content);
        }

        private void AddMessage(MessageRole role, string content)
        {
            var message = new Message(Guid.Empty, role, content);

            Messages.Add(message);

            MarkUpdated();
        }
    }
}
