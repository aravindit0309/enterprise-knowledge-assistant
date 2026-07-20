using Amazon.BedrockRuntime.Model;
using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock
{
    public interface IBedrockRequestBuilder
    {
        InvokeModelRequest Build(ChatRequest request);
    }
}
