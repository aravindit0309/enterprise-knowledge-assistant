using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.WebResearch;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public interface IWebResearchService
    {
        Task<WebResearchResult> SearchAsync(string query, CancellationToken cancellationToken = default);
    }
}
