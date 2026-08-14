namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.WebResearch
{
    public sealed record WebResearchResult(IReadOnlyCollection<WebResearchItem> Results);
}
