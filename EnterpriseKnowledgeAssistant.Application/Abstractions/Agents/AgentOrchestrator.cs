using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.Tools;
using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Agents
{
    public sealed class AgentOrchestrator : IAgentOrchestrator
    {
        private readonly IAgentDecisionService _decisionService;
        private readonly IReadOnlyCollection<IAgentTool> _tools;
        private readonly ILogger<AgentOrchestrator> _logger;
        private readonly IChatService _chatService;

        public AgentOrchestrator(IAgentDecisionService decisionService, IEnumerable<IAgentTool> tools, IChatService chatService,
            ILogger<AgentOrchestrator> logger)
        {
            _decisionService = decisionService;
            _tools = tools.ToList();
            _chatService = chatService;
            _logger = logger;
        }

        public async Task<AgentResult> ExecuteAsync(AgentRequest request, CancellationToken cancellationToken = default)
        {
            var totalStopwatch = Stopwatch.StartNew();

            _logger.LogInformation(
                """
            ======================================================
            Agent Request Started

            ConversationId : {ConversationId}

            User Message:
            {UserMessage}
            ======================================================
            """, request.ConversationId, request.UserMessage);

            var orderedMessages = request.Messages.OrderBy(m => m.CreatedAtUtc).ToList();

            var toolDefinitions = _tools.Select(tool => new AgentToolDefinition(tool.Name,tool.Description)).ToList();

            // -------------------------------
            // Agent Decision
            // -------------------------------

            var decisionStopwatch = Stopwatch.StartNew();
            AgentDecision decision;

            decision = await _decisionService.DecideAsync(request.UserMessage, orderedMessages, toolDefinitions, cancellationToken);
            

            decisionStopwatch.Stop();

            _logger.LogInformation(
                """
            Agent Decision

            Requires Tool : {RequiresTool}
            Tool          : {ToolName}
            Tool Input    : {ToolInput}

            Decision Time : {ElapsedMilliseconds} ms
            """, decision.RequiresTool, decision.ToolName ?? "None", decision.ToolInput ?? "N/A", decisionStopwatch.ElapsedMilliseconds);

            // -------------------------------
            // No Tool Required
            // -------------------------------

            if (!decision.RequiresTool)
            {
                _logger.LogInformation("No tool required. Generating direct response.");

                var generationStopwatch = Stopwatch.StartNew();

                var response = await _chatService.SendAsync(orderedMessages, null, cancellationToken);

                generationStopwatch.Stop();

                _logger.LogInformation("Direct response generated in {ElapsedMilliseconds} ms",generationStopwatch.ElapsedMilliseconds);

                totalStopwatch.Stop();

                _logger.LogInformation(
                    """
                ======================================================
                Agent Request Completed

                Total Duration : {ElapsedMilliseconds} ms
                ======================================================
                """, totalStopwatch.ElapsedMilliseconds);

                return new AgentResult(
                    response.Response,
                    response.ModelUsed,
                    Array.Empty<AgentSource>());
            }

            // -------------------------------
            // Locate Tool
            // -------------------------------

            var tool = _tools.FirstOrDefault(tool => string.Equals(tool.Name, decision.ToolName, StringComparison.OrdinalIgnoreCase));

            if (tool is null)
            {
                _logger.LogWarning(
                    """
                Agent selected an unknown tool.

                ToolName : {ToolName}

                Falling back to direct response.
                """, decision.ToolName);

                var generationStopwatch = Stopwatch.StartNew();

                var response = await _chatService.SendAsync(orderedMessages, null, cancellationToken);

                generationStopwatch.Stop();

                totalStopwatch.Stop();

                _logger.LogInformation(
                    """
                ======================================================
                Agent Request Completed

                Total Duration : {ElapsedMilliseconds} ms
                ======================================================
                """, totalStopwatch.ElapsedMilliseconds);

                return new AgentResult(
                    response.Response,
                    response.ModelUsed,
                    Array.Empty<AgentSource>());
            }

            // -------------------------------
            // Execute Tool
            // -------------------------------

            var toolInput = string.IsNullOrWhiteSpace(decision.ToolInput) ? request.UserMessage : decision.ToolInput;

            var toolStopwatch = Stopwatch.StartNew();

            var toolResult = await tool.ExecuteAsync(toolInput, cancellationToken);

            toolStopwatch.Stop();

            _logger.LogInformation(
                """
            Tool Execution

            Tool              : {ToolName}
            Success           : {Success}
            Sources Retrieved : {SourceCount}
            Duration          : {ElapsedMilliseconds} ms
            """,tool.Name, toolResult.Success, toolResult.Sources.Count, toolStopwatch.ElapsedMilliseconds);

            if (!toolResult.Success)
            {
                _logger.LogInformation("Knowledge search returned no matching documents.");
            }

            // -------------------------------
            // Final Grounded Response
            // -------------------------------

            var generationStopwatchGrounded = Stopwatch.StartNew();

            var groundedResponse = await _chatService.SendAsync(orderedMessages, toolResult.Content, cancellationToken);

            generationStopwatchGrounded.Stop();
             _logger.LogInformation("Grounded response generated in {ElapsedMilliseconds} ms", generationStopwatchGrounded.ElapsedMilliseconds);

            totalStopwatch.Stop();

            _logger.LogInformation(
                """
            ======================================================
            Agent Request Completed

            Total Duration : {ElapsedMilliseconds} ms
            ======================================================
            """,  totalStopwatch.ElapsedMilliseconds);

            return new AgentResult(
                groundedResponse.Response,
                groundedResponse.ModelUsed,
                toolResult.Sources);
        }
    }
}
