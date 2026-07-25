namespace EnterpriseKnowledgeAssistant.Application.Common.Interfaces
{
    public interface ITextChunker
    {
        IReadOnlyList<string> Chunk(string text);
    }
}
