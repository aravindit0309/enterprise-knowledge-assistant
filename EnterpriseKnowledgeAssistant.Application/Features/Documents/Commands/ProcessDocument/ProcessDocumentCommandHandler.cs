using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using EnterpriseKnowledgeAssistant.Domain.Documents;
using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Documents.Commands.ProcessDocument
{
    public sealed class ProcessDocumentCommandHandler: IRequestHandler<ProcessDocumentCommand, ProcessDocumentResponse>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ITextExtractorResolver _textExtractorResolver;
        private readonly ITextChunker _textChunker;
        private readonly IDocumentChunkRepository _documentChunkRepository;

        public ProcessDocumentCommandHandler( 
            IDocumentRepository documentRepository,
            IFileStorageService fileStorageService,
            ITextExtractorResolver textExtractorResolver,
            ITextChunker textChunker,
            IDocumentChunkRepository documentChunkRepository)
        {
            _documentRepository = documentRepository;
            _fileStorageService = fileStorageService;
            _textExtractorResolver = textExtractorResolver;
            _textChunker = textChunker;
            _documentChunkRepository = documentChunkRepository;
        }

        public async Task<ProcessDocumentResponse> Handle(ProcessDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await _documentRepository.GetByIdAsync(request.DocumentId, cancellationToken);

            if (document is null)
            {
                throw new KeyNotFoundException($"Document '{request.DocumentId}' was not found.");
            }

            await using var stream = await _fileStorageService.OpenReadAsync(document.StoredFileName, cancellationToken);

            var extension = Path.GetExtension(document.FileName);

            var extractor = _textExtractorResolver.Resolve(extension);

            var extractedText = await extractor.ExtractTextAsync(stream, cancellationToken);

            // NEW: Split extracted text into chunks
            var chunkContents = _textChunker.Chunk(extractedText);

            await _documentChunkRepository.DeleteByDocumentIdAsync( document.Id, cancellationToken);

            var chunks = chunkContents
                .Select((content, index) => new DocumentChunk
                {
                    Id = Guid.NewGuid(),
                    DocumentId = document.Id,
                    ChunkIndex = index,
                    Content = content,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

            if (chunks.Count > 0)
            {
                await _documentChunkRepository.AddRangeAsync(chunks, cancellationToken);
            }
            // NEW: Split extracted text into chunks

            return new ProcessDocumentResponse(
                document.Id,
                document.FileName,
                extractedText);
        }
    }
}
