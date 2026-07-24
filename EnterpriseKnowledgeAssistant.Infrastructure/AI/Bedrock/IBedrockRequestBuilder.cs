namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock
{
    public interface IBedrockRequestBuilder
    {
        Amazon.BedrockRuntime.Model.InvokeModelRequest Build(IReadOnlyCollection<Domain.Entities.Message> messages);
    }
}
