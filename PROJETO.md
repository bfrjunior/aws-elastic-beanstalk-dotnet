# 🛒 Products API — AWS Elastic Beanstalk + RDS + .NET 8

CRUD de produtos serverless construído com .NET 8 Minimal API, deployado no AWS Elastic Beanstalk com banco de dados PostgreSQL no Amazon RDS.

---

## 🎯 O que vamos construir

API REST com CRUD completo de produtos.

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/products` | Lista todos os produtos |
| `GET` | `/products/{id}` | Busca produto por ID |
| `POST` | `/products` | Cria um novo produto |
| `PUT` | `/products/{id}` | Atualiza um produto |
| `DELETE` | `/products/{id}` | Remove um produto |

---

## 🏗️ Arquitetura

```
Cliente (Postman / Browser)
          │
          ▼
  Elastic Beanstalk (Load Balancer / Nginx)
          │
          ▼
  EC2 Instance (.NET 8 Web API)
          │
          ▼
  Amazon RDS (PostgreSQL)
```

---

## 🛠️ Stack

| Tecnologia | Uso |
|------------|-----|
| .NET 8 Minimal API | Framework da aplicação |
| AWS Elastic Beanstalk | Plataforma de deploy (EC2 + RDS + CloudWatch) |
| Amazon RDS PostgreSQL | Banco de dados |
| Entity Framework Core 8 | ORM |
| Npgsql | Driver PostgreSQL |
| Swashbuckle | Swagger UI |

---

## 📦 Entidade: Product

```json
{
  "id": "uuid",
  "name": "Teclado Mecânico",
  "price": 299.90,
  "stock": 15
}
```

---

## 📁 Estrutura do Projeto

```
ebs-products-api/
├── PROJETO.md
├── README.md
├── docker-compose.yml
└── src/
    └── ProductsApi/
        ├── ProductsApi.csproj
        ├── Program.cs
        ├── Data/
        │   └── AppDbContext.cs
        └── Models/
            └── Product.cs
```

---

## ☁️ Infraestrutura AWS (Elastic Beanstalk provisiona tudo)

| Recurso | Detalhe | Status |
|---------|---------|--------|
| Elastic Beanstalk Environment | Web server, .NET 8 on Linux | ⬜ |
| EC2 Instance | t2.small, single instance | ⬜ |
| RDS PostgreSQL | t3.small, 10GB | ⬜ |
| CloudWatch | Log streaming ativado | ⬜ |
| IAM Roles | EBS service role + EC2 instance role | ⬜ |
| Nginx | Proxy reverso (padrão do EBS) | ⬜ |

> ⬜ pendente · 🔄 em andamento · ✅ concluído

---

## 🚀 Como rodar localmente

**Pré-requisitos:** .NET 8 SDK, Docker

```bash
# Subir banco local
docker compose up -d

# Rodar migrations
dotnet tool run dotnet-ef database update

# Rodar aplicação
dotnet run
```

Acesse o Swagger em `http://localhost:5000/swagger`.

---

## 📦 Deploy no Elastic Beanstalk

O deploy é feito via **publish to AWS** no Rider/Visual Studio com o AWS Toolkit instalado.

A connection string do RDS é configurada via variável de ambiente no Elastic Beanstalk:

```
ConnectionStrings__Default = Host=<rds-endpoint>;Port=5432;Database=productsdb;Username=postgres;Password=<senha>
```

---

## 🔑 Diferença para o Vídeo 1 (Lambda)

| | Lambda (vídeo 1) | Elastic Beanstalk (vídeo 2) |
|--|--|--|
| Compute | Serverless (função) | EC2 (servidor sempre ligado) |
| Escala | Automática | Configurável |
| Custo | Pay per request | Pay per hour (EC2) |
| Setup | Manual (CLI) | Automático (wizard) |
| Ideal para | APIs esporádicas | APIs com tráfego contínuo |

---

## 📝 Aprendizados

- Como o Elastic Beanstalk provisiona EC2 + RDS + CloudWatch automaticamente
- Diferença entre deploy serverless (Lambda) e deploy em servidor (EC2)
- Como configurar IAM roles para o Beanstalk e EC2
- Como passar connection string via variável de ambiente no EBS
- Como fazer deploy via AWS Toolkit no Rider/Visual Studio
