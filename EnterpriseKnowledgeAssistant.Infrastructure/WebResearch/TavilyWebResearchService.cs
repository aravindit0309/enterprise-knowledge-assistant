using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.WebResearch;
using EnterpriseKnowledgeAssistant.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EnterpriseKnowledgeAssistant.Infrastructure.WebResearch
{
    public sealed class TavilyWebResearchService : IWebResearchService
    {
        private readonly HttpClient _httpClient;
        private readonly TavilyOptions _options;
        private readonly ILogger<TavilyWebResearchService> _logger;

        public TavilyWebResearchService(
            HttpClient httpClient,
            IOptions<TavilyOptions> options,
            ILogger<TavilyWebResearchService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<WebResearchResult> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            var request = new TavilySearchRequest
            {
                Query = query,
                SearchDepth = "basic",
                MaxResults = _options.MaxResults,
                IncludeAnswer = false,
                IncludeRawContent = false
            };

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_options.BaseUrl.TrimEnd('/')}/search");

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _options.ApiKey);

            httpRequest.Content = JsonContent.Create(request);

            _logger.LogInformation("Executing Tavily web search for query: {Query}", query);

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            response.EnsureSuccessStatusCode();

            var tavilyResponse = await response.Content.ReadFromJsonAsync<TavilySearchResponse>(cancellationToken);

            if (tavilyResponse?.Results is null)
            {
                return new WebResearchResult([]);
            }

            var results = tavilyResponse.Results
                .Where(result =>
                    !string.IsNullOrWhiteSpace(result.Title) &&
                    !string.IsNullOrWhiteSpace(result.Url))
                .Select(result => new WebResearchItem(
                    result.Title,
                    result.Url,
                    result.Content ?? string.Empty))
                .ToList();

            _logger.LogInformation("Tavily returned {ResultCount} results for query: {Query}",
                results.Count, query);

            return new WebResearchResult(results);
        }
    }
}
