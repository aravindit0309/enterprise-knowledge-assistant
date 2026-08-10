using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Application.Features.Memory.StoreMemory;
using MediatR;

public sealed class StoreMemoryTool : IAgentTool
{
    private readonly ISender _sender;

    public StoreMemoryTool(ISender sender)
    {
        _sender = sender;
    }

    public string Name => AgentToolNames.StoreMemory;

    public string Description =>
        "Store important user information for future conversations.";

    public async Task<AgentToolResult> ExecuteAsync(
        string input,
        CancellationToken cancellationToken = default)
    {
        await _sender.Send(
            new StoreMemoryCommand(input),
            cancellationToken);

        return new AgentToolResult(
            true,
             "The requested information has been successfully stored as a long-term memory. Briefly acknowledge that you will remember it. Do not provide additional information about the stored subject.",
            []);
    }
}