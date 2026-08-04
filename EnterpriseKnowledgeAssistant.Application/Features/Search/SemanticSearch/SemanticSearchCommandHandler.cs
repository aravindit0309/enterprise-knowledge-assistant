using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Search.SemanticSearch
{
    public sealed class SemanticSearchCommandHandler: IRequestHandler<SemanticSearchCommand,
        IReadOnlyList<SemanticSearchResult>>
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IDocumentChunkRepository _documentChunkRepository;

        public SemanticSearchCommandHandler(
            IEmbeddingService embeddingService,
            IDocumentChunkRepository documentChunkRepository)
        {
            _embeddingService = embeddingService;
            _documentChunkRepository = documentChunkRepository;
        }

        public async Task<IReadOnlyList<SemanticSearchResult>> Handle(
            SemanticSearchCommand request,
            CancellationToken cancellationToken)
        {
            var embedding = await _embeddingService.GenerateEmbeddingAsync(
                request.Query,
                cancellationToken);

            var chunks = await _documentChunkRepository.SearchSimilarAsync(
                embedding.ToArray(),
                request.Limit,
                cancellationToken);

            return chunks
                .Select(chunk => new SemanticSearchResult(
                    chunk.DocumentId,
                    chunk.Document.FileName,
                    chunk.ChunkIndex,
                    chunk.Content))
                .ToList();
        }
    }
}
