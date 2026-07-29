using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;

namespace EnterpriseKnowledgeAssistant.Infrastructure.TextExtraction
{
    public sealed class TextExtractor : ITextExtractor
    {
        public bool CanHandle(string fileExtension)
        {
            return string.Equals(fileExtension, ".txt",StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> ExtractTextAsync(Stream stream,CancellationToken cancellationToken)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            using var reader = new StreamReader(stream, leaveOpen: true);

            return await reader.ReadToEndAsync(cancellationToken);
        }
    }
}
