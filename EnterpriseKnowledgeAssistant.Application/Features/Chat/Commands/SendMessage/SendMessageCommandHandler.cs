using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
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
        private readonly IEmbeddingService _embeddingService;
        private readonly IDocumentChunkRepository _documentChunkRepository;

        public SendMessageCommandHandler(IChatService chatService, IConversationRepository conversationRepository,
            IEmbeddingService embeddingService, IDocumentChunkRepository documentChunkRepository)
        {
            _chatService = chatService;
            _conversationRepository = conversationRepository;
            _embeddingService = embeddingService;
            _documentChunkRepository = documentChunkRepository;
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

            // 1. Generate embedding for the user's question
            var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(command.Request.Message, cancellationToken);

            // 2. Retrieve relevant document chunks
            var relevantChunks =  await _documentChunkRepository.SearchSimilarAsync(queryEmbedding.ToArray(), 3,cancellationToken);
            
            //Building the source from which the content is generated
            var sources = relevantChunks
                .Select(chunk => new ChatSource
                {
                    DocumentId = chunk.DocumentId,
                    DocumentName = chunk.Document.FileName,
                    ChunkIndex = chunk.ChunkIndex
                })
                .ToList();

            // 3. Build temporary RAG context
            var knowledgeContext = string.Join( "\n\n---\n\n", relevantChunks.Select(chunk => chunk.Content));

            // 4. Persist the real user message
            conversation.AddUserMessage(command.Request.Message);

            // 5. Send conversation + temporary knowledge to Bedrock
            var response = await _chatService.SendAsync(conversation.Messages, knowledgeContext, cancellationToken);

            // 6. Persist only the assistant's actual response
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
                ModelUsed = response.ModelUsed,
                Sources = sources
            };
        }
    }
}
