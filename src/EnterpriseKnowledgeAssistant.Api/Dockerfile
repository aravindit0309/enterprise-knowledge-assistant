FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ["src/EnterpriseKnowledgeAssistant.Api/EnterpriseKnowledgeAssistant.Api.csproj", "src/EnterpriseKnowledgeAssistant.Api/"]
COPY ["src/EnterpriseKnowledgeAssistant.Service/EnterpriseKnowledgeAssistant.Service.csproj", "src/EnterpriseKnowledgeAssistant.Service/"]
RUN dotnet restore "src/EnterpriseKnowledgeAssistant.Api/EnterpriseKnowledgeAssistant.Api.csproj"

# Copy everything and publish
COPY . .
WORKDIR /src/src/EnterpriseKnowledgeAssistant.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "EnterpriseKnowledgeAssistant.Api.dll"]
