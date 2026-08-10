namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Data
{
    public interface ISqlQueryExecutor
    {
        Task<IReadOnlyList<IReadOnlyDictionary<string, object?>>> ExecuteAsync(
            string sql,
            CancellationToken cancellationToken = default);
    }
}
