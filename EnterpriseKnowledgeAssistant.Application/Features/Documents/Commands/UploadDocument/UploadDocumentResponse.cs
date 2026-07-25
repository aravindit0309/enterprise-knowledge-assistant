namespace EnterpriseKnowledgeAssistant.Application.Features.Documents.Commands.UploadDocument
{
    public record UploadDocumentResponse(
    Guid DocumentId,
    string FileName,
    DocumentUploadStatus Status);
}
