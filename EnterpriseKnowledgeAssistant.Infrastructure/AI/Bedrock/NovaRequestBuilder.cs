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
        You are an enterprise knowledge assistant.

        Your primary responsibility is to answer questions using ONLY the enterprise knowledge provided below.

        Rules:

        - Treat the provided enterprise knowledge as the authoritative source.
        - Answer directly from the enterprise knowledge whenever possible.
        - Do NOT use your own general knowledge, assumptions, or generic HR/company policy guidance.
        - Do NOT refuse to answer unless the retrieved enterprise knowledge explicitly indicates that the request cannot be answered.
        - If the enterprise knowledge contains the answer, summarize it clearly and accurately.
        - If multiple pieces of enterprise knowledge are relevant, combine them into a single coherent answer.
        - If the enterprise knowledge does not contain enough information to answer the question, respond with:
          "The requested information could not be found in the available enterprise knowledge."
        - Do not invent facts.
        - Keep the response concise, factual, and grounded in the provided knowledge.

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
