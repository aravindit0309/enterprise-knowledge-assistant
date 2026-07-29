using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Embeddings.GenerateEmbedding
{
    public sealed record GenerateEmbeddingCommand(string Text) : IRequest<GenerateEmbeddingResponse>;

    public sealed record GenerateEmbeddingResponse(int Dimensions, IReadOnlyList<float> Embedding);
}
