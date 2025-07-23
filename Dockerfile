# Usando a imagem base oficial do .NET 8 runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Configurar variáveis de ambiente para produção
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Usando a imagem SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./CnpjApi.csproj", "./"]
RUN dotnet restore "./CnpjApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CnpjApi.csproj" -c Release -o /app/build

# Publicando a aplicação
FROM build AS publish
RUN dotnet publish "CnpjApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Configuração final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Criando usuário não-root para segurança
RUN adduser --disabled-password --gecos "" --uid 1000 appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "CnpjApi.dll"]
