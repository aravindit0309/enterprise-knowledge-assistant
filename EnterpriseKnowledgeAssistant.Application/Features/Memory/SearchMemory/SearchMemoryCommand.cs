using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Memory.SearchMemory
{
    public sealed record SearchMemoryCommand(string Query,int Limit = 5) : IRequest<IReadOnlyList<MemorySearchResult>>;

    public sealed record MemorySearchResult(Guid Id, string Content);
}
