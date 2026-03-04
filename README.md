# 🚀 ProductSolution

A clean architecture based ASP.NET Core Web API project built using modern best practices including layered architecture, JWT authentication, Docker support, and Entity Framework Core.

---

## 📌 Project Overview

ProductSolution is a scalable backend system designed with separation of concerns using:

* Domain Layer
* Application Layer
* Infrastructure Layer
* API Layer
* Unit Testing Support

The project follows Clean Architecture principles to ensure maintainability, scalability, and testability.

---

## 🏗️ Architecture Structure

```
ProductSolution
│
├── src
│   ├── API
│   ├── Application
│   ├── Domain
│   └── Infrastructure
│
├── tests
└── ProductSolution.sln
```

### 🔹 Domain

Contains core business logic, entities, and exceptions.

### 🔹 Application

Contains DTOs, interfaces, services, and business rules.

### 🔹 Infrastructure

Handles database access, EF Core configurations, authentication services, and external integrations.

### 🔹 API

Entry point of the application. Contains controllers, middleware, authentication setup, and configuration.

---

## 🛠️ Technologies Used

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication (Access & Refresh Tokens)
* Docker & Docker Compose
* Clean Architecture
* Dependency Injection

---

## 🔐 Authentication

This project implements:

* JWT Access Tokens
* Refresh Tokens
* Secure login endpoint
* Token validation middleware

---

## 🐳 Running with Docker

### 1️⃣ Build the project

```
docker compose build
```

### 2️⃣ Run containers

```
docker compose up
```

### 3️⃣ Stop containers

```
docker compose down
```

---

## 🧪 Running Locally (Without Docker)

### 1️⃣ Restore packages

```
dotnet restore
```

### 2️⃣ Apply migrations

```
dotnet ef database update
```

### 3️⃣ Run API

```
dotnet run --project src/API
```

---

## 📂 Database Migrations

To add a new migration:

```
dotnet ef migrations add MigrationName --project src/Infrastructure --startup-project src/API
```

To update database:

```
dotnet ef database update --startup-project src/API
```

---

## 📬 API Testing

You can test APIs using:

* Swagger (when running locally)
* Postman

---

## 🔄 Git Workflow

```
git add .
git commit -m "Your commit message"
git push
```

---

## 📈 Future Improvements

* Role-based authorization
* Logging with Serilog
* Global exception handling middleware improvements
* CI/CD using GitHub Actions
* Unit & Integration Tests coverage expansion

---

## 👨‍💻 Author

Developed as a full-stack .NET learning and production-ready backend solution.

---

## 📄 License

This project is open-source and available under the MIT License.

---

⭐ If you found this project useful, consider giving it a star on GitHub!
