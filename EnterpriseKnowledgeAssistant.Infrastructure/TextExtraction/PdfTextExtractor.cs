using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using System.Text;
using UglyToad.PdfPig;

namespace EnterpriseKnowledgeAssistant.Infrastructure.TextExtraction
{
    public sealed class PdfTextExtractor : ITextExtractor
    {
        public bool CanHandle(string fileExtension)
        {
            return string.Equals(fileExtension, ".pdf", StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> ExtractTextAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            var textBuilder = new StringBuilder();

            using var document = PdfDocument.Open(stream);

            foreach (var page in document.GetPages())
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (textBuilder.Length > 0)
                {
                    textBuilder.AppendLine();
                }

                textBuilder.Append(page.Text);
            }

            return Task.FromResult(textBuilder.ToString());
        }
    }
}
