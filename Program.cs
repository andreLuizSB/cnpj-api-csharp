using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Configuração de CORS para desenvolvimento
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configuração do pipeline de requisição
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

// Endpoint para consulta da razão social por CNPJ
app.MapGet("/cnpj/{cnpj}/razao-social", async (
    string cnpj, 
    HttpClient httpClient, 
    ILogger<Program> logger) =>
{
    try
    {
        // Validação básica do CNPJ
        if (string.IsNullOrWhiteSpace(cnpj))
        {
            logger.LogWarning("CNPJ não informado");
            return Results.BadRequest(new { erro = "CNPJ é obrigatório" });
        }

        // Remove caracteres especiais do CNPJ
        var cnpjLimpo = new string(cnpj.Where(char.IsDigit).ToArray());
        
        if (cnpjLimpo.Length != 14)
        {
            logger.LogWarning("CNPJ inválido: {CNPJ}", cnpj);
            return Results.BadRequest(new { erro = "CNPJ deve conter 14 dígitos" });
        }

        logger.LogInformation("Consultando CNPJ: {CNPJ}", cnpjLimpo);

        // Configuração do HttpClient com timeout e headers
        httpClient.Timeout = TimeSpan.FromSeconds(30);
        
        // Adiciona User-Agent se ainda não foi adicionado
        if (!httpClient.DefaultRequestHeaders.Contains("User-Agent"))
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "CNPJ-API/1.0");
        }
        
        // Chamada para a API do CNPJA (mesma URL do Python)
        var url = $"https://open.cnpja.com/office/{cnpjLimpo}";
        logger.LogInformation("Consultando API externa: {Url}", url);
        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                logger.LogWarning("CNPJ não encontrado: {CNPJ}", cnpjLimpo);
                return Results.NotFound(new { erro = "Empresa não encontrada" });
            }
            
            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                logger.LogWarning("Limite de taxa excedido na API externa para CNPJ: {CNPJ}", cnpjLimpo);
                return Results.Json(new { erro = "Limite de consultas excedido. Tente novamente em alguns minutos." }, statusCode: 429);
            }
            
            logger.LogError("API externa retornou status {StatusCode}", response.StatusCode);
            return Results.Problem("Erro ao consultar API externa");
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        
        // Log do JSON retornado para debug
        logger.LogDebug("JSON retornado da API: {JsonContent}", jsonContent);
        
        // Parse do JSON usando System.Text.Json
        using var document = JsonDocument.Parse(jsonContent);
        var root = document.RootElement;

        // Extração da razão social do campo company.name (como no Python)
        string? razaoSocial = null;
        
        if (root.TryGetProperty("company", out var companyElement) && 
            companyElement.TryGetProperty("name", out var nameElement))
        {
            razaoSocial = nameElement.GetString();
        }
        
        if (!string.IsNullOrWhiteSpace(razaoSocial))
        {
            logger.LogInformation("Razão social encontrada para CNPJ {CNPJ}: {RazaoSocial}", cnpjLimpo, razaoSocial);
            
            return Results.Ok(new 
            { 
                cnpj = cnpjLimpo,
                razao_social = razaoSocial,
                consultado_em = DateTime.UtcNow
            });
        }

        logger.LogWarning("Razão social não encontrada no retorno da API para CNPJ: {CNPJ}", cnpjLimpo);
        return Results.NotFound(new { erro = "Razão social não encontrada" });
    }
    catch (HttpRequestException ex)
    {
        logger.LogError(ex, "Erro ao consultar API externa: {Error}", ex.Message);
        return Results.Problem("Erro ao consultar API externa");
    }
    catch (TaskCanceledException ex)
    {
        logger.LogError(ex, "Timeout ao consultar CNPJ: {CNPJ}", cnpj);
        return Results.Problem("Timeout na consulta - tente novamente");
    }
    catch (JsonException ex)
    {
        logger.LogError(ex, "Erro ao processar resposta JSON para CNPJ: {CNPJ}", cnpj);
        return Results.Problem("Erro ao processar dados retornados");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro inesperado ao consultar CNPJ: {CNPJ}", cnpj);
        return Results.Problem("Erro interno do servidor");
    }
})
.WithName("GetRazaoSocial")
.WithSummary("Consulta a razão social de uma empresa pelo CNPJ")
.WithDescription("Endpoint que consulta a razão social de uma empresa utilizando a API pública do CNPJA")
.WithOpenApi();

// Endpoint de health check
app.MapGet("/health", () => Results.Ok(new { status = "OK", timestamp = DateTime.UtcNow }))
   .WithName("HealthCheck")
   .WithSummary("Verifica se a API está funcionando")
   .WithOpenApi();

app.Run();
