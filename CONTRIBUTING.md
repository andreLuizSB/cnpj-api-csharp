# Contribuindo para o CNPJ API

Obrigado pelo seu interesse em contribuir! 🎉

## Como Contribuir

### 🐛 Reportar Bugs

1. Verifique se o bug já não foi reportado nas [Issues](../../issues)
2. Crie uma nova issue com:
   - Título claro e descritivo
   - Passos para reproduzir
   - Comportamento esperado vs atual
   - Versão do .NET e sistema operacional

### 💡 Sugerir Melhorias

1. Abra uma issue com a tag `enhancement`
2. Descreva a funcionalidade desejada
3. Explique por que seria útil
4. Considere implementações alternativas

### 🔧 Pull Requests

1. **Fork** o projeto
2. **Clone** seu fork localmente
3. **Crie uma branch** para sua feature (`git checkout -b feature/nova-funcionalidade`)
4. **Faça suas alterações** seguindo as convenções do projeto
5. **Teste** suas alterações
6. **Commit** com mensagens claras (`git commit -m 'Adiciona nova funcionalidade'`)
7. **Push** para sua branch (`git push origin feature/nova-funcionalidade`)
8. **Abra um Pull Request**

## Padrões de Código

### C# / .NET

- Use **PascalCase** para métodos e propriedades públicas
- Use **camelCase** para variáveis locais
- Adicione **comentários XML** para APIs públicas
- Siga as **convenções do .NET**

### Commits

Use o formato: `tipo: descrição`

Tipos:
- `feat`: nova funcionalidade
- `fix`: correção de bug
- `docs`: documentação
- `style`: formatação
- `refactor`: refatoração
- `test`: testes
- `chore`: tarefas de manutenção

Exemplos:
```
feat: adiciona validação de CPF
fix: corrige parsing de datas no JSON
docs: atualiza README com novos endpoints
```

## Testes

- Execute `dotnet test` antes de submeter
- Adicione testes para novas funcionalidades
- Mantenha cobertura de testes alta

## Estrutura do Projeto

```
CnpjApiDotnet/
├── Program.cs              # Ponto de entrada da API
├── CnpjApi.csproj         # Configuração do projeto
├── Dockerfile             # Container Docker
├── test-api.ps1           # Scripts de teste
├── README.md              # Documentação principal
└── Properties/            # Configurações do projeto
```

## Ambiente de Desenvolvimento

### Pré-requisitos

- .NET 8.0 SDK
- Git
- Editor (VS Code, Visual Studio, etc.)

### Configuração

```bash
# Clone o projeto
git clone https://github.com/seu-usuario/cnpj-api-dotnet.git

# Navegue para o diretório
cd cnpj-api-dotnet

# Restore dependências
dotnet restore

# Execute a aplicação
dotnet run
```

### Testes

```bash
# Execute todos os testes
dotnet test

# Execute script de teste da API
.\test-api.ps1
```

## Processo de Review

1. **Automático**: GitHub Actions executa testes
2. **Manual**: Maintainers revisam o código
3. **Feedback**: Discussão e possíveis ajustes
4. **Merge**: Após aprovação

## Dúvidas?

- Abra uma [Discussion](../../discussions)
- Entre em contato via Issues
- Consulte a documentação no README

## Código de Conduta

- Seja respeitoso e profissional
- Aceite feedback construtivo
- Foque no problema, não na pessoa
- Ajude outros contribuidores

Obrigado por contribuir! 🚀
