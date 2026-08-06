using EnterpriseKnowledgeAssistant.Application.Agents.Planning;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Models;
using System.Text.Json;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI.Planner;

public static class PlannerMapper
{
    public static ExecutionPlan ToExecutionPlan(string plannerResponse)
    {
        try
        {
            var cleanedResponse = CleanResponse(plannerResponse);

            var dto = JsonSerializer.Deserialize<PlannerResponseDto>(
                cleanedResponse,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (dto is null)
            {
                return CreateFallbackPlan();
            }

            return ToExecutionPlan(dto);
        }
        catch (JsonException)
        {
            return CreateFallbackPlan();
        }
    }

    private static ExecutionPlan ToExecutionPlan(PlannerResponseDto dto)
    {
        var plan = new ExecutionPlan();

        foreach (var step in dto.Steps.OrderBy(s => s.Order))
        {
            plan.Steps.Add(new ExecutionStep
            {
                Order = step.Order,
                Type = ParseStepType(step.Type),
                ToolName = step.Tool,
                Input = step.Input,
                Description = step.Description
            });
        }

        return plan;
    }

    private static ExecutionStepType ParseStepType(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "retrieve" => ExecutionStepType.Retrieve,
            "respond" => ExecutionStepType.Respond,
            _ => throw new InvalidOperationException(
                $"Unknown execution step type '{value}'.")
        };
    }

    private static string CleanResponse(string response)
    {
        return response
            .Replace("```json", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("```", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Trim();
    }

    private static ExecutionPlan CreateFallbackPlan()
    {
        var plan = new ExecutionPlan();

        plan.Steps.Add(new ExecutionStep
        {
            Order = 1,
            Type = ExecutionStepType.Respond
        });

        return plan;
    }
}