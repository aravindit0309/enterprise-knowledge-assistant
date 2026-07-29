namespace EnterpriseKnowledgeAssistant.Application.Common.Interfaces
{
    public interface ITextExtractorResolver
    {
        ITextExtractor Resolve(string fileExtension);
    }
}
