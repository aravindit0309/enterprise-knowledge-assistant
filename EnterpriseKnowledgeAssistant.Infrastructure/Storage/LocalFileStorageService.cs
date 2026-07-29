using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Storage
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _uploadFolder;
        private readonly IOptions<StorageOptions> _storageOptions;

        public LocalFileStorageService(IOptions<StorageOptions> storageOptions)
        {
            _storageOptions = storageOptions;
            _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), _storageOptions.Value.UploadPath);

            Directory.CreateDirectory(_uploadFolder);
        }

        public async Task<string> SaveAsync(Stream stream,string fileName,CancellationToken cancellationToken)
        {
            var extension = Path.GetExtension(fileName);

            var storedFileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(_uploadFolder, storedFileName);

            await using var fileStream = File.Create(filePath);

            await stream.CopyToAsync(fileStream, cancellationToken);

            return storedFileName;  
        }

        public Task<Stream> OpenReadAsync(string storedFileName ,CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(_uploadFolder, storedFileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Stored file '{storedFileName}' was not found.",filePath);
            }

            Stream stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true);

            return Task.FromResult(stream);
        }
    }

}
