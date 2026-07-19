using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;
using Microsoft.Extensions.Options;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock
{
    public class AmazonBedrockChatService : IChatService
    {
        private readonly BedrockOptions _options;

        public AmazonBedrockChatService(
            IOptions<BedrockOptions> options)
        {
            _options = options.Value;
        }

        public Task<ChatResponse> GetChatResponseAsync(ChatRequest request)
        {
            return Task.FromResult(new ChatResponse
            {
                Response = "This response will come from Amazon Bedrock.",
                ModelUsed = "Placeholder"
            });
        }
    }
}
