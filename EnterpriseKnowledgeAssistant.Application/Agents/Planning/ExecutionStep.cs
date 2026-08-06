namespace EnterpriseKnowledgeAssistant.Application.Agents.Planning
{
    public sealed record ExecutionStep
    {
        public required int Order { get; init; }
        public required ExecutionStepType Type { get; init; }
        public string? ToolName { get; init; }
        public string? Input { get; init; }
        public string? Description { get; init; }
    }
}
