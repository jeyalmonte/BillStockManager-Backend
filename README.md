# Bill Stock Manager - Backend

## Description

BillStockManager-Backend is a backend system developed with .NET 8.0, designed to manage bills and product stock for retail environments. It integrates with a SQL database and exposes a robust RESTful API for handling billing operations, inventory management, and user authentication via JWT tokens.

The project follows Clean Architecture and Clean Code principles, ensuring a modular, maintainable, and scalable codebase built on industry best practices.

## Table of Contents

- [Prerequisites](#prerequisites)
  - [For Local Development](#for-local-development)
  - [For Dockerized Deployment](#for-dockerized-deployment)
- [Project Structure](#project-structure)
- [Installation](#installation)
- [Dependencies](#dependencies)
- [Scripts](#scripts)
  - [Database Migrations](#database-migrations)
  - [Database Seeding](#database-seeding)
  - [Running Tests](#running-tests)
- [License](#license)

## Prerequisites

Before running the project, ensure the following tools are installed on your system:

### For Local Development

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
  Required to build and run the backend.

- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
  Required if you're running the database locally without Docker.

- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extensions (optional)

### For Dockerized Deployment

- [Docker](https://www.docker.com/)  
  Required to containerize and run the application and database.

- [Docker Compose](https://docs.docker.com/compose/)  
  Used to orchestrate multiple containers.

- [Postman](https://www.postman.com/)  
  Recommended tool for testing API endpoints.

---

## Project Structure

```
BillStockManager-Backend
├── src
│   ├── Api
│   ├── Application
│   ├── Domain
│   ├── Infrastructure
│   └── SharedKernel
│── Tests
│   ├── Application.UnitTests
│   ├── Domain.UnitTests
│   ├── Architecture.UnitTests
├── .dockerignore
├── .gitignore
├──  Directory.Packages.props
├── docker-compose.yml
├── Dockerfile
├── README.md
├── BillStockManager.sln

```

- `src/`: Contains the source code for the application, divided into different projects for API, Application, Domain, Infrastructure and SharedKernel.
  - `Api`: The ASP.NET Core Web API project that serves as the entry point for the application.
  - `Application`: Contains the application logic, including services and use cases.
  - `Domain`: Contains the domain entities and business logic.
  - `Infrastructure`: Contains the data access layer, including repositories and database context.
  - `SharedKernel`: Contains shared components and utilities used across the application.
- `Tests/`: Contains the test projects for unit and integration tests.
  - `Application.UnitTests`: Unit tests for the application layer.
  - `Domain.UnitTests`: Unit tests for the domain layer.
  - `Architecture.UnitTests`: Tests to ensure adherence to Clean Architecture principles.
- `docker-compose.yml`: Docker Compose file for orchestrating multiple containers.
- `Dockerfile`: Dockerfile for building the application image.
- `Directory.Packages.props`: Directory-level NuGet package management file.
- `README.md`: Documentation for the project.
- `BillStockManager.sln`: Solution file for the project.

---

## Installation

### Local Development

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/BillStockManager-Backend.git
    cd BillStockManager-Backend
   ```
2. Open the solution in Visual Studio or your preferred IDE.
3. Restore the NuGet packages:
   ```bash
   dotnet restore
   ```
4. Update the connection string in `appsettings.json` to point to your local SQL Server instance.
5. Update the JWT credentials in `appsettings.json` to match your desired secret key, issuer, and audience.
6. Run the database migrations to set up the database schema:
   ```bash
   dotnet ef database update --project src/Infrastructure/BillStockManager.Infrastructure.csproj
   ```
7. Start the application:
   ```bash
    dotnet run --project src/Api/BillStockManager.Api.csproj
   ```
8. The API will be available at `http://localhost:5000` (or the port specified in your configuration)
9. Use a tool like Postman or Swagger to test the API endpoints.
10. To run the tests, execute the following command in the solution directory:
    ```bash
    dotnet test
    ```

---

### Dockerized Deployment

1. Ensure Docker is running on your machine.
2. Build and run the Docker containers using Docker Compose:
   ```bash
   docker-compose up --build -d
   ```
3. The API will be available at `http://localhost:5000` (or the port specified in your configuration).
4. Use a tool like Postman or Swagger to test the API endpoints.
5. To stop the containers, run:
   ```bash
   docker-compose down
   ```

---

## Dependencies

The project uses the following core libraries and tools:

- **Entity Framework Core** (`Microsoft.EntityFrameworkCore`, `SqlServer`, `Tools`) – ORM for data access.
- **JWT Authentication** (`Microsoft.AspNetCore.Authentication.JwtBearer`, `System.IdentityModel.Tokens.Jwt`) – Secure token-based authentication.
- **Validation** (`FluentValidation`, `FluentValidation.AspNetCore`) – Clean, reusable input validation.
- **CQRS** (`MediatR`) – Decouples business logic with a mediator pattern.
- **Mapping** (`Mapster`) – Lightweight object mapping between layers.
- **Swagger** (`Swashbuckle.AspNetCore`) – API documentation and testing UI.
- **Testing** (`xUnit`, `Moq`, `FluentAssertions`) – For unit and integration testing.

Full list available in the `Directory.Packages.props` file.

---

## Scripts

### Database Migrations

- To create a new migration, run the following command in the `src/Infrastructure` directory:
  ```bash
  dotnet ef migrations add MigrationName --project BillStockManager.Infrastructure.csproj --startup-project BillStockManager.Api.csproj
  ```
- To apply the migrations to the database, run:
  ```bash
  dotnet ef database update --project BillStockManager.Infrastructure.csproj --startup-project BillStockManager.Api.csproj
  ```
- To remove the last migration, run:
  ```bash
  dotnet ef migrations remove --project BillStockManager.Infrastructure.csproj --startup-project BillStockManager.Api.csproj
  ```

### Database Seeding

- The database seeding is handled in the `Infrastructure` project. You can modify the `DbInitializer` class to customize the initial data.
- To seed the database, ensure that the `Seed` method is called in the `Configure` method of the `Startup` class in the `Api` project.
- The seeding process will run automatically when the application starts, provided that the database is empty.
- You can also manually trigger the seeding process by calling the `Seed` method in your application startup logic.

### Running Tests

- To run tests in a specific project, navigate to that project directory and run:
  ```bash
    dotnet test
  ```
- To run tests with code coverage, use the following command:

  ```bash
  dotnet test --collect:"XPlat Code Coverage"
  ```

---

## License

Designed and developed by [Jeyson Almonte](https://github.com/jjalmonte), a passionate Software Engineer dedicated to building scalable and maintainable software solutions.
