using EnterpriseKnowledgeAssistant.Application.Features.Memory.SearchMemory;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.Tools
{
    public sealed class SearchMemoryTool : IAgentTool
    {
        private const int DefaultResultLimit = 5;

        private readonly ISender _sender;
        private readonly ILogger<SearchMemoryTool> _logger;

        public SearchMemoryTool(ISender sender,ILogger<SearchMemoryTool> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public string Name => AgentToolNames.SearchMemory;

        public string Description => "Search the assistant's long-term memory for information previously remembered.";

        public async Task<AgentToolResult> ExecuteAsync(string input, CancellationToken cancellationToken = default)
        {
            var results = await _sender.Send(new SearchMemoryCommand(input, DefaultResultLimit), cancellationToken);

            if (results.Count == 0)
            {
                return new AgentToolResult(false, "No relevant information was found in long-term memory.", []);
            }

            var content = string.Join(
                Environment.NewLine +
                "----------------------------------------" +
                Environment.NewLine,
                results.Select(result =>
                    $"""
                {result.Content}
                """));

            _logger.LogInformation("Memory retrieved from semantic search: {Memory}", content);

            return new AgentToolResult(
                true,
                content,
                []);
        }
    }
}
