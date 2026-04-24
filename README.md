# 🛒 Products API — AWS Elastic Beanstalk + RDS + .NET 8

CRUD de produtos construído com .NET 8 Minimal API, deployado no AWS Elastic Beanstalk com banco de dados PostgreSQL no Amazon RDS.

---

## ✨ Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/products` | Lista todos os produtos |
| `GET` | `/products/{id}` | Busca produto por ID |
| `POST` | `/products` | Cria um novo produto |
| `PUT` | `/products/{id}` | Atualiza um produto |
| `DELETE` | `/products/{id}` | Remove um produto |

### Exemplo

```bash
# Criar produto
curl -X POST "http://<ebs-url>/products" \
  -H "Content-Type: application/json" \
  -d '{"name":"Teclado Mecânico","price":299.90,"stock":15}'

# Listar produtos
curl "http://<ebs-url>/products"

# Atualizar produto
curl -X PUT "http://<ebs-url>/products/{id}" \
  -H "Content-Type: application/json" \
  -d '{"name":"Teclado Mecânico RGB","price":349.90,"stock":10}'

# Deletar produto
curl -X DELETE "http://<ebs-url>/products/{id}"
```

---

## 🏗️ Arquitetura

```
Cliente (Postman / Browser)
          │
          ▼
  Elastic Beanstalk (Nginx)
          │
          ▼
  EC2 t3.micro (.NET 8 Web API)
          │
          ▼
  Amazon RDS (PostgreSQL)
```

---

## 🛠️ Stack

| Tecnologia | Uso |
|------------|-----|
| .NET 8 Minimal API | Framework da aplicação |
| AWS Elastic Beanstalk | Plataforma de deploy (EC2 + Nginx + CloudWatch) |
| Amazon RDS PostgreSQL | Banco de dados |
| Entity Framework Core 8 | ORM + Migrations |
| Npgsql 8 | Driver PostgreSQL |
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

## 📁 Estrutura

```
ebs-products-api/
├── README.md
├── PROJETO.md
├── docker-compose.yml
└── src/
    └── ProductsApi/
        ├── Program.cs
        ├── appsettings.json
        ├── Data/
        │   └── AppDbContext.cs
        └── Models/
            └── Product.cs
```

---

## ☁️ Infraestrutura AWS

| Recurso | Detalhe |
|---------|---------|
| Elastic Beanstalk | .NET 8 on Linux, single instance |
| EC2 | t3.micro |
| RDS | PostgreSQL 17, db.t4g.micro (compartilhado) |
| Security Groups | EC2 do EBS com acesso à porta 5432 do RDS |
| IAM Roles | Service role + EC2 instance profile |
| Nginx | Proxy reverso (porta 80 → 5000) |
| CloudWatch | Log streaming ativado |
| Região | `us-east-1` |

---

## 🚀 Como rodar localmente

**Pré-requisitos:** .NET 8 SDK, Docker

```bash
# Subir banco local
docker compose up -d

# Rodar migrations
dotnet tool run dotnet-ef database update

# Rodar aplicação
ASPNETCORE_ENVIRONMENT=Development dotnet run
```

Acesse o Swagger em `http://localhost:5075/swagger`.

---

## 📦 Deploy no Elastic Beanstalk

```bash
# Publicar e zipar
dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish
cd publish && zip -r ../deploy.zip . && cd ..

# Subir para S3
aws s3 cp deploy.zip s3://<bucket>/deploy-vX.zip --region us-east-1

# Criar versão
aws elasticbeanstalk create-application-version \
  --application-name products-api \
  --version-label vX.X.X \
  --source-bundle S3Bucket=<bucket>,S3Key=deploy-vX.zip \
  --region us-east-1

# Deploy
aws elasticbeanstalk update-environment \
  --environment-name products-api-dev \
  --version-label vX.X.X \
  --region us-east-1
```

A connection string do RDS é configurada via variável de ambiente no EBS:

```
ConnectionStrings__Default = Host=<rds-endpoint>;Port=5432;Database=productsdb;Username=postgres;Password=<senha>;SSL Mode=Require;Trust Server Certificate=true
```

---
---

## 📝 Aprendizados

- Como o Elastic Beanstalk provisiona EC2 + Nginx + CloudWatch automaticamente
- Como fazer deploy via CLI (S3 → versão → update-environment)
- Como configurar Security Groups para comunicação EC2 → RDS
- Como passar connection string via variável de ambiente no EBS
- Como um servidor RDS pode hospedar múltiplos bancos de dados
- Diferença entre deploy serverless (Lambda) e em servidor (EC2/EBS)
