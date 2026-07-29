using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Embeddings.GenerateEmbedding
{
    public sealed class GenerateEmbeddingCommandHandler: IRequestHandler<GenerateEmbeddingCommand, GenerateEmbeddingResponse>
    {
        private readonly IEmbeddingService _embeddingService;

        public GenerateEmbeddingCommandHandler(IEmbeddingService embeddingService)
        {
            _embeddingService = embeddingService;
        }

        public async Task<GenerateEmbeddingResponse> Handle(GenerateEmbeddingCommand request,CancellationToken cancellationToken)
        {
            var embedding = await _embeddingService.GenerateEmbeddingAsync(request.Text,cancellationToken);

            return new GenerateEmbeddingResponse(embedding.Count,embedding);
        }
    }
}
