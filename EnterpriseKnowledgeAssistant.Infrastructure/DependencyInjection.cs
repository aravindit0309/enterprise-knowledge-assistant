using Amazon;
using Amazon.BedrockRuntime;
using Amazon.Extensions.NETCore.Setup;
using EnterpriseKnowledgeAssistant.Application.Common.Interfaces;
using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Application.Interfaces;
using EnterpriseKnowledgeAssistant.Domain.Documents;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock;
using EnterpriseKnowledgeAssistant.Infrastructure.Persistence;
using EnterpriseKnowledgeAssistant.Infrastructure.Persistence.Repositories;
using EnterpriseKnowledgeAssistant.Infrastructure.Storage;
using EnterpriseKnowledgeAssistant.Infrastructure.TextChunking;
using EnterpriseKnowledgeAssistant.Infrastructure.TextExtraction;
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
                configuration.GetConnectionString("KnowledgeAssistantDb"),
                npgsqlOptions => npgsqlOptions.UseVector()));

            services.AddAWSService<IAmazonBedrockRuntime>();
            services.AddScoped<IChatService, AmazonBedrockChatService>();
            services.AddScoped<IBedrockRequestBuilder, NovaRequestBuilder>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            
            services.AddScoped<ITextExtractorResolver, TextExtractorResolver>();
            services.AddScoped<ITextExtractor, TextExtractor>();
            services.AddScoped<ITextExtractor, PdfTextExtractor>();
            services.AddScoped<ITextExtractor, DocxTextExtractor>();

            services.AddScoped<ITextChunker, TextChunker>();
            services.AddScoped<IDocumentChunkRepository, DocumentChunkRepository>();

            services.AddScoped<IEmbeddingService, AmazonBedrockEmbeddingService>();

            services.Configure<StorageOptions>(configuration.GetSection(StorageOptions.SectionName));

            return services;
        }
    }
}
