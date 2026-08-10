using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Memory.StoreMemory
{
    public sealed record StoreMemoryCommand(string Content) : IRequest;
}
