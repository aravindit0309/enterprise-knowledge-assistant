using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents;
using EnterpriseKnowledgeAssistant.Application.Abstractions.Agents.Tools;
using EnterpriseKnowledgeAssistant.Application.Agents.Supervisor;
using EnterpriseKnowledgeAssistant.Application.Features.Chat.Commands.SendMessage;
using EnterpriseKnowledgeAssistant.Application.Features.Documents.Commands.UploadDocument;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<IAgentOrchestrator, AgentOrchestrator>();
            services.AddScoped<IAgentTool, SearchKnowledgeBaseTool>();
            services.AddScoped<ISupervisorAgent, SupervisorAgent>();
            services.AddScoped<IAgentTool, StoreMemoryTool>();
            services.AddScoped<IAgentTool, SearchMemoryTool>();
            services.AddScoped<IAgentTool, ExecuteSqlTool>();
            services.AddScoped<IAgentTool, WebResearchTool>();

            return services;
        }
    }
}
