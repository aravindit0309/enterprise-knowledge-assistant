namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public sealed record AgentToolResult( bool Success, string Content, IReadOnlyCollection<AgentSource> Sources);
}
