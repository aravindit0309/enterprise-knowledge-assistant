using EnterpriseKnowledgeAssistant.Application.Abstractions.Data;

public sealed class SqlValidator : ISqlValidator
{
    private static readonly string[] ForbiddenKeywords =
    [
        "INSERT",
        "UPDATE",
        "DELETE",
        "DROP",
        "ALTER",
        "CREATE",
        "TRUNCATE"
    ];

    public bool IsValid(string sql)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            return false;
        }

        var normalizedSql = sql.Trim();

        if (!normalizedSql.StartsWith( "SELECT ", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (normalizedSql.Contains(';'))
        {
            return false;
        }

        return !ForbiddenKeywords.Any(keyword =>
            normalizedSql.Contains(
                keyword,
                StringComparison.OrdinalIgnoreCase));
    }
}