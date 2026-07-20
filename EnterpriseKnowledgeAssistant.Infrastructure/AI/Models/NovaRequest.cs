namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Models
{
    public class NovaRequest
    {
        public string SchemaVersion { get; set; } = "messages-v1";

        public List<NovaMessage> Messages { get; set; } = new();

        public InferenceConfig InferenceConfig { get; set; } = new();
    }
}
