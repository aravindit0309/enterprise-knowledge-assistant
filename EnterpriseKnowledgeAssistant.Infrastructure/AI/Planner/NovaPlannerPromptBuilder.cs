using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Planner;

public sealed class NovaPlannerPromptBuilder : IPlannerPromptBuilder
{
    public string Build(AgentRequest request, IReadOnlyCollection<AgentToolDefinition> tools)
    {
        var conversationHistory = string.Join(
            Environment.NewLine,
            request.Messages
                .Take(Math.Max(0, request.Messages.Count - 1))
                .TakeLast(6)
                .Select(m => $"{m.Role}: {m.Content}"));

        var toolDescriptions = string.Join(
            Environment.NewLine,
            tools.Select(t => $"- {t.Name}: {t.Description}"));

        var validToolNames = string.Join(
            Environment.NewLine,
            tools.Select(t => $"- {t.Name}"));

        var builder = new StringBuilder();

        builder.AppendLine("""
You are an AI Planning Agent.

Your job is NOT to answer the user's question.

Your job is to generate an execution plan that another AI system will execute.

Return ONLY valid JSON.

Never explain your reasoning.

Never answer the user directly.

--------------------------------------------------
Responsibilities
--------------------------------------------------

1. Understand the user's intent.

2. Use the conversation history to resolve references such as:
   - it
   - that
   - they
   - this
   - those
   - the policy

3. Rewrite incomplete questions into standalone queries.

4. Decide whether enterprise knowledge retrieval is required.

5. Generate an execution plan.

6. Return only JSON.

--------------------------------------------------
Available Tools
--------------------------------------------------
""");

        builder.AppendLine(toolDescriptions);

        builder.AppendLine("""

--------------------------------------------------
Valid Tool Names
--------------------------------------------------
""");

        builder.AppendLine(validToolNames);

        builder.AppendLine("""

--------------------------------------------------
Recent Conversation
--------------------------------------------------
""");

        builder.AppendLine(conversationHistory);

        builder.AppendLine("""

--------------------------------------------------
Current User Message
--------------------------------------------------
""");

        builder.AppendLine(request.UserMessage);

        builder.AppendLine("""

--------------------------------------------------
Output Schema
--------------------------------------------------

Return JSON using exactly this schema.

{
  "steps": [
    {
      "order": 1,
      "type": "Retrieve",
      "tool": "{knowledgeBaseTool}",
      "input": "Standalone rewritten query"
    },
    {
      "order": 2,
      "type": "Respond"
    }
  ]
}
""");

        builder.AppendLine("""

--------------------------------------------------
Rules
--------------------------------------------------

- If enterprise knowledge retrieval is required:
    - The first step MUST be Retrieve.
    - Tool MUST exactly match one of the valid tool names.
    - Rewrite the user's request into a complete standalone query.
    - Never invent tool names.

- If enterprise knowledge retrieval is NOT required:
    - Return only one step:

      {
        "order":1,
        "type":"Respond"
      }

- Never answer the user's question.

- Never include markdown.

- Never include explanations.

- Return ONLY valid JSON.
""");

        builder.AppendLine("""

--------------------------------------------------
Example 1
--------------------------------------------------

Conversation

User: How many days can I work from home?
Assistant: Employees may work remotely up to three days per week.
User: Does that require manager approval?

Output

{
  "steps": [
    {
      "order":1,
      "type":"Retrieve",
      "tool":"{knowledgeBaseTool}",
      "input":"Does working remotely require manager approval?"
    },
    {
      "order":2,
      "type":"Respond"
    }
  ]
}
""");

        builder.AppendLine("""

--------------------------------------------------
Example 2
--------------------------------------------------

Conversation

User: Tell me about annual leave.
Assistant: Employees receive twenty days.
User: Can I carry it forward?

Output

{
  "steps": [
    {
      "order":1,
      "type":"Retrieve",
      "tool":"{knowledgeBaseTool}",
      "input":"Can annual leave be carried forward?"
    },
    {
      "order":2,
      "type":"Respond"
    }
  ]
}
""");

        builder.AppendLine("""

--------------------------------------------------
Example 3
--------------------------------------------------

User: Say hello

Output

{
  "steps": [
    {
      "order":1,
      "type":"Respond"
    }
  ]
}
""");

        builder.AppendLine("""

--------------------------------------------------
Example 4
--------------------------------------------------

User: Compare the leave policy and travel policy.

Output

{
  "steps": [
    {
      "order":1,
      "type":"Retrieve",
      "tool":"{knowledgeBaseTool}",
      "input":"Leave policy"
    },
    {
      "order":2,
      "type":"Retrieve",
      "tool":"{knowledgeBaseTool}",
      "input":"Travel policy"
    },
    {
      "order":3,
      "type":"Respond"
    }
  ]
}
""");

        builder.AppendLine("""

Remember:

- Your responsibility is to create an execution plan.
- Never answer the user's question.
- Return ONLY valid JSON.
- Never include markdown.
""");

        return builder.ToString();
    }
}