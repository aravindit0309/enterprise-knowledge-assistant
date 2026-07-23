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
            var conversation = new Conversation();

            conversation.AddUserMessage(command.Request.Message);

            //await _conversationRepository.AddAsync( conversation, cancellationToken);

            //await _conversationRepository.SaveChangesAsync(cancellationToken);

            var response = await _chatService.SendAsync(command.Request,cancellationToken);

            conversation.AddAssistantMessage(response.Response);

            await _conversationRepository.AddAsync(conversation, cancellationToken); // TODO added temporarily

            await _conversationRepository.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
