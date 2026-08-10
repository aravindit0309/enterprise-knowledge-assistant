using EnterpriseKnowledgeAssistant.Application.Abstractions.Persistence;
using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Memory.SearchMemory
{
    public sealed class SearchMemoryCommandHandler : IRequestHandler<SearchMemoryCommand, IReadOnlyList<MemorySearchResult>>
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IMemoryRepository _memoryRepository;

        public SearchMemoryCommandHandler(IEmbeddingService embeddingService, IMemoryRepository memoryRepository)
        {
            _embeddingService = embeddingService;
            _memoryRepository = memoryRepository;
        }

        public async Task<IReadOnlyList<MemorySearchResult>> Handle(SearchMemoryCommand request, CancellationToken cancellationToken)
        {
            var embedding = await _embeddingService.GenerateEmbeddingAsync(request.Query, cancellationToken);

            var memories = await _memoryRepository.SearchSimilarAsync(embedding.ToArray(), request.Limit, cancellationToken);

            return memories.Select(memory => new MemorySearchResult(memory.Id, memory.Content)).ToList();
        }
    }
}
