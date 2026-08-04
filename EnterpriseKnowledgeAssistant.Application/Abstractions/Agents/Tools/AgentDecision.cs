namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.Tools
{
    public sealed record AgentDecision(bool RequiresTool, string? ToolName, string? ToolInput);
}
