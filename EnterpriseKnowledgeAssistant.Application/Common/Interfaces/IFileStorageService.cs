namespace EnterpriseKnowledgeAssistant.Application.Common.Interfaces
{
	public interface IFileStorageService
	{
		Task<string> SaveAsync(Stream stream, string fileName, CancellationToken cancellationToken);
        Task<Stream> OpenReadAsync(string storedFileName,CancellationToken cancellationToken);
    }
}
	