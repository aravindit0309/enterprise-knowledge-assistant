namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Models
{
    public sealed class PlannerStepDto
    {
        public int Order { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? Tool { get; set; }
        public string? Input { get; set; }
        public string? Description { get; set; }
    }
}
