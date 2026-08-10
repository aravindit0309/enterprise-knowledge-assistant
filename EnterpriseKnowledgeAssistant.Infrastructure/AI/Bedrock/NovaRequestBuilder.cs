//using Amazon.BedrockRuntime.Model;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock.Models;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DomainMessageRole = EnterpriseKnowledgeAssistant.Domain.Enums.MessageRole;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock
{
    public class NovaRequestBuilder : IBedrockRequestBuilder
    {
        private readonly BedrockOptions _options;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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
        You are an enterprise assistant.

        Use the provided context to answer the user's current question.

        Rules:

        - Answer the user's question directly using ONLY the provided context.
        - Treat the provided context as authoritative.
        - Previous assistant responses are NOT authoritative and must NOT be used as a source of facts.
        - Use ONLY information contained in the provided context.
        - Do not use general knowledge, assumptions, typical practices, recommendations, or invented facts.
        - Do NOT add information that is not explicitly supported by the provided context.
        - If the context directly answers the question, give only the supported answer.
        - Do NOT expand the answer with reasons, considerations, examples, or advice unless they are explicitly present in the context.
        - If the context contains a SQL query result, interpret the result and answer the user's question using that result.
        - If the context contains multiple results, combine them only when relevant.
        - If the context does not contain enough information to answer the question, respond with:
          "The requested information could not be found in the provided context."
        - Do not expose SQL queries or internal execution details unless the user asks for them.
        - Keep the response concise and factual.

        Context:

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
