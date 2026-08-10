namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Data
{
    public interface ISqlGenerator
    {
        Task<string> GenerateAsync(string userRequest, CancellationToken cancellationToken = default);
    }
}
