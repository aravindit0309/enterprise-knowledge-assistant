using EnterpriseKnowledgeAssistant.Infrastructure.AI.Models;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock.Models
{
    public class NovaRequest
    {
        public string SchemaVersion { get; set; } = "messages-v1";

        public List<NovaMessage> Messages { get; set; } = new();

        public InferenceConfig InferenceConfig { get; set; } = new();
    }
}
