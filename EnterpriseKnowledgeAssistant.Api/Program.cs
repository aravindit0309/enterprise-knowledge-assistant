using EnterpriseKnowledgeAssistant.Api.Middleware;
using EnterpriseKnowledgeAssistant.Application;
using EnterpriseKnowledgeAssistant.Application.Features.Chat;
using EnterpriseKnowledgeAssistant.Application.Features.Documents.Commands.UploadDocument;
using EnterpriseKnowledgeAssistant.Infrastructure;
using EnterpriseKnowledgeAssistant.Infrastructure.AI.Bedrock;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Register application services
builder.Services.AddScoped<IChatService, AmazonBedrockChatService>();
builder.Services.Configure<BedrockOptions>(builder.Configuration.GetSection(BedrockOptions.SectionName));
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(UploadDocumentCommand).Assembly);
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
