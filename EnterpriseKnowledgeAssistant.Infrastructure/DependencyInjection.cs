using Amazon;
using Amazon.BedrockRuntime;
using Amazon.Extensions.NETCore.Setup;
using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddAWSService<IAmazonBedrockRuntime>();
            services.AddScoped<IChatService, AmazonBedrockChatService>();
            services.AddScoped<IBedrockRequestBuilder, NovaRequestBuilder>();

            return services;
        }
    }
}
