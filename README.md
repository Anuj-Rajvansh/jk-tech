
# JK-Tech - Document Management System

Welcome to JK-Tech, a clean and modular Document Management System built with ASP.NET Core. It’s designed to handle user authentication, document storage, and secure access control — all following best practices for maintainable, testable code.

---

## What This Project Does

This project lets you:

- Register, login, and logout users using JWT tokens
- Assign roles to users and secure APIs accordingly
- Perform full CRUD operations on documents
- Follow a clean architecture: separate responsibilities for controller, service, and repository
- Ingestion API's to track the status
- Write and run unit tests for core components using xUnit

---

## Project Structure (Simplified)

| Folder | Purpose |
|--------|---------|
| `DocManagementSystem.Auth` | Handles user login, registration, CURD Documents and JWT auth |
| `DocManagementSystem.Document` | APIs to manage document data |
| `DocManagementSystem.Services` | Business logic for both Auth and Document modules |
| `DocManagementSystem.Repositories` | Data access layer using EF Core |
| `DocManagementSystem.Shared` | Common models, Custom Middleware and helpers used across the app |
| `DocManagementSystem.Test.*` | xUnit-based tests cases for controllers, services, and repos |

---

## Tech Stack

- ASP.NET Core 8– Web API backend
- Entity Framework Core – ORM for database access
- JWT – JSON Web Token for authentication
- xUnit – Unit testing framework
- Application Insights (optional) – For monitoring and logging

---

## How to Run the Project

1. Clone the repo:

   ```bash
   git clone https://github.com/Anuj-Rajvansh/jk-tech.git
   cd jk-tech
   ```

2. Make sure your connection string is set in `appsettings.json` under `DocManagementSystem.Auth` and `DocManagementSystem.Document`.

3. Run the project:

   ```bash
   dotnet run --project DocManagementSystem.Auth
   ```

   And separately:

   ```bash
   dotnet run --project DocManagementSystem.Document
   ```

---

## Running Tests

We use xUnit for unit testing. To run all tests:

```bash
dotnet test
```

Tests are already written for:

- Auth Controller
- Document Controller
- Services and Repositories

---

## Notes

- Logging is implemented across controller, service, and repository layers.
- JWT tokens are validated on protected endpoints.
- You can plug in Application Insights to trace and monitor your API flow.

---

## License

This project is open-source and available under the MIT License.
