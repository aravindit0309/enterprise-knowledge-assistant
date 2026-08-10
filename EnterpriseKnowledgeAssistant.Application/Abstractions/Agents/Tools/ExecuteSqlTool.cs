using EnterpriseKnowledgeAssistant.Application.Abstractions.Data;
using Microsoft.Extensions.Logging;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.Tools
{
    public sealed class ExecuteSqlTool : IAgentTool
    {
        private readonly ISqlQueryExecutor _sqlQueryExecutor;
        private readonly ISqlValidator _sqlValidator;
        private readonly ISqlGenerator _sqlGenerator;
        private readonly ILogger<ExecuteSqlTool> _logger;

        public ExecuteSqlTool(ISqlQueryExecutor sqlQueryExecutor, ISqlValidator sqlValidator, ISqlGenerator sqlGenerator, ILogger<ExecuteSqlTool> logger)
        {
            _sqlQueryExecutor = sqlQueryExecutor;
            _sqlValidator = sqlValidator;
            _sqlGenerator = sqlGenerator;
            _logger = logger;
        }

        public string Name => AgentToolNames.ExecuteSql;

        public string Description => "Execute a read-only SQL query against the approved enterprise database.";

        public async Task<AgentToolResult> ExecuteAsync(string input, CancellationToken cancellationToken = default)
        {
            var sql = await _sqlGenerator.GenerateAsync(input, cancellationToken);

            if (!_sqlValidator.IsValid(sql))
            {
                _logger.LogWarning("Rejected invalid SQL query.");

                return new AgentToolResult(
                    false,
                    "The SQL query was rejected because only safe read-only SELECT queries are allowed.",
                    []);
            }

            var rows = await _sqlQueryExecutor.ExecuteAsync(sql, cancellationToken);

            if (rows.Count == 0)
            {
                return new AgentToolResult(true, "The SQL query returned no results.", []);
            }

            var content = string.Join(
                Environment.NewLine,
                rows.Select(row =>
                    string.Join(
                        " | ",
                        row.Select(column =>
                            $"{column.Key}: {column.Value}"))));

            _logger.LogInformation("SQL query executed successfully and returned {RowCount} rows.", rows.Count);

            return new AgentToolResult(true, content, []);
        }
    }
}
