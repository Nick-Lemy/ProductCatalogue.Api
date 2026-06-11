# Product Catalogue API

Backend API for the product catalogue and asset management platform: manage products, their variants, and (soon) product assets, reviews, and publishing readiness.

## Project structure

```
ProductCatalogue.Api/
├── Controllers/        # API endpoints (ProductsController, VariantsController)
├── Services/           # Business logic (Product/, Variant/ - interface + implementation)
├── Models/             # Entities and enums (Product, Variant, ProductStatus, ProductReadiness)
├── DTOs/               # Request/response contracts (Create, Update, Query, Response per resource)
├── Data/               # AppDbContext + seed data
├── Exceptions/         # Custom exceptions + GlobalExceptionHandler
├── Mappings/           # Mapster mapping configuration
├── Migrations/         # EF Core migrations
├── Program.cs          # App startup and DI registration
├── docker-compose.yml  # Local PostgreSQL
└── .env.example        # Template for database credentials

ProductCatalogue.Tests/
├── Controllers/        # Controller tests (xUnit + Moq)
└── Services/           # Service tests (xUnit + EF in-memory)
```

## Run locally

All commands from `ProductCatalogue.Api/`.

```bash
# 1. Start PostgreSQL
cp .env.example .env # fill in your credentials
docker compose up -d

# 2. Set the connection string (one-time, stored outside the repo)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=3003;Database=product_catalogue_db;Username=<user>;Password=<password>"

# 3. Create the schema and seed data
dotnet ef database update

# 4. Run
dotnet run
```

API: `http://localhost:5093` — Swagger UI: `http://localhost:5093/swagger`

## Run tests

```bash
dotnet test ../ProductCatalogue.Tests/ProductCatalogue.Tests.csproj
```
