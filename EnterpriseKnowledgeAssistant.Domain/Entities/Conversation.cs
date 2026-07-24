using EnterpriseKnowledgeAssistant.Domain.Common;
using EnterpriseKnowledgeAssistant.Domain.Enums;

namespace EnterpriseKnowledgeAssistant.Domain.Entities
{
    public class Conversation : BaseEntity
    {
        private readonly List<Message> _messages = new();

        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

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
            _messages.Add(new Message(Id, role, content));

            MarkUpdated();
        }
    }
}
