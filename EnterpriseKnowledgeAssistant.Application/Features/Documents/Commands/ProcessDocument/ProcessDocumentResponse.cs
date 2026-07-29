namespace EnterpriseKnowledgeAssistant.Application.Features.Documents.Commands.ProcessDocument
{
    public sealed record ProcessDocumentResponse(
    Guid DocumentId,
    string FileName,
    string ExtractedText);
}
