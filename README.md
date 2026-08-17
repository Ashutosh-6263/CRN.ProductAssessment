# CRN Product Assessment

A production-style ASP.NET Core Web API built with **.NET 8**, **Entity Framework Core**, **SQL Server**, **JWT Authentication**, **Docker**, and **automated testing**.

## 📌 Project Overview

CRN Product Assessment is a RESTful Web API for managing products and their related items.

The project demonstrates clean backend development practices including layered architecture, authentication, authorization, validation, repository patterns, API versioning, exception handling, automated testing, and Docker containerization.

## 🚀 Features

* ASP.NET Core Web API
* .NET 8
* Entity Framework Core
* SQL Server
* JWT Authentication
* Refresh Tokens
* Role-Based Authorization
* BCrypt Password Hashing
* Repository Pattern
* Unit of Work Pattern
* DTOs
* FluentValidation
* API Versioning
* Global Exception Handling
* Pagination
* Swagger / OpenAPI
* xUnit Testing
* Moq
* Docker
* Docker Compose

## 🏗️ Architecture

The solution follows a layered architecture:

```text
                    ┌─────────────────┐
                    │     CRN.API     │
                    │   Controllers   │
                    │    Middleware   │
                    └────────┬────────┘
                             │
                             ▼
                 ┌─────────────────────┐
                 │   CRN.Application   │
                 │ Services / DTOs     │
                 │ Interfaces / Rules │
                 └──────────┬──────────┘
                            │
                            ▼
                    ┌───────────────┐
                    │  CRN.Domain   │
                    │   Entities    │
                    │   Enums       │
                    │  Exceptions   │
                    └───────────────┘

                 ┌─────────────────────┐
                 │ CRN.Infrastructure │
                 │ EF Core / Repos     │
                 │ Database / Services │
                 └──────────┬──────────┘
                            │
                            ▼
                     ┌────────────┐
                     │ SQL Server │
                     └────────────┘
```

## 🛠️ Technology Stack

| Technology            | Purpose                          |
| --------------------- | -------------------------------- |
| .NET 8                | Backend framework                |
| ASP.NET Core Web API  | REST API                         |
| Entity Framework Core | ORM / Database access            |
| SQL Server            | Relational database              |
| JWT                   | Authentication                   |
| BCrypt                | Password hashing                 |
| FluentValidation      | Request validation               |
| xUnit                 | Automated testing                |
| Moq                   | Mocking                          |
| Swagger / OpenAPI     | API documentation                |
| Docker                | Containerization                 |
| Docker Compose        | Multi-container application      |
| Git                   | Version control                  |
| GitHub                | Source control and collaboration |

## 📂 Project Structure

```text
CRN.ProductAssessment
│
├── CRN.API
│   ├── Controllers
│   │   ├── AuthController.cs
│   │   ├── ProductsController.cs
│   │   └── ItemsController.cs
│   │
│   ├── Middleware
│   │   └── ExceptionHandlingMiddleware.cs
│   │
│   ├── Dockerfile
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
│
├── CRN.Application
│   ├── DTOs
│   │   ├── Auth
│   │   ├── Item
│   │   └── Product
│   │
│   ├── Interfaces
│   ├── Services
│   ├── Settings
│   └── Validators
│
├── CRN.Domain
│   ├── Entities
│   │   ├── Product.cs
│   │   ├── Item.cs
│   │   ├── User.cs
│   │   └── RefreshToken.cs
│   │
│   ├── Enums
│   └── Exceptions
│
├── CRN.Infrastructure
│   ├── Data
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations
│   │
│   ├── Migrations
│   ├── Repositories
│   └── Services
│
├── CRN.API.Tests
│   ├── CustomWebApplicationFactory.cs
│   ├── ProductsControllerTests.cs
│   └── TestAuthenticationHandler.cs
│
├── CRN.Application.Tests
│   └── ProductServiceTests.cs
│
├── docker-compose.yml
├── .dockerignore
├── .gitignore
├── CRN.ProductAssessment.sln
└── README.md
```

