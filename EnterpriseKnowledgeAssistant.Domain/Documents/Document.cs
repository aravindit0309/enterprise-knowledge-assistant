namespace EnterpriseKnowledgeAssistant.Domain.Documents
{
    public class Document
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; } = string.Empty;
        public string StoredFileName { get; private set; } = string.Empty;
        public string ContentType { get; private set; } = string.Empty;
        public long FileSize { get; private set; }
        public DateTime UploadedAt { get; private set; }
        public DocumentStatus Status { get; private set; }
        public ICollection<DocumentChunk> Chunks { get; set; } = new List<DocumentChunk>();

        // Required by EF Core
        private Document()
        {
        }

        public Document(string fileName, string storedFileName, string contentType, long fileSize)
        {
            Id = Guid.NewGuid();
            FileName = fileName;
            StoredFileName = storedFileName;
            ContentType = contentType;
            FileSize = fileSize;
            UploadedAt = DateTime.UtcNow;
            Status = DocumentStatus.Uploaded;
        }

        public void MarkProcessing()
        {
            Status =DocumentStatus.Processing;
        }

        public void MarkCompleted()
        {
            Status = DocumentStatus.Completed;
        }

        public void MarkFailed()
        {
            Status = DocumentStatus.Failed;
        }
    }
}
