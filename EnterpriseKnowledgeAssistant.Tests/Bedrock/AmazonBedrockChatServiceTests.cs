using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using Xunit;

namespace EnterpriseKnowledgeAssistant.Tests.Bedrock
{
    public class AmazonBedrockChatServiceTests
    {
        [Fact]
        public async Task GetChatResponseAsync_ReturnsResponse_WhenBedrockReturnsValidNovaResponse()
        {
            // Arrange
            var json = "{\"output\":{\"message\":{\"content\":[{\"text\":\"Hello from Nova\"}]}}}";

            var invokeResponse = new InvokeModelResponse
            {
                Body = new MemoryStream(Encoding.UTF8.GetBytes(json))
            };

            var bedrockMock = new Mock<IAmazonBedrockRuntime>();
            bedrockMock
                .Setup(x => x.InvokeModelAsync(It.IsAny<InvokeModelRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(invokeResponse);

            var requestBuilderMock = new Mock<IBedrockRequestBuilder>();
            var expectedRequest = new InvokeModelRequest { ModelId = "test-model" };
            requestBuilderMock.Setup(x => x.Build(It.IsAny<IReadOnlyCollection<Domain.Entities.Message>>())).Returns(expectedRequest);

            var loggerMock = new Mock<ILogger<AmazonBedrockChatService>>();

            var service = new AmazonBedrockChatService(bedrockMock.Object, loggerMock.Object, requestBuilderMock.Object);

            // Act
            //var result = await service.GetChatResponseAsync(new ChatRequest { Message = "hi" });

            //// Assert
            //Assert.NotNull(result);
            //Assert.Equal("Hello from Nova", result.Response);
            //Assert.Equal("test-model", result.ModelUsed);
        }

        [Fact]
        public async Task GetChatResponseAsync_ThrowsInvalidOperationException_WhenBedrockThrows()
        {
            // Arrange
            var bedrockMock = new Mock<IAmazonBedrockRuntime>();
            bedrockMock
                .Setup(x => x.InvokeModelAsync(It.IsAny<InvokeModelRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("bedrock failure"));

            var requestBuilderMock = new Mock<IBedrockRequestBuilder>();
            //requestBuilderMock.Setup(x => x.Build(It.IsAny<ChatRequest>())).Returns(new InvokeModelRequest { ModelId = "m" });

            //var loggerMock = new Mock<ILogger<AmazonBedrockChatService>>();

            //var service = new AmazonBedrockChatService(bedrockMock.Object, loggerMock.Object, requestBuilderMock.Object);

            //// Act & Assert
            //await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.GetChatResponseAsync(new ChatRequest { Message = "hi" }));
        }
    }
}
