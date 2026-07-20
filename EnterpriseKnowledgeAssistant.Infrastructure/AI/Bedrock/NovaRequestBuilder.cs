using Amazon.BedrockRuntime.Model;
using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

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

        public InvokeModelRequest Build(ChatRequest request)
        {
            var novaRequest = new NovaRequest
            {
                Messages =
            {
                new NovaMessage
                {
                    Role = "user",
                    Content =
                    {
                        new TextContent
                        {
                            Text = request.Message
                        }
                    }
                }
            },

                InferenceConfig = new InferenceConfig
                {
                    Temperature = _options.Temperature,
                    MaxTokens = _options.MaxTokens
                }
            };

            var json = JsonSerializer.Serialize(novaRequest, JsonOptions);

            return new InvokeModelRequest
            {
                ModelId = _options.ModelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(Encoding.UTF8.GetBytes(json))
            };
        }
    }
}
