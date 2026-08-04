using EnterpriseKnowledgeAssistant.Application.Features.Search.SemanticSearch;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.Tools
{
    public sealed class SearchKnowledgeBaseTool : IAgentTool
    {
        private const int DefaultResultLimit = 5;
        private readonly ISender _sender;
        private readonly ILogger<SearchKnowledgeBaseTool> _logger;

        public SearchKnowledgeBaseTool(ISender sender, ILogger<SearchKnowledgeBaseTool> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public string Name => AgentToolNames.SearchKnowledgeBase;

        public string Description =>
            "Search uploaded documents for information relevant to the user's question.";

        public async Task<AgentToolResult> ExecuteAsync(string input, CancellationToken cancellationToken = default)
        {
            var results = await _sender.Send( new SemanticSearchCommand(input, DefaultResultLimit), cancellationToken);

            if (results.Count == 0)
            {
                return new AgentToolResult(false,
    """
    The enterprise knowledge search did not find any relevant information for the user's request.

    The assistant should clearly state that the requested information could not be found in the available enterprise knowledge.
    """,
    []);
            }

            var content = string.Join(
     Environment.NewLine +
     "----------------------------------------" +
     Environment.NewLine,
     results.Select(result =>
         $"""
        Document: {result.DocumentName}
        Chunk: {result.ChunkIndex}  

        {result.Content}
        """));

            _logger.LogInformation("""Knowledge retrieved from semantic search:{Knowledge}""", content);
            var sources = results.Select(result => new AgentSource(result.DocumentId, result.DocumentName, result.ChunkIndex)).ToList();

            return new AgentToolResult(true, content, sources);
        }
    }
}
