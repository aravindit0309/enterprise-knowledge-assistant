namespace EnterpriseKnowledgeAssistant.Infrastructure.WebResearch
{
    public sealed class TavilySearchResponse
    {
        public List<TavilySearchResult> Results { get; set; } = [];
    }

    public sealed class TavilySearchResult
    {
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public double Score { get; set; }
    }
}
