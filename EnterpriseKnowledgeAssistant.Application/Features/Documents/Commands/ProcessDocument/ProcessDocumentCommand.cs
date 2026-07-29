using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Documents.Commands.ProcessDocument
{
    public sealed record ProcessDocumentCommand(Guid DocumentId): IRequest<ProcessDocumentResponse>;
}
    