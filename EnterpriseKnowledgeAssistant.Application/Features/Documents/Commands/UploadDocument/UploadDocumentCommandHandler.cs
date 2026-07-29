using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using EnterpriseKnowledgeAssistant.Domain.Documents;
using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Documents.Commands.UploadDocument
{
    public class UploadDocumentCommandHandler
    : IRequestHandler<UploadDocumentCommand, UploadDocumentResponse>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IDocumentRepository _documentRepository;

        public UploadDocumentCommandHandler(
            IFileStorageService fileStorageService,
            IDocumentRepository documentRepository)
        {
            _fileStorageService = fileStorageService;
            _documentRepository = documentRepository;
        }

        public async Task<UploadDocumentResponse> Handle( UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            // Step 1 - Save file
            var storedFileName = await _fileStorageService.SaveAsync( request.FileStream, request.FileName, cancellationToken);

            // Step 2 - Create domain entity
            var document = new Document( request.FileName, storedFileName, request.ContentType,request.FileSize);

            // Step 3 - Persist metadata
            await _documentRepository.AddAsync(document, cancellationToken);
            await _documentRepository.SaveChangesAsync(cancellationToken);

            // Step 4 - Return response
            return new UploadDocumentResponse(
                document.Id,
                document.FileName,
                DocumentUploadStatus.Success);
        }
    }
}
