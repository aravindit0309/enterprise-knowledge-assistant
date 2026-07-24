using Amazon.BedrockRuntime;
using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock
{
    public class AmazonBedrockChatService : IChatService
    {
        private readonly IAmazonBedrockRuntime _bedrockRuntime;
        private readonly IBedrockRequestBuilder _requestBuilder;
        private readonly ILogger<AmazonBedrockChatService> _logger;

        public AmazonBedrockChatService( IAmazonBedrockRuntime bedrockRuntime, 
            ILogger<AmazonBedrockChatService> logger, IBedrockRequestBuilder requestBuilder)
        {
           
            _bedrockRuntime = bedrockRuntime;
            _logger = logger;
            _requestBuilder = requestBuilder;
        }

        public async Task<ChatResponse> SendAsync(IReadOnlyCollection<Domain.Entities.Message> messages, CancellationToken cancellationToken = default)
        {
            try
            {
                var invokeRequest = _requestBuilder.Build(messages);

                _logger.LogInformation( "Invoking Bedrock model {ModelId}", invokeRequest.ModelId);

                var response = await _bedrockRuntime.InvokeModelAsync(invokeRequest, cancellationToken);

                using var reader = new StreamReader(response.Body);

                var json = await reader.ReadToEndAsync();

                var novaResponse = JsonSerializer.Deserialize<NovaResponse>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                var answer = novaResponse?.Output?.Message?.Content?.FirstOrDefault()?.Text ?? "No response generated.";

                return new ChatResponse
                {
                    Response = answer,
                    ModelUsed = invokeRequest.ModelId
                };
            }
            catch (AmazonBedrockRuntimeException ex)
            {
                _logger.LogError(ex, "Bedrock invocation failed.");
                throw;
            }
        }
    }
}
