using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock
{
    public class AmazonBedrockEmbeddingService : IEmbeddingService
    {
        private const string ModelId = "amazon.titan-embed-text-v2:0";
        private const int Dimensions = 256;

        private readonly IAmazonBedrockRuntime _bedrockRuntime;
        private readonly ILogger<AmazonBedrockEmbeddingService> _logger;

        public AmazonBedrockEmbeddingService(IAmazonBedrockRuntime bedrockRuntime, ILogger<AmazonBedrockEmbeddingService> logger)
        {
            _bedrockRuntime = bedrockRuntime;
            _logger = logger;
        }

        public async Task<IReadOnlyList<float>> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Text cannot be empty.",nameof(text));
            }

            var requestBody = new
            {
                inputText = text,
                dimensions = Dimensions,
                normalize = true
            };

            var json = JsonSerializer.Serialize(requestBody);

            var request = new InvokeModelRequest
            {
                ModelId = ModelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(Encoding.UTF8.GetBytes(json))
            };

            _logger.LogInformation("Generating embedding using Bedrock model {ModelId}",ModelId);

            try
            {
                var response = await _bedrockRuntime.InvokeModelAsync(request, cancellationToken);

                using var reader = new StreamReader(response.Body);

                var responseJson = await reader.ReadToEndAsync();

                var embeddingResponse =
                    JsonSerializer.Deserialize<TitanEmbeddingResponse>(
                        responseJson,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (embeddingResponse?.Embedding is null || embeddingResponse.Embedding.Count == 0)
                {
                    throw new InvalidOperationException("Bedrock returned an empty embedding.");
                }

                return embeddingResponse.Embedding;
            }
            catch (AmazonBedrockRuntimeException ex)
            {
                _logger.LogError(ex, "Embedding generation failed for model {ModelId}", ModelId);
                throw;
            }
        }

        private sealed class TitanEmbeddingResponse
        {
            public List<float> Embedding { get; set; } = [];
        }
    }
}
