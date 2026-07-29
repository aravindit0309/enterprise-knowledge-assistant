using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Documents.Commands.UploadDocument
{
    public record UploadDocumentCommand( Stream FileStream, string FileName,
    string ContentType, long FileSize ) : IRequest<UploadDocumentResponse>;
}
