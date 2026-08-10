using EnterpriseKnowledgeAssistant.Application.Abstractions.Data;
using EnterpriseKnowledgeAssistant.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public sealed class SqlQueryExecutor : ISqlQueryExecutor
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<SqlQueryExecutor> _logger;

    public SqlQueryExecutor(AppDbContext dbContext, ILogger<SqlQueryExecutor> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IReadOnlyList<IReadOnlyDictionary<string, object?>>> ExecuteAsync(
        string sql, CancellationToken cancellationToken = default)
    {
        await using var command = _dbContext.Database.GetDbConnection().CreateCommand();

        command.CommandText = sql;

        if (command.Connection!.State != System.Data.ConnectionState.Open)
        {
            await command.Connection.OpenAsync(cancellationToken);
        }

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        _logger.LogInformation(
    "SQL result columns: {ColumnCount}",
    reader.FieldCount);

        var results = new List<IReadOnlyDictionary<string, object?>>();

        while (await reader.ReadAsync(cancellationToken))
        {
            _logger.LogInformation(
    "SQL row read: {Value}",
    reader.GetValue(0));

            var row = new Dictionary<string, object?>(
                StringComparer.OrdinalIgnoreCase);

            for (var i = 0; i < reader.FieldCount; i++)
            {
                row[reader.GetName(i)] =
                    await reader.IsDBNullAsync(i, cancellationToken)
                        ? null
                        : reader.GetValue(i);
            }

            results.Add(row);
        }

        return results;
    }
}