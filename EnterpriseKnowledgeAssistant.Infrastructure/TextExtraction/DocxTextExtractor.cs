using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Infrastructure.TextExtraction
{
    public sealed class DocxTextExtractor : ITextExtractor
    {
        public bool CanHandle(string fileExtension)
        {
            return string.Equals(fileExtension, ".docx",StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> ExtractTextAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            using var document = WordprocessingDocument.Open(
                stream,
                false);

            var body = document.MainDocumentPart?.Document.Body;

            if (body is null)
            {
                return Task.FromResult(string.Empty);
            }

            var textBuilder = new StringBuilder();

            foreach (var paragraph in body.Descendants<Paragraph>())
            {
                cancellationToken.ThrowIfCancellationRequested();

                var text = paragraph.InnerText;

                if (string.IsNullOrWhiteSpace(text))
                {
                    continue;
                }

                textBuilder.AppendLine(text);
            }

            return Task.FromResult(textBuilder.ToString());
        }
    }
}
