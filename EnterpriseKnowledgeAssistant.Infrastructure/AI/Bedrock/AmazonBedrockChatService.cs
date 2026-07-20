using Amazon.BedrockRuntime;
using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        public Task<ChatResponse> GetChatResponseAsync(ChatRequest request)
        {
            var invokeRequest = _requestBuilder.Build(request);

            _logger.LogInformation( "Prepared request for Bedrock model {ModelId}", invokeRequest.ModelId);

            return Task.FromResult(new ChatResponse
            {
                Response = "This response will come from Amazon Bedrock.",
                ModelUsed = invokeRequest.ModelId
            });
        }
    }
}
