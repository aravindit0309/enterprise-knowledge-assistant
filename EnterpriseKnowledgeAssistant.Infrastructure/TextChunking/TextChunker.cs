using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;

namespace EnterpriseKnowledgeAssistant.Infrastructure.TextChunking
{
    public sealed class TextChunker : ITextChunker
    {
        private const int ChunkSize = 1000;
        private const int ChunkOverlap = 200;

        public IReadOnlyList<string> Chunk(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<string>();
            }

            var chunks = new List<string>();
            var start = 0;

            while (start < text.Length)
            {
                var length = Math.Min(ChunkSize,text.Length - start);

                var chunk = text.Substring(start, length).Trim();

                if (!string.IsNullOrWhiteSpace(chunk))
                {
                    chunks.Add(chunk);
                }

                if (start + length >= text.Length)
                {
                    break;
                }

                start += ChunkSize - ChunkOverlap;
            }

            return chunks;
        }
    }
}
