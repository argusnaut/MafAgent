# MafAgent

Um agente de IA em evolução para estudos sobre a biblioteca **Microsoft Agent Framework (MAF)**, desenvolvido com .NET.

Este repositório tem como objetivo principal explorar as capacidades de agendamento, raciocínio e integração com modelos de linguagem (LLMs) usando o ecossistema da Microsoft, servindo também como laboratório para testes práticos de arquitetura de agentes.

## Objetivo
- Estudar e implementar padrões de **AI Agents**.
- Explorar a biblioteca **MAF** (Microsoft Agent Framework) ou pacotes relacionados.
- Praticar desenvolvimento Fullstack com foco em backend inteligente (.NET).
- Manter um fluxo de trabalho seguro e profissional (User Secrets, CI/CD potencial).

## Tecnologias & Dependências
- **.NET 10**
- **C#**
- **Microsoft Agent Framework (MAF)**
- **OpenAI Client**
- **UserSecrets** para gestão segura de credenciais

## Segurança (Importante!)

⚠️ **NUNCA** commit chaves de API ou senhas diretamente no código ou no histórico do Git.

Para este projeto, as chaves de acesso (API Keys) são gerenciadas via **dotnet user-secrets**:

1.  **Iniciar o mecanismo de secrets:**
    ```bash
    dotnet user-secrets init
    ```

2.  **Adicionar a chave da OpenAI:**
    ```bash
    # Exemplo genérico (adicione a chave correta conforme sua configuração)
    dotnet user-secrets set "OpenAi:ApiKey" "sk-proj-..."
    ```

3.  **No código:**
    O sistema carrega essas chaves automaticamente via `IConfiguration` sem que elas apareçam nos arquivos de texto.

## Como Rodar

### Pré-requisitos
- .NET SDK instalado
- Chave de API configurada via `user-secrets` (passo acima)

### Passos
1.  Clone o repositório:
    ```bash
    git clone https://github.com/argusnaut/MafAgent.git
    cd MafAgent
    ```

2.  Configure as suas chaves (substituindo os valores reais):
    ```bash
    dotnet user-secrets set "OpenAiApiKey" "SUA_CHAVE_AQUI"
    ```

3.  Execute o projeto:
    ```bash
    dotnet run
    ```
