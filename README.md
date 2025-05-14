
# JK-Tech - Document Management System

Welcome to **JK-Tech**, a comprehensive and modular **Document Management System** developed using **ASP.NET Core**. This system provides a seamless experience for user authentication, document storage, and secure access control while adhering to best practices in software development, including separation of concerns, maintainability, and testability.

The system includes essential features for managing documents, such as authentication via JWT tokens, role-based access control, CRUD operations for documents, and logging throughout the flow for better monitoring and debugging.

---

## Key Features

This system supports the following features:

1. **User Authentication & Authorization:**
   - **Register**: Users can register by providing their username, password, and role.
   - **Login**: Validates users' credentials and generates a JWT token for authentication.
   - **Logout**: Invalidates the current JWT token, logging the user out.
   - **Role-based access**: Different access levels can be assigned to users (e.g., Admin, User).

2. **Document Management:**
   - **Create**: Allows users to upload and store documents.
   - **Read**: Retrieve document details by ID.
   - **Update**: Edit metadata for existing documents.
   - **Delete**: Remove documents from the system.

3. **Logging and Ingestion Tracking:**
   - Integrated **Ingestion Controller**, **Service**, and **Repository** for tracking the flow and logging all document-related actions.
   - Each step of the document processing and authentication flow is logged for better monitoring and troubleshooting.

4. **Unit Testing:**
   - Unit tests are written for controllers, services, and repositories using the **xUnit** framework, ensuring the reliability and correctness of the application.

---

## Project Structure

This project follows a modular structure that separates responsibilities into different layers. The main components are:

### 1. **Auth Module** (`DocManagementSystem.Auth`)
   - Handles all authentication-related tasks, including user registration, login, and JWT token generation.
   - Implements controllers and services to manage user credentials and session tokens.
   - **Controllers**: `AuthController` provides API endpoints for registration, login, and logout.
   - **Services**: `AuthService` contains business logic for user authentication.
   - **Repositories**: `AuthRepository` manages data access for users.

### 2. **Document Module** (`DocManagementSystem.Document`)
   - Manages the CRUD operations for documents in the system.
   - **Controllers**: `DocumentController` provides endpoints for managing documents (create, read, update, delete).
   - **Services**: `DocumentService` handles business logic for document management.
   - **Repositories**: `DocumentRepository` interacts with the database to perform CRUD operations on documents.
   
### 3. **Ingestion Tracking Module** (`DocManagementSystem.Ingestion`)
   - New **Ingestion Controller**, **Service**, and **Repository** have been implemented to track the flow of document ingestion.
   - Logs all document upload activities, allowing you to trace when a document is ingested, processed, and stored.

   **IngestionController**: 
   Handles the ingestion of documents, logging all document uploads and initiates business logic through the ingestion service.
   
   **IngestionService**: 
   Contains business logic for tracking document ingestion events, including validation and processing.

   **IngestionRepository**: 
   Interacts with the database to store logs and track the ingestion history of documents.

### 4. **Shared Module** (`DocManagementSystem.Shared`)
   - Contains common models and utility classes used across the application.
   - Includes shared DTOs (Data Transfer Objects), validation models, and helper functions.

### 5. **Testing Module** (`DocManagementSystem.Test`)
   - Contains unit tests for all core components.
   - Uses **xUnit** to write tests for the AuthController, DocumentController, IngestionController, services, and repositories.

---

## Tech Stack

The project uses the following technologies:

- **ASP.NET Core**: The backend framework for building the API.
- **Entity Framework Core**: An Object-Relational Mapper (ORM) for database access and management.
- **JWT (JSON Web Tokens)**: A compact, URL-safe means of representing claims to securely authenticate users.
- **xUnit**: A testing framework for unit and integration tests.
- **Application Insights** (optional): For monitoring application performance and logging detailed telemetry data.

---

## How to Set Up and Run the Project

To get started with this project locally, follow these steps:

### 1. Clone the Repository

Clone the repository to your local machine using Git:


git clone https://github.com/Anuj-Rajvansh/jk-tech.git
cd jk-tech

### 2. Set Up the MySql Database

Ensure that you have a database set up for the application. The connection string for the database should be configured in the `appsettings.json` file for both the Auth and Document modules.

Example of configuring the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server_name;Database=your_database_name;User Id=your_username;Password=your_password;"
  }
}
```

### 3. Run the Application

To run the application, execute the following commands:

For the Auth API:

```bash
dotnet run --project DocManagementSystem.Auth
```

For the Document API:

```bash
dotnet run --project DocManagementSystem.Document
```

This will start the APIs on the specified ports (default is usually `http://localhost:5000` for Auth and `http://localhost:5001` for Document).

---

## Running Unit Tests

The project includes unit tests written with the **xUnit** framework. To run all tests:

1. Restore dependencies:

   ```bash
   dotnet restore
   ```

2. Run the tests:

   ```bash
   dotnet test
   ```

This will execute the tests and show the results in the terminal.

Tests are written for the following components:

- AuthController
- DocumentController
- AuthService
- DocumentService
- Repositories

---

## Logging

Logging is crucial for tracking and debugging the application's flow. In this system, logging is implemented in:

- **Controllers**: For tracking incoming requests and responses.
- **Services**: For business logic tracking.
- **Repositories**: For tracking database interactions and CRUD operations.

Logs can be tracked using tools like **Application Insights** for detailed telemetry and performance tracking.

---

## Notes

- **JWT Authentication**: The API endpoints that require authorization use JWT tokens for secure access.
- **Roles**: Users can be assigned different roles (like Admin or User), which define the level of access they have within the system.
- **Database**: Ensure that your database is correctly configured before running the project. Entity Framework Core will handle the migration of tables.

---

## License

This project is open-source and available under the **MIT License**. Feel free to fork and contribute.

