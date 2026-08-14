using System.Text.Json.Serialization;

namespace EnterpriseKnowledgeAssistant.Infrastructure.WebResearch
{
    public sealed class TavilySearchRequest
    {
        [JsonPropertyName("query")]
        public string Query { get; set; } = string.Empty;

        [JsonPropertyName("search_depth")]
        public string SearchDepth { get; set; } = "basic";

        [JsonPropertyName("max_results")]
        public int MaxResults { get; set; }

        [JsonPropertyName("include_answer")]
        public bool IncludeAnswer { get; set; }

        [JsonPropertyName("include_raw_content")]
        public bool IncludeRawContent { get; set; }
    }
}
