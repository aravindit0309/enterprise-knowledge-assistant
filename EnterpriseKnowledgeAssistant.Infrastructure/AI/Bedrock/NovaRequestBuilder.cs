//using Amazon.BedrockRuntime.Model;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock.Models;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using DomainMessageRole = EnterpriseKnowledgeAssistant.Domain.Enums.MessageRole;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock
{
    public class NovaRequestBuilder : IBedrockRequestBuilder
    {
        private readonly BedrockOptions _options;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public NovaRequestBuilder(IOptions<BedrockOptions> options)
        {
            _options = options.Value;
        }

        public Amazon.BedrockRuntime.Model.InvokeModelRequest Build(IReadOnlyCollection<Domain.Entities.Message> messages)
        {
            var novaRequest = new NovaRequest
            {
                Messages = messages
           .Select(m => new NovaMessage
           {
               Role = m.Role switch
               {
                   DomainMessageRole.User => "user",
                   DomainMessageRole.Assistant => "assistant",
                   DomainMessageRole.System => "system",
                   _ => throw new InvalidOperationException($"Unsupported message role: {m.Role}")
               },

               Content = new List<TextContent>
               {
                    new TextContent
                    {
                        Text = m.Content
                    }
               }
           })
           .ToList(),

                InferenceConfig = new InferenceConfig
                {
                    Temperature = _options.Temperature,
                    MaxTokens = _options.MaxTokens
                }
            };

            var json = JsonSerializer.Serialize(novaRequest, JsonOptions);

            return new Amazon.BedrockRuntime.Model.InvokeModelRequest
            {
                ModelId = _options.ModelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(Encoding.UTF8.GetBytes(json))
            };
        }
    }
}
