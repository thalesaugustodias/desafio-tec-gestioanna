# desafio-tec-gestionna

This is the description of what the code block changes:
Criar documentação completa do projeto com todos os detalhes técnicos

This is the code block that represents the suggested code change:

# 🏦 API de Consulta de Créditos Constituídos

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?logo=docker)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

> Sistema de background service com APIs RESTful para consulta e integração de créditos constituídos, utilizando mensageria e processamento assíncrono.

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Arquitetura](#arquitetura)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Execução](#instalação-e-execução)
- [Endpoints da API](#endpoints-da-api)
- [Health Checks](#health-checks)
- [Testes](#testes)
- [Deploy](#deploy)
- [Documentação Técnica](#documentação-técnica)

---

## 🎯 Sobre o Projeto

Este projeto implementa um **microserviço de créditos constituídos** que oferece:

- ✅ **APIs RESTful** para consulta e integração de créditos
- ✅ **Background Service** que processa mensagens a cada 500ms
- ✅ **Mensageria** com Service Bus (implementação em memória para testes locais)
- ✅ **CQRS Pattern** usando MediatR
- ✅ **Clean Architecture** com separação em camadas
- ✅ **Health Checks** para monitoramento (/self e /ready)
- ✅ **Docker** para containerização
- ✅ **Testes Unitários** com alta cobertura

---

## 🏗️ Arquitetura

O projeto segue os princípios da **Clean Architecture** e **SOLID**, organizado em camadas:

```
┌─────────────────────────────────────────────────────────┐
│                     API Layer                           │
│  Controllers | Middlewares | Health Checks              │
└─────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────┐
│                  Application Layer                       │
│  Commands | Queries | Handlers | Services | Factories   │
│                   (CQRS + MediatR)                       │
└─────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────┐
│                   Domain Layer                           │
│  Entities | DTOs | Interfaces                            │
└─────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────┐
│                Infrastructure Layer                      │
│  Repositories | DbContext | Messaging | Migrations      │
└─────────────────────────────────────────────────────────┘
```

### Padrões de Projeto Implementados

- **CQRS (Command Query Responsibility Segregation)**: Separação de comandos e queries
- **Repository Pattern**: Abstração de acesso a dados
- **Factory Pattern**: Criação e mapeamento de objetos
- **Singleton Pattern**: Service Bus em memória
- **Mediator Pattern**: MediatR para desacoplamento
- **Dependency Injection**: Inversão de controle

---

## 🚀 Tecnologias Utilizadas

### Core
- **.NET 8.0** - Framework principal
- **C#** - Linguagem de programação
- **ASP.NET Core** - Web API

### Persistência
- **Entity Framework Core 8.0** - ORM
- **SQLite** - Banco de dados (facilita testes locais)
- **Migrations** - Versionamento de schema

### Mensageria
- **Service Bus In-Memory** - Implementação local para testes
- *(Em produção: Azure Service Bus ou Kafka)*

### Padrões e Bibliotecas
- **MediatR** - Implementação de CQRS
- **Swagger/OpenAPI** - Documentação interativa
- **Docker** - Containerização

### Testes
- **xUnit** - Framework de testes
- **Moq** - Mock objects
- **InMemory Database** - Testes de repositório

---

## 📁 Estrutura do Projeto

```
ConsultaDeCreditos/
├── 📂 ConsultaDeCreditos.API/
│   ├── Controllers/           # Endpoints REST
│   ├── HealthChecks/          # Self e Ready checks
│   ├── Middlewares/           # Exception handling e logging
│   ├── Program.cs             # Configuração da aplicação
│   └── Dockerfile             # Container configuration
│
├── 📂 ConsultaDeCreditos.Application/
│   ├── Commands/              # Commands CQRS
│   ├── Queries/               # Queries CQRS
│   ├── Handlers/              # Command/Query handlers
│   ├── Services/              # Background services
│   └── Factories/             # Object factories
│
├── 📂 ConsultaDeCreditos.Domain/
│   ├── Entidades/             # Domain entities
│   ├── DTOs/                  # Data Transfer Objects
│   └── Interfaces/            # Contratos (Repositórios e Mensageria)
│
├── 📂 ConsultaDeCreditos.Infrastructure/
│   ├── Persistencia/          # DbContext e configurações
│   ├── Repositorios/          # Implementação de repositórios
│   ├── Mensageria/            # Service Bus provider
│   └── Migrations/            # Migrations do EF Core
│
├── 📂 ConsultaDeCreditos.IoC/
│   └── DependencyInjection.cs # Registro de dependências
│
├── 📂 ConsultaDeCreditos.Tests/
│   ├── Factories/             # Testes de factories
│   ├── Handlers/              # Testes de handlers
│   └── Repositorios/          # Testes de repositórios
│
├── docker-compose.yml         # Orquestração de containers
└── README.md                  # Este arquivo
```

---

## ⚙️ Pré-requisitos

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (opcional, para execução em container)
- Editor de código ([Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/))

---

## 🔧 Instalação e Execução

### Opção 1: Execução Local (sem Docker)

1. **Clone o repositório**
```bash
git clone https://github.com/thalesaugustodias/desafio-tec-gestioanna.git
cd desafio-tec-gestionna
```

2. **Restaurar dependências**
```bash
dotnet restore
```

3. **Executar a aplicação**
```bash
cd ConsultaDeCreditos.API
dotnet run
```

4. **Acessar a API**
- Swagger UI: `http://localhost:7096` ou `https://localhost:5222`
- Health Check (Self): `http://localhost:5000/self`
- Health Check (Ready): `http://localhost:5000/ready`

### Opção 2: Execução com Docker

1. **Build e executar com docker-compose**
```bash
docker-compose up --build
```

2. **Acessar a API**
- API: `http://localhost:5000`
- Swagger: `http://localhost:5000`

### Opção 3: Apenas Docker (sem compose)

```bash
docker build -t consulta-creditos-api -f ConsultaDeCreditos.API/Dockerfile .
docker run -p 5000:8080 -e ASPNETCORE_ENVIRONMENT=Development consulta-creditos-api
```

---

## 📡 Endpoints da API

### 1. Integrar Créditos Constituídos

**POST** `/api/creditos/integrar-credito-constituido`

Integra uma lista de créditos constituídos, publicando cada um individualmente no tópico de mensageria.

**Request Body:**
```json
[
  {
    "numeroCredito": "123456",
    "numeroNfse": "7891011",
    "dataConstituicao": "2024-02-25",
    "valorIssqn": 1500.75,
    "tipoCredito": "ISSQN",
    "simplesNacional": "Sim",
    "aliquota": 5.0,
    "valorFaturado": 30000.00,
    "valorDeducao": 5000.00,
    "baseCalculo": 25000.00
  }
]
```

**Response:** `202 Accepted`
```json
{
  "success": true
}
```

---

### 2. Obter Créditos por NFS-e

**GET** `/api/creditos/{numeroNfse}`

Retorna todos os créditos associados a uma NFS-e específica.

**Exemplo:** `GET /api/creditos/7891011`

**Response:** `200 OK`
```json
[
  {
    "numeroCredito": "123456",
    "numeroNfse": "7891011",
    "dataConstituicao": "2024-02-25",
    "valorIssqn": 1500.75,
    "tipoCredito": "ISSQN",
    "simplesNacional": "Sim",
    "aliquota": 5.0,
    "valorFaturado": 30000.00,
    "valorDeducao": 5000.00,
    "baseCalculo": 25000.00
  }
]
```

---

### 3. Obter Crédito por Número

**GET** `/api/creditos/credito/{numeroCredito}`

Retorna os detalhes de um crédito específico.

**Exemplo:** `GET /api/creditos/credito/123456`

**Response:** `200 OK`
```json
{
  "numeroCredito": "123456",
  "numeroNfse": "7891011",
  "dataConstituicao": "2024-02-25",
  "valorIssqn": 1500.75,
  "tipoCredito": "ISSQN",
  "simplesNacional": "Sim",
  "aliquota": 5.0,
  "valorFaturado": 30000.00,
  "valorDeducao": 5000.00,
  "baseCalculo": 25000.00
}
```

---

## 🏥 Health Checks

### Self Check
**GET** `/self`

Verifica se o serviço está ativo e respondendo.

**Response:** `200 OK`

### Ready Check
**GET** `/ready`

Verifica se o serviço está pronto para receber requisições (valida banco de dados e dependências).

**Response:** `200 OK`

---

## 🧪 Testes

O projeto possui testes unitários cobrindo as principais funcionalidades.

### Executar todos os testes
```bash
dotnet test
```

### Executar com cobertura
```bash
dotnet test /p:CollectCoverage=true
```

### Estrutura de Testes

- ✅ **Factory Tests**: Testa mapeamento entre DTOs e Entidades
- ✅ **Handler Tests**: Testa Commands e Queries handlers
- ✅ **Repository Tests**: Testa operações de banco de dados

---

## 📖 Documentação Técnica

### Background Service

O `ProcessadorMensagensBackgroundService` executa continuamente:

1. ⏱️ Verifica a cada **500ms** se existem mensagens no tópico
2. 📥 Consome mensagens uma a uma (não em bulk)
3. ✔️ Valida se o crédito já existe (evita duplicação)
4. 💾 Insere na base de dados de forma individual
5. 📝 Registra logs de todas as operações

### Fluxo de Integração

```
Cliente → POST /integrar-credito-constituido
           ↓
    [Command Handler] → Publica mensagens individuais
           ↓
    [Service Bus Topic: integrar-credito-constituido-entry]
           ↓
    [Background Service] → Consome a cada 500ms
           ↓
    [Validação de Duplicação]
           ↓
    [Repositório] → Insere no banco
           ↓
    [SQLite Database]
```

### Modelo de Dados

```sql
CREATE TABLE credito (
    id                BIGINT PRIMARY KEY,
    numero_credito    VARCHAR(50)    NOT NULL UNIQUE,
    numero_nfse       VARCHAR(50)    NOT NULL,
    data_constituicao DATE           NOT NULL,
    valor_issqn       DECIMAL(15, 2) NOT NULL,
    tipo_credito      VARCHAR(50)    NOT NULL,
    simples_nacional  BOOLEAN        NOT NULL,
    aliquota          DECIMAL(5, 2)  NOT NULL,
    valor_faturado    DECIMAL(15, 2) NOT NULL,
    valor_deducao     DECIMAL(15, 2) NOT NULL,
    base_calculo      DECIMAL(15, 2) NOT NULL,
    data_criacao      DATETIME       NOT NULL
);

CREATE INDEX IX_credito_numero_nfse ON credito(numero_nfse);
```

### Middlewares

1. **ExceptionHandlingMiddleware**: Captura exceções globais e retorna respostas padronizadas
2. **RequestLoggingMiddleware**: Registra todas as requisições HTTP com duração

### Princípios SOLID Aplicados

- **S** - Single Responsibility: Cada classe tem uma única responsabilidade
- **O** - Open/Closed: Extensível via interfaces e DI
- **L** - Liskov Substitution: Interfaces bem definidas
- **I** - Interface Segregation: Interfaces específicas e coesas
- **D** - Dependency Inversion: Dependências via abstrações (interfaces)

---

## 📝 Notas de Produção

### Mensageria

A implementação atual utiliza **Service Bus em memória** para facilitar testes locais. Para produção, recomenda-se:

- **Azure Service Bus**: Para ambientes Azure
- **Apache Kafka**: Para alta throughput
- **RabbitMQ**: Para mensageria robusta

### Banco de Dados

O projeto usa **PostgreSQL** para facilitar o desenvolvimento. Para produção, migre para:

- **PostgreSQL** (recomendado no desafio)
- **MariaDB** (recomendado no desafio)

**Mudança de Provider:**
```csharp
// No arquivo DependencyInjection.cs
services.AddDbContext<ConsultaCreditosDbContext>(options =>
    options.UseNpgsql(connectionString)); // PostgreSQL
```

---

## 👨‍💻 Autor

**Thales Augusto Dias**

- GitHub: [@thalesaugustodias](https://github.com/thalesaugustodias)

---

## 🙏 Agradecimentos

Desafio técnico desenvolvido demonstrando conhecimentos em:
- Arquitetura de Software (Clean Architecture)
- Clean Code & SOLID Principles
- Design Patterns (CQRS, Repository, Factory, Singleton)
- .NET 8 & Entity Framework Core
- Mensageria & Background Services
- Testes Unitários
- DevOps (Docker & Docker Compose)

---