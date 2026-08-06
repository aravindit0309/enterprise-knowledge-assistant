using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Application.Agents.Supervisor;
using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;
using EnterpriseKnowledgeAssistant.Application.Interfaces;
using EnterpriseKnowledgeAssistant.Domain.Entities;
using MediatR;

namespace EnterpriseKnowledgeAssistant.Application.Features.Chat.Commands.SendMessage
{
    public sealed class SendMessageCommandHandler
    {
        private readonly IChatService _chatService;
        private readonly IConversationRepository _conversationRepository;
        private readonly IAgentOrchestrator _agentOrchestrator;
        private readonly ISupervisorAgent _supervisorAgent;

        public SendMessageCommandHandler(IChatService chatService, IConversationRepository conversationRepository, IAgentOrchestrator agentOrchestrator,
            ISupervisorAgent supervisorAgent)
        {
            _chatService = chatService;
            _conversationRepository = conversationRepository;
            _agentOrchestrator = agentOrchestrator;
            _supervisorAgent = supervisorAgent;
        }

        public async Task<ChatResponse> HandleAsync( SendMessageCommand command, CancellationToken cancellationToken)
        {
            Conversation conversation;

            if(command.Request.ConversationId != null && command.Request.ConversationId != Guid.Empty)
            {
                conversation = await _conversationRepository.GetByIdAsync(command.Request.ConversationId.Value)
                        ?? throw new KeyNotFoundException($"Conversation {command.Request.ConversationId.Value} was not found.");
            }
            else
            {
                conversation = new Conversation();
            }

            // Persist the real user message in the conversation.
            conversation.AddUserMessage(command.Request.Message);

            // Create the request once and reuse it throughout the pipeline.
            var agentRequest = new AgentRequest(conversation.Id, command.Request.Message, conversation.Messages);

            // Let the supervisor create an execution plan.
            var executionPlan = await _supervisorAgent.CreatePlanAsync(agentRequest, cancellationToken);

            // Let the agent decide how to answer.
            var agentResult = await _agentOrchestrator.ExecuteAsync(executionPlan, agentRequest, cancellationToken);

            // Persist only the final assistant response.
            conversation.AddAssistantMessage(agentResult.Response);

            if (!command.Request.ConversationId.HasValue)
            {
                await _conversationRepository.AddAsync(conversation, cancellationToken);
            }

            await _conversationRepository.SaveChangesAsync(cancellationToken);

            return new ChatResponse
            {
                ConversationId = conversation.Id,
                Response = agentResult.Response,
                ModelUsed = agentResult.ModelUsed,

                Sources = agentResult.Sources
                    .Select(source => new ChatSource
                    {
                        DocumentId = source.DocumentId,
                        DocumentName = source.DocumentName,
                        ChunkIndex = source.ChunkIndex
                    }).ToList()
            };
        }
    }
}
