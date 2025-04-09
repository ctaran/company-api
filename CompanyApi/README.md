# Company API

A .NET 8 Web API for managing company information with PostgreSQL database.

## Features

- CRUD operations for companies
- PostgreSQL database integration
- Docker support
- Swagger UI for API documentation
- Input validation using FluentValidation
- AutoMapper for object mapping

## Prerequisites

- .NET 8 SDK
- Docker and Docker Compose
- PostgreSQL (if running locally without Docker)

## Getting Started

1. Clone the repository:
```bash
git clone https://github.com/ctaran/company-api.git
cd company-api
```

2. Start the PostgreSQL database using Docker:
```bash
docker-compose up -d
```

3. Run the API:
```bash
cd src/CompanyApi/CompanyApi
dotnet run
```

4. Access the Swagger UI:
- http://localhost:5020/swagger/index.html

## API Endpoints

- GET /api/companies - Get all companies
- GET /api/companies/{id} - Get a specific company
- POST /api/companies - Create a new company
- PUT /api/companies/{id} - Update a company
- DELETE /api/companies/{id} - Delete a company

## Database Configuration

The API uses PostgreSQL with the following default configuration:
- Host: localhost
- Database: companydb
- Username: postgres
- Password: postgres

You can modify these settings in `appsettings.json`.

## Development

To add new migrations:
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## License

MIT 