using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock;
using EnterpriseKnowledgeAssistant.Service;
using EnterpriseKnowledgeAssistant.Infrastructure;
using EnterpriseKnowledgeAssistant.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Register application services
builder.Services.AddScoped<IKnowledgeService, KnowledgeService>();
builder.Services.AddScoped<IChatService, AmazonBedrockChatService>();
builder.Services.Configure<BedrockOptions>(builder.Configuration.GetSection(BedrockOptions.SectionName));
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
