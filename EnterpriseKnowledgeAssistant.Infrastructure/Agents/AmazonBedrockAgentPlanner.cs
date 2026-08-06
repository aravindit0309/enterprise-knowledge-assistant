using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.Tools;
using EnterpriseKnowledgeAssistant.Application.Agents.Planning;
using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Domain.Entities;
using EnterpriseKnowledgeAssistant.Domain.Enums;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Planner;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Agents
{
    public sealed class AmazonBedrockAgentPlanner : IAgentPlanner
    {
        private readonly IChatService _chatService;
        private readonly ILogger<AmazonBedrockAgentPlanner> _logger;
        private readonly IPlannerPromptBuilder _promptBuilder;

        public AmazonBedrockAgentPlanner(IChatService chatService, ILogger<AmazonBedrockAgentPlanner> logger, IPlannerPromptBuilder promptBuilder)
        {
            _chatService = chatService;
            _logger = logger;
            _promptBuilder = promptBuilder;
        }

        public async Task<ExecutionPlan> PlanAsync(AgentRequest agentRequest, IReadOnlyCollection<AgentToolDefinition> tools,
            CancellationToken cancellationToken = default)
        {
            var prompt = _promptBuilder.Build(agentRequest, tools);

            var messages = new List<Message>
            {
                new(Guid.Empty, MessageRole.User, prompt)
            };

            var response = await _chatService.SendAsync(messages, null, cancellationToken);

            return PlannerMapper.ToExecutionPlan(response.Response);
        }
    }
}
