namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public sealed record AgentSource(Guid DocumentId, string DocumentName, int ChunkIndex);
}
