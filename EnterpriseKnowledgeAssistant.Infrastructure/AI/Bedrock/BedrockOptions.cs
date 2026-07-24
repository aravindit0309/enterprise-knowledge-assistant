using System.ComponentModel.DataAnnotations;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock
{
    public class BedrockOptions
    {
        public const string SectionName = "Bedrock";

        public string Region { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string ModelId { get; init; } = string.Empty;
        public float Temperature { get; set; } = 0.7f;
        public int MaxTokens { get; set; } = 1024;
        public bool EnableStreaming { get; set; } = false;
        public int RequestTimeoutSeconds { get; set; } = 60;
    }
}
