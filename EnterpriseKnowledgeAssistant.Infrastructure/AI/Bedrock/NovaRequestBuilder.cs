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

        public Amazon.BedrockRuntime.Model.InvokeModelRequest Build(IReadOnlyCollection<Domain.Entities.Message> messages,
            string? knowledgeContext = null)
        {
            // Convert persisted conversation history into Nova user/assistant messages.
            var novaMessages = messages
                .Select(m => new NovaMessage
                {
                    Role = m.Role switch
                    {
                        DomainMessageRole.User => "user",
                        DomainMessageRole.Assistant => "assistant",

                        _ => throw new InvalidOperationException(
                            $"Unsupported message role: {m.Role}")
                    },

                    Content = new List<TextContent>
                    {
                new TextContent
                {
                    Text = m.Content
                }
                    }
                })
                .ToList();

            // RAG context is sent as a system instruction.
            // It is NOT added to the persisted conversation history.
            List<TextContent>? system = null;

            if (!string.IsNullOrWhiteSpace(knowledgeContext))
            {
                system = new List<TextContent>
                {
                    new TextContent
                    {
                        Text = $"""
                            You are an enterprise knowledge assistant.

                            Answer the user's question using only the enterprise knowledge provided below.

                            Rules:
                            - Base your answer on the provided enterprise knowledge.
                            - Do not invent facts that are not supported by the provided knowledge.
                            - If the provided knowledge does not contain enough information to answer the question, say that the answer could not be found in the available enterprise knowledge.
                            - Answer clearly and concisely.

                            Enterprise knowledge:

                            {knowledgeContext}
                            """
                    }
                };
            }

            var novaRequest = new NovaRequest
            {
                System = system,

                Messages = novaMessages,

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
                Body = new MemoryStream(
                    Encoding.UTF8.GetBytes(json))
            };
        }
    }
}
