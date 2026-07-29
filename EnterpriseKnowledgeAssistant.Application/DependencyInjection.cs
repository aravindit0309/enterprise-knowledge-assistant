using EnterpriseKnowledgeAssistant.Application.Features.Chat.Commands.SendMessage;
using EnterpriseKnowledgeAssistant.Application.Features.Documents.Commands.UploadDocument;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register MediatR
            // Register Validators
            // Register Application Services
            // (Nothing yet)
            services.AddScoped<SendMessageCommandHandler>();
            services.AddScoped<UploadDocumentCommandHandler>();

            return services;
        }
    }
}
