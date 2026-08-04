using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.Tools;
using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Domain.Entities;
using EnterpriseKnowledgeAssistant.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Agents
{
    public sealed class BedrockAgentDecisionService : IAgentDecisionService
    {
        private readonly IChatService _chatService;
        private readonly ILogger<BedrockAgentDecisionService> _logger;

        public BedrockAgentDecisionService(IChatService chatService, ILogger<BedrockAgentDecisionService> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        public async Task<AgentDecision> DecideAsync(string userMessage, IReadOnlyCollection<Message> conversationMessages, IReadOnlyCollection<AgentToolDefinition> tools,
            CancellationToken cancellationToken = default)
        {
            var conversationHistory = string.Join(Environment.NewLine, conversationMessages
                .Take(Math.Max(0, conversationMessages.Count - 1)) //remove the current message since its coming in input
                .TakeLast(6).Select(message =>$"{message.Role}: {message.Content}"));

            var toolDescriptions = string.Join(Environment.NewLine, tools.Select(tool => $"- {tool.Name}: {tool.Description}"));

            var validToolNames = string.Join(Environment.NewLine, tools.Select(tool => $"- {tool.Name}"));

            var prompt = $$"""
You are an AI agent responsible for selecting the correct tool for the user's request.

Your responsibilities are:

1. Decide whether the user's request requires one of the available tools.
2. If a tool is required, rewrite the user's request into a clear, self-contained query that can be understood without conversation history.

Available tools:

{toolDescriptions}

Valid tool names:

{{validToolNames}}

Recent conversation:

{{conversationHistory}}

Current user message:

{{userMessage}}

Instructions:

- Use the recent conversation to resolve references such as:
  - it
  - that
  - they
  - this
  - those
  - the policy

- If the current message depends on previous conversation, rewrite it into a standalone query.

Examples

Conversation:
User: How many days can I work from home?
Assistant: Employees may work remotely up to three days per week.
User: Does that require manager approval?

Tool Input:
Does working remotely require manager approval?

---

Conversation:
User: Tell me about annual leave.
Assistant: Employees receive twenty days.
User: Can I carry it forward?

Tool Input:
Can annual leave be carried forward?

---

If no tool is required:

Return:

{
"requiresTool": false,
  "toolName": null,
  "toolInput": null
}

If a tool IS required:

- toolName MUST exactly match one of the valid tool names.
- Never invent a tool name.
- Never rename a tool.
- toolInput MUST be rewritten as a complete standalone query.

Return ONLY valid JSON.

Example:

{
   "requiresTool": true,
  "toolName": "search_knowledge_base",
  "toolInput": "Does working remotely require manager approval?"
}

Do not include markdown.

Do not include explanations.

Do not output anything except the JSON object.
""";

            // Adapt this line to however Message is constructed in your Domain.
            var messages = new List<Message>
            {
                // Use an explicit ConversationId (Empty is acceptable for transient requests)
                new Message(Guid.Empty, MessageRole.User, prompt)
            };

            var response = await _chatService.SendAsync(messages, null, cancellationToken);

            return ParseDecision(response.Response);
        }

        private AgentDecision ParseDecision(string response)
        {
            try
            {
                var cleanedResponse = CleanResponse(response);

                var decision = JsonSerializer.Deserialize<AgentDecision>(
                    cleanedResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (decision is not null)
                {
                    return ValidateDecision(decision);
                }

                _logger.LogWarning("Agent decision response was null. Falling back to direct chat.");
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex,"Unable to parse agent decision response. Falling back to direct chat.");
            }

            return new AgentDecision(
                RequiresTool: false,
                ToolName: null,
                ToolInput: null);
        }

        private AgentDecision ValidateDecision(AgentDecision decision)
        {
            if (!decision.RequiresTool)
            {
                return decision;
            }

            if (string.IsNullOrWhiteSpace(decision.ToolName))
            {
                _logger.LogWarning("Agent requested a tool but ToolName was empty. Falling back to direct chat.");

                return new AgentDecision(
                    RequiresTool: false,
                    ToolName: null,
                    ToolInput: null);
            }

            return decision;
        }

        private static string CleanResponse(string response)
        {
            return response
                .Replace("```json", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("```", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Trim();
        }
    }
}
