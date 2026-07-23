using Amazon;
using Amazon.BedrockRuntime;
using Amazon.Extensions.NETCore.Setup;
using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Application.Interfaces;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock;
using EnterpriseKnowledgeAssistant.Infrastructure.Persistence;
using EnterpriseKnowledgeAssistant.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EnterpriseKnowledgeAssistant.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
       this IServiceCollection services, IConfiguration configuration)
        {
            services
            .AddOptions<BedrockOptions>().Bind(configuration.GetSection("Bedrock")).ValidateDataAnnotations().ValidateOnStart();

            services.AddDefaultAWSOptions(new AWSOptions
            {
                Region = RegionEndpoint.GetBySystemName(
             configuration["Bedrock:Region"])
            });

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
                configuration.GetConnectionString("KnowledgeAssistantDb")));

            services.AddAWSService<IAmazonBedrockRuntime>();
            services.AddScoped<IChatService, AmazonBedrockChatService>();
            services.AddScoped<IBedrockRequestBuilder, NovaRequestBuilder>();
            services.AddScoped<IConversationRepository, ConversationRepository>();

            return services;
        }
    }
}
