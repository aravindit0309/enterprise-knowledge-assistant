using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;

namespace EnterpriseKnowledgeAssistant.Infrastructure.TextExtraction
{
    public sealed class TextExtractorResolver : ITextExtractorResolver
    {
        private readonly IEnumerable<ITextExtractor> _extractors;

        public TextExtractorResolver(IEnumerable<ITextExtractor> extractors)
        {
            _extractors = extractors;
        }

        public ITextExtractor Resolve(string fileExtension)
        {
            var extractor = _extractors.FirstOrDefault(x => x.CanHandle(fileExtension));

            if (extractor is null)
            {
                throw new NotSupportedException( $"No text extractor is registered for '{fileExtension}'.");
            }

            return extractor;
        }
    }
}
