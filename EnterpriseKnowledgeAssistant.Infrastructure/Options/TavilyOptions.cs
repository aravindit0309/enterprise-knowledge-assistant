namespace EnterpriseKnowledgeAssistant.Infrastructure.Options
{
    public sealed class TavilyOptions
    {
        public string BaseUrl { get; set; } = "https://api.tavily.com";
        public string ApiKey { get; set; } = string.Empty;
        public int MaxResults { get; set; } = 3;
    }
}
