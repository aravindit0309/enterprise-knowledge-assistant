using EnterpriseKnowledgeAssistant.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Domain.Entities
{
    public class Conversation : BaseEntity
    {
        private readonly List<Message> _messages = new();

        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

        public void AddMessage(Message message)
        {
            _messages.Add(message);
            MarkUpdated();
        }
    }
}
