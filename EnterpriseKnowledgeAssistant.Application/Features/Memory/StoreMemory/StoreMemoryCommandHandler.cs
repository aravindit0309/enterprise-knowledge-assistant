using EnterpriseKnowledgeAssistant.Application.Abstractions.Persistence;
using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using EnterpriseKnowledgeAssistant.Domain.Memory;
using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Memory.StoreMemory;

public sealed class StoreMemoryCommandHandler : IRequestHandler<StoreMemoryCommand>
{
    private readonly IMemoryRepository _memoryRepository;
    private readonly IEmbeddingService _embeddingService;

    public StoreMemoryCommandHandler(IMemoryRepository memoryRepository, IEmbeddingService embeddingService)
    {
        _memoryRepository = memoryRepository;
        _embeddingService = embeddingService;
    }

    public async Task Handle(StoreMemoryCommand request, CancellationToken cancellationToken)
    {
        var embedding = await _embeddingService.GenerateEmbeddingAsync(request.Content, cancellationToken);

        var memory = new MemoryRecord
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            CreatedAt = DateTime.UtcNow,
            Embedding = embedding.ToArray()
        };

        await _memoryRepository.AddAsync(memory, cancellationToken);
    }
}