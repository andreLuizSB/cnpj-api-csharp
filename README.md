# CNPJ API

[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Docker](https://img.shields.io/badge/Docker-Ready-blue)](https://hub.docker.com/)
[![Azure](https://img.shields.io/badge/Azure-Compatible-blue)](https://azure.microsoft.com/)

API mínima em .NET 8 para consulta de razão social de empresas utilizando a API pública do CNPJA.

## 🎯 Funcionalidades

- ✅ Consulta razão social por CNPJ
- ✅ Validação de formato do CNPJ
- ✅ Tratamento de erros abrangente
- ✅ Logging estruturado
- ✅ Documentação Swagger/OpenAPI
- ✅ Health check endpoint
- ✅ Containerização com Docker
- ✅ Configuração para Azure

## Endpoints

### GET /cnpj/{cnpj}/razao-social
Consulta a razão social de uma empresa pelo CNPJ.

**URL da API externa**: `https://open.cnpja.com/office/{cnpj}`

**Parâmetros:**
- `cnpj`: CNPJ da empresa (pode conter ou não formatação)

**Exemplo de uso:**
```
GET /cnpj/11222333000181/razao-social
GET /cnpj/11.222.333/0001-81/razao-social
```

**Resposta de sucesso (200):**
```json
{
  "cnpj": "11222333000181",
  "razao_social": "CAIXA ESCOLAR DA ESCOLA ESTADUAL DE ENSINO FUNDAMENTAL JOSEFINA JACQUES NORONHA",
  "consultado_em": "2025-07-14T23:03:39.6426861Z"
}
```

**Possíveis erros:**
- `400 Bad Request`: CNPJ inválido ou mal formatado
- `404 Not Found`: CNPJ não encontrado na base de dados ou razão social não encontrada
- `429 Too Many Requests`: Limite de consultas excedido na API externa
- `500 Internal Server Error`: Erro interno do servidor

### GET /health
Endpoint de verificação de saúde da API.

**Resposta:**
```json
{
  "status": "OK",
  "timestamp": "2025-07-14T10:30:00Z"
}
```

## Como executar

### Desenvolvimento Local

1. Certifique-se de ter o .NET 8 SDK instalado
2. Navegue até o diretório do projeto
3. Execute o comando:
   ```bash
   dotnet run
   ```
4. Acesse a documentação Swagger em: `https://localhost:7000/swagger` (HTTPS) ou `http://localhost:5000/swagger` (HTTP)

### Testes Automatizados

Execute o script de testes PowerShell:
```powershell
.\test-api.ps1
```

### Docker

1. **Build da imagem:**
   ```bash
   docker build -t cnpj-api .
   ```

2. **Executar container:**
   ```bash
   docker run -p 8080:8080 cnpj-api
   ```

3. **Acessar a API:**
   - Swagger: `http://localhost:8080/swagger`
   - Health Check: `http://localhost:8080/health`

## Deploy no Azure

### Azure Container Apps (Recomendado)

1. **Build e push para Azure Container Registry:**
   ```bash
   az acr build --registry <registry-name> --image cnpj-api .
   ```

2. **Deploy para Container Apps:**
   ```bash
   az containerapp create \
     --name cnpj-api \
     --resource-group <resource-group> \
     --environment <environment-name> \
     --image <registry-name>.azurecr.io/cnpj-api \
     --target-port 8080 \
     --ingress external
   ```

### Azure App Service

1. **Deploy direto do código:**
   ```bash
   az webapp up --name <app-name> --resource-group <resource-group> --runtime "DOTNETCORE:8.0"
   ```

## Configurações

### Ambiente de Desenvolvimento
- Logging detalhado habilitado
- Swagger UI habilitado
- CORS configurado para desenvolvimento

### Ambiente de Produção
- Logging reduzido (apenas warnings e erros)
- Porta 8080 para containers
- Configuração otimizada para performance

## Limitações

- **API Externa**: A API do CNPJA possui limite de taxa. Em caso de erro 429, aguarde alguns minutos antes de tentar novamente.
- **Dados**: Os dados retornados dependem da disponibilidade na base do CNPJA.

## Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Minimal APIs
- System.Text.Json
- Swagger/OpenAPI
- HttpClient
- Docker

## Estrutura do Projeto

```
CnpjApiDotnet/
├── Program.cs                 # Arquivo principal da API
├── CnpjApi.csproj            # Configuração do projeto
├── Dockerfile                # Containerização
├── .dockerignore            # Arquivos ignorados no build
├── Properties/
│   └── launchSettings.json  # Configurações de inicialização
├── appsettings.json         # Configurações gerais
├── appsettings.Development.json  # Configurações de desenvolvimento
├── appsettings.Production.json   # Configurações de produção
├── test-api.ps1            # Script de testes
└── README.md               # Esta documentação
```

## Monitoramento e Logs

A API implementa logging estruturado com diferentes níveis:
- **Information**: Operações normais e consultas realizadas
- **Warning**: CNPJs inválidos e limites de taxa
- **Error**: Erros de conectividade e problemas internos

Para monitoramento em produção, recomenda-se integrar com:
- Azure Application Insights
- Azure Monitor
- Logs centralizados

## Segurança

- Validação rigorosa de entrada
- Timeout configurado para requisições externas
- Container executado com usuário não-root
- Logs não expostos informações sensíveis
- CORS configurado adequadamente para cada ambiente

## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor, leia [CONTRIBUTING.md](CONTRIBUTING.md) para detalhes sobre nosso código de conduta e processo de submissão de pull requests.

### Desenvolvimento Local

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-funcionalidade`)
3. Commit suas mudanças (`git commit -am 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/nova-funcionalidade`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

## 🆘 Suporte

- 📧 **Issues**: [GitHub Issues](../../issues)
- 💬 **Discussões**: [GitHub Discussions](../../discussions)
- 📖 **Documentação**: Consulte este README

## 🙏 Agradecimentos

- [CNPJA](https://cnpja.com/) pela API pública
- Comunidade .NET
- Contribuidores do projeto

---

⭐ **Gostou do projeto? Dê uma estrela!** ⭐
