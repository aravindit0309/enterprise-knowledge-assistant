using Amazon;
using Amazon.BedrockRuntime;
using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseKnowledgeAssistant.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
       this IServiceCollection services,
       IConfiguration configuration)
        {
            services.Configure<BedrockOptions>(
                configuration.GetSection(BedrockOptions.SectionName));

            var options = configuration
                .GetSection(BedrockOptions.SectionName).Get<BedrockOptions>()!;

            services.AddSingleton<IAmazonBedrockRuntime>(_ =>
                new AmazonBedrockRuntimeClient(
                    RegionEndpoint.GetBySystemName(options.Region)));

            services.AddScoped<IChatService, AmazonBedrockChatService>();

            return services;
        }
    }
}
