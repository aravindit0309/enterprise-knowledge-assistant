using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Models
{
    public class NovaMessage
    {
        public string Role { get; set; } = "user";

        public List<TextContent> Content { get; set; } = new();
    }
}
