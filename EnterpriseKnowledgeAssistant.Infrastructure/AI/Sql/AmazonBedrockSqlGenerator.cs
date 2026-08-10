using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using EnterpriseKnowledgeAssistant.Application.Abstractions.Data;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock.Models;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

public sealed class AmazonBedrockSqlGenerator : ISqlGenerator
{
    private readonly IAmazonBedrockRuntime _bedrockRuntime;
    private readonly BedrockOptions _options;
    private readonly ILogger<AmazonBedrockSqlGenerator> _logger;

    public AmazonBedrockSqlGenerator(
        IAmazonBedrockRuntime bedrockRuntime,
        IOptions<BedrockOptions> options,
        ILogger<AmazonBedrockSqlGenerator> logger)
    {
        _bedrockRuntime = bedrockRuntime;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> GenerateAsync(string userRequest, CancellationToken cancellationToken = default)
    {
        var prompt = $"""
You are a SQL generation assistant.

Generate a PostgreSQL SELECT query for the user's request.

The table name is exactly "Documents" and must always be quoted.

You may ONLY query the following table:

Documents
- Id
- FileName
- StoredFileName
- ContentType
- FileSize
- UploadedAt
- Status

Always use double quotes around the table and column names exactly as shown.

Rules:
- Return ONLY the SQL query.
- Generate SELECT statements only.
- Do not use INSERT, UPDATE, DELETE, DROP, ALTER, CREATE, or TRUNCATE.
- Do not access any table other than Documents.
- Do not include markdown.
- Do not include explanations.
- Do not include a trailing semicolon.

User request:
{userRequest}
""";

        var request = new NovaRequest
        {
            System =
            [
                new TextContent
                {
                    Text = """
You generate safe, read-only PostgreSQL queries for an enterprise application.
Return only SQL.
"""
                }
            ],

            Messages =
            [
                new NovaMessage
                {
                    Role = "user",
                    Content =
                    [
                        new TextContent
                        {
                            Text = prompt
                        }
                    ]
                }
            ],

            InferenceConfig = new InferenceConfig
            {
                Temperature = 0,
                MaxTokens = 500
            }
        };

        var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var invokeRequest = new InvokeModelRequest
        {
            ModelId = _options.ModelId,
            ContentType = "application/json",
            Accept = "application/json",
            Body = new MemoryStream(Encoding.UTF8.GetBytes(json))
        };

        _logger.LogInformation("Generating SQL using Bedrock model {ModelId}", _options.ModelId);

        _logger.LogInformation("SQL generation request: {Request}", json);

        var response = await _bedrockRuntime.InvokeModelAsync(invokeRequest, cancellationToken);

        using var reader = new StreamReader(response.Body);

        var responseJson = await reader.ReadToEndAsync();

        var novaResponse = JsonSerializer.Deserialize<NovaResponse>(
            responseJson,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        var sql = novaResponse?.Output?.Message?.Content?.FirstOrDefault()?.Text
           ?.Trim()
           ?? string.Empty;

        sql = sql
            .Replace("```sql", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("```", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Trim();

        return sql;
    }
}