using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.Tools;
using EnterpriseKnowledgeAssistant.Application.Agents.Planning;
using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public sealed class AgentOrchestrator : IAgentOrchestrator
    {        
        private readonly IReadOnlyCollection<IAgentTool> _tools;
        private readonly ILogger<AgentOrchestrator> _logger;
        private readonly IChatService _chatService;

        public AgentOrchestrator( IEnumerable<IAgentTool> tools, IChatService chatService,
            ILogger<AgentOrchestrator> logger)
        {
            _tools = tools.ToList();
            _chatService = chatService;
            _logger = logger;
        }

        public async Task<AgentResult> ExecuteAsync(ExecutionPlan executionPlan, AgentRequest request, CancellationToken cancellationToken = default)
        {
            var totalStopwatch = Stopwatch.StartNew();

            var orderedMessages = request.Messages.OrderBy(m => m.CreatedAtUtc).ToList();

            var knowledgeContexts = new List<string>();
            var sources = new List<AgentSource>();
            
            foreach (var step in executionPlan.Steps.OrderBy(s => s.Order))
            {
                switch (step.Type)
                {
                    case ExecutionStepType.Retrieve:
                        {
                            if (string.IsNullOrWhiteSpace(step.ToolName))
                            {
                                continue;
                            }

                            var tool = _tools.FirstOrDefault(t =>
                                string.Equals(
                                    t.Name,
                                    step.ToolName,
                                    StringComparison.OrdinalIgnoreCase));

                            if (tool is null)
                            {
                                _logger.LogWarning("Planner requested unknown tool '{ToolName}'.",step.ToolName);
                                continue;
                            }

                            var toolInput = string.IsNullOrWhiteSpace(step.Input) ? request.UserMessage : step.Input;

                            var toolResult = await tool.ExecuteAsync(toolInput, cancellationToken);

                            knowledgeContexts.Add(toolResult.Content);
                            sources.AddRange(toolResult.Sources);

                            break;
                        }

                    case ExecutionStepType.Respond:
                        {
                            var combinedKnowledgeRespond = string.Join(Environment.NewLine + Environment.NewLine, knowledgeContexts);
                            var response = await _chatService.SendAsync(orderedMessages, combinedKnowledgeRespond, cancellationToken);
                            totalStopwatch.Stop();

                            return new AgentResult(response.Response, response.ModelUsed, sources);
                        }

                    default:
                        {
                            _logger.LogWarning("Unsupported execution step type '{StepType}'.",step.Type);
                            break;
                        }
                }
            }

            // Defensive fallback if planner forgot to add a Respond step.
            var combinedKnowledge = knowledgeContexts.Any() ? 
                string.Join(Environment.NewLine + Environment.NewLine, knowledgeContexts) : null;

            var fallbackResponse =  await _chatService.SendAsync(orderedMessages, combinedKnowledge, cancellationToken);

            totalStopwatch.Stop();

            return new AgentResult(fallbackResponse.Response, fallbackResponse.ModelUsed, sources);
        }
    }
}
