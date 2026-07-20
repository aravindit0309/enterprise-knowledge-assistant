namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock.Models
{
    public class NovaMessageResponse
    {
        public string Role { get; set; } = string.Empty;

        public List<NovaContentResponse> Content { get; set; } = new();
    }
}
