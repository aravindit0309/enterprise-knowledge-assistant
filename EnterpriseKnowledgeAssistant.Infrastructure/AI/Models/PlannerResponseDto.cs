namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Models
{
    public sealed class PlannerResponseDto
    {
        public List<PlannerStepDto> Steps { get; init; } = [];
    }
}
