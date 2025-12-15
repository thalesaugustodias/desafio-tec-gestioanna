# Exemplos de Requisições - API de Créditos Constituídos

## Usando cURL

### 1. Health Check - Self

```bash
curl -X GET http://localhost:5000/self
```

### 2. Health Check - Ready

```bash
curl -X GET http://localhost:5000/ready
```

### 3. Integrar Créditos Constituídos

```bash
curl -X POST http://localhost:5000/api/creditos/integrar-credito-constituido \
  -H "Content-Type: application/json" \
  -d '[
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
    },
    {
      "numeroCredito": "789012",
      "numeroNfse": "7891011",
      "dataConstituicao": "2024-02-26",
      "valorIssqn": 1200.50,
      "tipoCredito": "ISSQN",
      "simplesNacional": "Não",
      "aliquota": 4.5,
      "valorFaturado": 25000.00,
      "valorDeducao": 4000.00,
      "baseCalculo": 21000.00
    }
  ]'
```

### 4. Obter Créditos por NFS-e

```bash
curl -X GET http://localhost:5000/api/creditos/7891011
```

### 5. Obter Crédito por Número do Crédito

```bash
curl -X GET http://localhost:5000/api/creditos/credito/123456
```

---

## Usando PowerShell

### 1. Health Check - Self

```powershell
Invoke-RestMethod -Uri "http://localhost:5000/self" -Method Get
```

### 2. Integrar Créditos Constituídos

```powershell
$body = @(
    @{
        numeroCredito = "123456"
        numeroNfse = "7891011"
        dataConstituicao = "2024-02-25"
        valorIssqn = 1500.75
        tipoCredito = "ISSQN"
        simplesNacional = "Sim"
        aliquota = 5.0
        valorFaturado = 30000.00
        valorDeducao = 5000.00
        baseCalculo = 25000.00
    }
) | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/creditos/integrar-credito-constituido" `
    -Method Post `
    -Body $body `
    -ContentType "application/json"
```

### 3. Obter Créditos por NFS-e

```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/creditos/7891011" -Method Get
```

---

## Usando HTTP Client (VS Code REST Client Extension)

Crie um arquivo `requests.http`:

```http
### Health Check - Self
GET http://localhost:5000/self

### Health Check - Ready
GET http://localhost:5000/ready

### Integrar Créditos Constituídos
POST http://localhost:5000/api/creditos/integrar-credito-constituido
Content-Type: application/json

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
  },
  {
    "numeroCredito": "789012",
    "numeroNfse": "7891011",
    "dataConstituicao": "2024-02-26",
    "valorIssqn": 1200.50,
    "tipoCredito": "ISSQN",
    "simplesNacional": "Não",
    "aliquota": 4.5,
    "valorFaturado": 25000.00,
    "valorDeducao": 4000.00,
    "baseCalculo": 21000.00
  }
]

### Obter Créditos por NFS-e
GET http://localhost:5000/api/creditos/7891011

### Obter Crédito por Número
GET http://localhost:5000/api/creditos/credito/123456
```

---

## Testando o Fluxo Completo

### Passo 1: Verificar saúde do serviço

```bash
curl http://localhost:5000/self
curl http://localhost:5000/ready
```

### Passo 2: Integrar créditos (publica na fila)

```bash
curl -X POST http://localhost:5000/api/creditos/integrar-credito-constituido \
  -H "Content-Type: application/json" \
  -d '[{"numeroCredito": "TEST001", "numeroNfse": "NFS001", "dataConstituicao": "2024-02-25", "valorIssqn": 100.00, "tipoCredito": "ISSQN", "simplesNacional": "Sim", "aliquota": 5.0, "valorFaturado": 2000.00, "valorDeducao": 0.00, "baseCalculo": 2000.00}]'
```

### Passo 3: Aguardar processamento (background service processa em até 500ms)

```bash
sleep 1
```

### Passo 4: Consultar crédito inserido

```bash
curl http://localhost:5000/api/creditos/credito/TEST001
```

---

## Observações

- O Background Service processa mensagens a cada **500ms**
- Créditos duplicados (mesmo `numeroCredito`) não são inseridos novamente
- A API retorna `202 Accepted` para integrações (processamento assíncrono)
- Todos os logs são registrados no console da aplicação
