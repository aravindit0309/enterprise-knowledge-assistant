namespace EnterpriseKnowledgeAssistant.Service.Models;

public record KnowledgeItem
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
}
