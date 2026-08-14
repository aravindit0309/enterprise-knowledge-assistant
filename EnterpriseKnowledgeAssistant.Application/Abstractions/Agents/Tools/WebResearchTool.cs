using Microsoft.Extensions.Logging;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.Tools
{
    public sealed class WebResearchTool : IAgentTool
    {
        private readonly IWebResearchService _webResearchService;
        private readonly ILogger<WebResearchTool> _logger;

        public WebResearchTool(IWebResearchService webResearchService, ILogger<WebResearchTool> logger)
        {
            _webResearchService = webResearchService;
            _logger = logger;
        }

        public string Name => AgentToolNames.WebResearch;

        public string Description => "Search the public web for current external information.";

        public async Task<AgentToolResult> ExecuteAsync(string input, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new AgentToolResult(false, "No web research query was provided.", []);
            }

            try
            {
                var result = await _webResearchService.SearchAsync(input, cancellationToken);

                if (result.Results.Count == 0)
                {
                    return new AgentToolResult(
                        false,
                        """
                    The web research search did not find any relevant information
                    for the user's request.
                    """,
                        []);
                }

                var content = string.Join(
                    Environment.NewLine +
                    "----------------------------------------" +
                    Environment.NewLine,
                    result.Results.Select((item, index) =>
                        $"""
                    Source {index + 1}
                    Title: {item.Title}
                    URL: {item.Url}

                    {item.Content}
                    """));

                _logger.LogInformation("Web research returned {ResultCount} results for query: {Query}",
                    result.Results.Count,
                    input);

                return new AgentToolResult(true, content, []);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web research failed for query: {Query}", input);

                return new AgentToolResult(
                    false,
                    """
                Web research could not be completed at this time.
                The assistant should not rely on external web information
                for this request.
                """,
                    []);
            }
        }
    }
}
