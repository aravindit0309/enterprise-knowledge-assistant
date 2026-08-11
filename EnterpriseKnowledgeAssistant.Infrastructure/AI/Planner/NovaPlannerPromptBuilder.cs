using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Planner;

public sealed class NovaPlannerPromptBuilder : IPlannerPromptBuilder
{
    private const string PromptFileName = "PlannerPrompt.txt";

    public string Build(
        AgentRequest request,
        IReadOnlyCollection<AgentToolDefinition> tools)
    {
        var conversationHistory = string.Join(Environment.NewLine, request.Messages
                .Take(Math.Max(0, request.Messages.Count - 1))
                .TakeLast(6).Select(m => $"{m.Role}: {m.Content}"));

        var toolDescriptions = string.Join(Environment.NewLine, tools.Select(t => $"- {t.Name}: {t.Description}"));

        var validToolNames = string.Join(Environment.NewLine,tools.Select(t => $"- {t.Name}"));        

        var promptPath = Path.Combine(AppContext.BaseDirectory, "AI", "Planner", "Prompts", PromptFileName);

        if (!File.Exists(promptPath))
        {
            throw new FileNotFoundException($"Planner prompt file was not found: {promptPath}", promptPath);
        }

        var template = File.ReadAllText(promptPath);

        return template
            .Replace("{{TOOL_DESCRIPTIONS}}", toolDescriptions)
            .Replace("{{VALID_TOOL_NAMES}}", validToolNames)
            .Replace("{{CONVERSATION_HISTORY}}", conversationHistory)
            .Replace("{{USER_MESSAGE}}", request.UserMessage);
    }
}