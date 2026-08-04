namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public sealed record AgentResult(string Response, string ModelUsed, IReadOnlyCollection<AgentSource> Sources);
}
