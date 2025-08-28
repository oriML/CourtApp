# 📌 Project Overview

This project implements a **Web API** using **.NET C#** and a **PostgreSQL (Render) database**.  
It provides CRUD operations for handling **inquiries (פניות)**, user authentication via **JWT**, and integration with **Angular 19** frontend.

---

## 🏗 Architecture

The system follows **Separation of Concerns (SoC)** and a clean modular structure:

- **API Layer** – Controllers exposing endpoints (CRUD + reports).
- **Business Layer** – Services implementing business logic.
- **Data Access Layer** – EF Core / Dapper for DB operations.
- **Models** – DTOs, Entities, Validation classes (FluentValidation).
- **Security** – JWT authentication middleware, role-based authorization.
- **Concurrency Handling** – Optimistic concurrency via row-versioning.
- **Thread Management** – Async/await + task parallelism where needed.

---

## 🔐 Security

- **JWT Authentication** for secure access.
- **HttpOnly Cookies / LocalStorage token** (Angular client).
- **Input Validation** with FluentValidation to prevent injection.
- **CORS** policy configured for Angular app domain.
- **Error Handling Middleware** ensures structured error responses.

---

## 📊 Database

Currently uses **PostgreSQL (Render)**.  
Entities:

- **Users**: authentication and authorization.
- **Inquiries**: name, phone, email, departments[], description, status, createdAt, updatedAt.

Includes a **stored procedure** for generating a **monthly inquiries report**.

---

## 🌐 Frontend (Angular 19)

- **Login page** (JWT authentication).
- **Inquiries list** with CRUD operations.
- **Modal dialogs** for editing/viewing details.
- **Snackbar notifications** for success, errors, login, delete.
- **Monthly report dashboard**.
- **Responsive design** with Angular Material (white & light-blue theme).

---

## ⚙️ Installation & Running

### 🔹 Backend (.NET API)

1. Clone repository:
   ```bash
   git clone <repo-url>
   cd backend
   ```

2. Configure database connection (appsettings.json):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=<render-host>;Database=<db-name>;Username=<user>;Password=<password>"
   }
   ```

3. Run database migrations / seed:
   ```bash
   dotnet ef database update
   ```

4. Start API:
   ```bash
   dotnet run
   ```

---

### 🔹 Frontend (Angular 19)

1. Navigate to Angular app folder:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Run app:
   ```bash
   ng serve -o
   ```

---

## 🧪 Testing

- **Unit Tests** with xUnit / NUnit for services and controllers.
- **Integration Tests** for API endpoints.
- **Angular Unit Tests** with Jasmine/Karma.

---

## ✅ Pros & Cons of Chosen Stack

### Pros
- Modern .NET stack with strong typing.
- PostgreSQL is reliable, scalable, and supported by Render.
- Angular Material provides professional responsive UI.
- JWT enables stateless authentication.

### Cons
- More initial setup complexity (DB + Auth).
- Hosting costs on Render for large-scale data.
- Angular learning curve compared to simpler frameworks.

---

## 🚀 Deployment

- Backend containerized with **Docker**.
- Environment variables injected at runtime.
- CI/CD pipeline (GitHub Actions / Azure DevOps) recommended.

---

## 📌 Notes

- Cross-domain handled via **CORS middleware** in .NET.
- Error handling ensures all API responses are standardized (ProblemDetails).
- Future: add **user registration & roles**, advanced reporting, caching.