## 🔐 Authentication & Authorization

The API uses **JWT Bearer Authentication**.

Authentication flow:

```text
User
 │
 ▼
Register / Login
 │
 ▼
JWT Access Token
 │
 ▼
Authorization Header
 │
 ▼
Protected API Endpoint
```

The project also implements **Refresh Tokens** to allow users to obtain a new access token without logging in again.

Example authorization header:

```text
Authorization: Bearer <access_token>
```

Role-based authorization is also implemented using user roles such as:

```text
Admin
User
```

## 🔑 Password Security

User passwords are not stored as plain text.

Passwords are hashed using **BCrypt** before being stored in the database.

```text
Plain Password
      │
      ▼
   BCrypt
      │
      ▼
Password Hash
      │
      ▼
   SQL Server
```

## 📦 Product Management

The API provides functionality for managing products.

Supported operations include:

* Create product
* Retrieve products
* Retrieve product by ID
* Update product
* Delete product
* Pagination
* Audit information

Product audit fields include:

```text
CreatedBy
CreatedOn
ModifiedBy
ModifiedOn
```

## 📦 Item Management

Items are associated with products.

Relationship:

```text
Product
   │
   └── Items
        ├── Item 1
        ├── Item 2
        └── Item 3
```

The API validates that the requested product exists before creating or retrieving product-related items.

For example:

```text
GET /api/v1.0/products/1/items
```

returns the items belonging to product `1`.

If the product does not exist:

```text
GET /api/v1.0/products/9/items
```

the API returns:

```json
{
  "statusCode": 404,
  "message": "Product with id 9 was not found.",
  "traceId": "..."
}
```

## 🌐 API Versioning

The API uses URL-based API versioning.

Example:

```text
/api/v1.0/products
```

Product items:

```text
/api/v1.0/products/{productId}/items
```

This allows future API versions to be introduced without breaking existing clients.

## 📋 API Endpoints

### Authentication

```text
POST /api/v1.0/auth/register
POST /api/v1.0/auth/login
POST /api/v1.0/auth/refresh-token
```

### Products

```text
GET    /api/v1.0/products
GET    /api/v1.0/products/{id}
POST   /api/v1.0/products
PUT    /api/v1.0/products/{id}
DELETE /api/v1.0/products/{id}
```

### Product Items

```text
GET /api/v1.0/products/{productId}/items
```

Example:

```text
GET /api/v1.0/products/1/items
```

Successful response:

```json
[
  {
    "id": 1,
    "productId": 1,
    "quantity": 10
  }
]
```

## 🗄️ Database

The application uses **SQL Server** with **Entity Framework Core**.

Main database tables:

```text
Users
RefreshTokens
Products
Items
```

Relationship:

```text
Users
 │
 └── RefreshTokens

Products
 │
 └── Items
```

Entity Framework Core migrations are located in:

```text
CRN.Infrastructure/Migrations
```

## 🔄 Repository Pattern

The application uses the **Repository Pattern** to separate database access from business logic.

Repositories include:

```text
Repository
ProductRepository
ItemRepository
UserRepository
RefreshTokenRepository
```

This makes the application easier to maintain and test.

## 🔄 Unit of Work

The project uses the **Unit of Work Pattern** to coordinate multiple repository operations.

Example structure:

```text
UnitOfWork
   │
   ├── Products
   ├── Items
   ├── Users
   └── RefreshTokens
```

## ✅ Validation

The project uses **FluentValidation** for validating incoming requests.

Validation is implemented for requests such as:

```text
RegisterRequest
LoginRequest
CreateProductRequest
UpdateProductRequest
CreateItemRequest
UpdateItemRequest
```

This keeps validation logic separate from controllers.

## ⚠️ Global Exception Handling

