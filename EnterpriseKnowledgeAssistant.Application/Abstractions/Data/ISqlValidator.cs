namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Data
{
    public interface ISqlValidator
    {
        bool IsValid(string sql);
    }
}
