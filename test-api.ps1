# Scripts de teste para CNPJ API

## Teste do Health Check
Write-Host "=== Testando Health Check ===" -ForegroundColor Green
try {
    $healthResponse = Invoke-RestMethod -Uri "http://localhost:5000/health" -Method Get
    Write-Host "✅ Health Check OK: $($healthResponse.status)" -ForegroundColor Green
    Write-Host "Timestamp: $($healthResponse.timestamp)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro no Health Check: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== Testando Consulta CNPJ ===" -ForegroundColor Green

# CNPJs de teste (empresas conhecidas)
$cnpjsTeste = @(
    "11222333000181",  # CNPJ fictício
    "00000000000191",  # Banco do Brasil
    "33014556000104"   # Petrobras
)

foreach ($cnpj in $cnpjsTeste) {
    Write-Host "`n--- Testando CNPJ: $cnpj ---" -ForegroundColor Yellow
    
    try {
        $response = Invoke-RestMethod -Uri "http://localhost:5000/cnpj/$cnpj/razao-social" -Method Get
        
        Write-Host "✅ Sucesso!" -ForegroundColor Green
        Write-Host "CNPJ: $($response.cnpj)" -ForegroundColor White
        Write-Host "Razão Social: $($response.razao_social)" -ForegroundColor White
        Write-Host "Consultado em: $($response.consultado_em)" -ForegroundColor Gray
        
    } catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        
        switch ($statusCode) {
            400 { Write-Host "❌ CNPJ inválido (400)" -ForegroundColor Red }
            404 { Write-Host "⚠️  CNPJ não encontrado (404)" -ForegroundColor Yellow }
            429 { Write-Host "⚠️  Limite de consultas excedido (429) - Aguarde alguns minutos" -ForegroundColor Yellow }
            500 { Write-Host "❌ Erro interno do servidor (500)" -ForegroundColor Red }
            default { Write-Host "❌ Erro: $($_.Exception.Message)" -ForegroundColor Red }
        }
    }
}

Write-Host "`n=== Testando CNPJs com formato inválido ===" -ForegroundColor Green

$cnpjsInvalidos = @(
    "123",           # Muito curto
    "abcdefghij",    # Não numérico
    "",              # Vazio
    "12345678901234567890"  # Muito longo
)

foreach ($cnpj in $cnpjsInvalidos) {
    Write-Host "`n--- Testando CNPJ inválido: '$cnpj' ---" -ForegroundColor Yellow
    
    try {
        $response = Invoke-RestMethod -Uri "http://localhost:5000/cnpj/$cnpj/razao-social" -Method Get
        Write-Host "❌ Deveria ter retornado erro!" -ForegroundColor Red
    } catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        if ($statusCode -eq 400) {
            Write-Host "✅ Validação funcionando corretamente (400)" -ForegroundColor Green
        } else {
            Write-Host "⚠️  Status inesperado: $statusCode" -ForegroundColor Yellow
        }
    }
}

Write-Host "`n=== Teste concluído ===" -ForegroundColor Green
