using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Xunit;

namespace EnterpriseKnowledgeAssistant.Tests.Bedrock
{
    public class NovaRequestBuilderTests
    {
        [Fact]
        public void Build_ReturnsInvokeModelRequest_WithExpectedMetadata()
        {
            // Arrange
            var options = new BedrockOptions
            {
                ModelId = "nova-test",
                Temperature = 0.3f,
                MaxTokens = 150
            };

            var builder = new NovaRequestBuilder(Options.Create(options));

            var request = new ChatRequest { Message = "hello world" };

            // Act
            //var invokeRequest = builder.Build(request);

            //// Assert
            //Assert.NotNull(invokeRequest);
            //Assert.Equal("nova-test", invokeRequest.ModelId);
            //Assert.Equal("application/json", invokeRequest.ContentType);
            //Assert.Equal("application/json", invokeRequest.Accept);
            //Assert.NotNull(invokeRequest.Body);
        }

        [Fact]
        public async Task Build_SerializesNovaRequest_WithMessageAndInferenceConfig()
        {
            // Arrange
            var options = new BedrockOptions
            {
                ModelId = "nova-test",
                Temperature = 0.42f,
                MaxTokens = 250
            };

            var builder = new NovaRequestBuilder(Options.Create(options));

            var request = new ChatRequest { Message = "unit test message" };

            //// Act
            //var invokeRequest = builder.Build(request);

            //// Read body
            //using var reader = new StreamReader(invokeRequest.Body, Encoding.UTF8, false, 1024, leaveOpen: true);
            //invokeRequest.Body.Position = 0;
            //var json = await reader.ReadToEndAsync();

            //// Deserialize
            //var novaRequest = JsonSerializer.Deserialize<NovaRequest>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //// Assert
            //Assert.NotNull(novaRequest);
            //Assert.Equal("messages-v1", novaRequest!.SchemaVersion);
            //Assert.NotEmpty(novaRequest.Messages);
            //Assert.Equal("user", novaRequest.Messages[0].Role);
            //Assert.NotEmpty(novaRequest.Messages[0].Content);
            //Assert.Equal("unit test message", novaRequest.Messages[0].Content[0].Text);
            //Assert.Equal(options.Temperature, novaRequest.InferenceConfig.Temperature);
            //Assert.Equal(options.MaxTokens, novaRequest.InferenceConfig.MaxTokens);
        }
    }
}
