using EnterpriseKnowledgeAssistant.Application.Features.Chat.Models;
using EnterpriseKnowledgeAssistant.Application.Interfaces;
using EnterpriseKnowledgeAssistant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Features.Chat.Commands.SendMessage
{
    public sealed class SendMessageCommandHandler
    {
        private readonly IChatService _chatService;
        private readonly IConversationRepository _conversationRepository;

        public SendMessageCommandHandler(IChatService chatService, IConversationRepository conversationRepository)
        {
            _chatService = chatService;
            _conversationRepository = conversationRepository;
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

            conversation.AddUserMessage(command.Request.Message);

            var response = await _chatService.SendAsync(conversation.Messages, cancellationToken);

            conversation.AddAssistantMessage(response.Response);

            
            if (!command.Request.ConversationId.HasValue)
            {
                await _conversationRepository.AddAsync(conversation, cancellationToken);
            }

            await _conversationRepository.SaveChangesAsync(cancellationToken);

            return new ChatResponse
            {
                ConversationId = conversation.Id,
                Response = response.Response,
                ModelUsed = response.ModelUsed
            };
        }
    }
}
