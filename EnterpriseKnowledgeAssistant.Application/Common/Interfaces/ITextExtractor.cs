namespace EnterpriseKnowledgeAssistant.Application.Common.Interfaces
{
    public interface ITextExtractor
    {
        bool CanHandle(string fileExtension);

        Task<string> ExtractTextAsync(Stream stream, CancellationToken cancellationToken);
    }
}
