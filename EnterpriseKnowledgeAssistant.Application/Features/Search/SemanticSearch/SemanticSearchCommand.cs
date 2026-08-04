using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Search.SemanticSearch
{
    public sealed record SemanticSearchCommand(
    string Query,
    int Limit = 5)
    : IRequest<IReadOnlyList<SemanticSearchResult>>;

    public sealed record SemanticSearchResult(
        Guid DocumentId,
        string DocumentName,
        int ChunkIndex,
        string Content);
}