The application includes a global exception handling middleware:

```text
CRN.API/Middleware/ExceptionHandlingMiddleware.cs
```

It provides consistent error responses.

Example:

```json
{
  "statusCode": 404,
  "message": "Product with id 9 was not found.",
  "traceId": "..."
}
```

## 🧪 Automated Testing

The solution contains two test projects:

```text
CRN.Application.Tests
CRN.API.Tests
```

Testing technologies:

* xUnit
* Moq
* ASP.NET Core WebApplicationFactory
* Custom Authentication Handler

### Test Result

The current test suite contains:

```text
Total:    12
Passed:   12
Failed:   0
Skipped:  0
```

Run all tests with:

```powershell
dotnet test
```

## 🐳 Docker

The project uses Docker Compose to run the API and SQL Server.

Architecture:

```text
┌─────────────────────┐
│      CRN API        │
│    Port: 8080       │
└──────────┬──────────┘
           │
           │ Docker Network
           │
┌──────────▼──────────┐
│     SQL Server      │
│    Port: 1433       │
└─────────────────────┘
```

### Start Containers

```powershell
docker compose up -d --build
```

### Check Containers

```powershell
docker compose ps
```

### View API Logs

```powershell
docker compose logs --tail=50 api
```

### Stop Containers

```powershell
docker compose down
```

### Restart API

```powershell
docker compose restart api
```

## 🚀 Running the Application

### Prerequisites

Install:

* .NET 8 SDK
* Docker Desktop
* Git

### Clone Repository

```powershell
git clone https://github.com/Ashutosh-6263/CRN.ProductAssessment.git
```

Navigate to the project:

```powershell
cd CRN.ProductAssessment
```

### Run with Docker

```powershell
docker compose up -d --build
```

Check the services:

```powershell
docker compose ps
```

The API is available at:

```text
http://localhost:8080
```

Swagger UI:

```text
http://localhost:8080/swagger
```

## 🔧 Run Without Docker

Restore dependencies:

```powershell
dotnet restore
```

Build the solution:

```powershell
dotnet build
```

Run the API:

```powershell
dotnet run --project CRN.API
```

Run tests:

```powershell
dotnet test
```

## 🔒 Configuration & Security

Sensitive credentials should never be committed to GitHub.

The project uses environment variables for sensitive Docker configuration.

The following file is intentionally excluded from Git:

```text
.env
```

The `.gitignore` contains:

```text
.env
```

Production environments should use secure configuration mechanisms such as:

* Environment variables
* Azure Key Vault
* AWS Secrets Manager
* GitHub Actions Secrets
* Other secure secret-management solutions

## 📊 Example Database Data

Example products currently used during development:

```text
Id    ProductName       CreatedBy
----------------------------------
1     Lenovo            somesh
2     Acer Laptop       pawan
```

Example item:

```text
Id    ProductId    Quantity
---------------------------
1     1            10
```

## 📖 Swagger / OpenAPI

Swagger is enabled for API documentation and testing.

Open:

```text
http://localhost:8080/swagger
```

Swagger provides:

* Available endpoints
* Request models
* Response models
* Authentication
* API versioning
* Interactive API testing

## 🔍 Example API Response

Successful request:

```json
[
  {
    "id": 1,
    "productId": 1,
    "quantity": 10
  }
]
```

Not found request:

```json
{
  "statusCode": 404,
  "message": "Product with id 9 was not found.",
  "traceId": "0HNNS56D684TJ:00000002"
}
```

## 📌 Development Practices

The project follows several backend development practices:

* Separation of concerns
* Dependency Injection
* Layered architecture
* DTO-based API contracts
* Repository abstraction
* Unit of Work
* Input validation
* Centralized exception handling
* Authentication and authorization
* Automated testing
* Containerized development
* API versioning
* Secure configuration

## 📄 License

This project was created as a technical assessment and portfolio project.
