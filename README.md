# Company Management System

A full-stack application for managing company information, built with .NET 8 Web API and React TypeScript.

## Project Structure

- `/CompanyApi` - Backend API built with .NET 8
- `/company-client` - Frontend application built with React and TypeScript

## Backend Features

- CRUD operations for companies
- PostgreSQL database integration
- Docker support
- Swagger UI for API documentation
- Input validation using FluentValidation
- AutoMapper for object mapping

## Frontend Features

- Modern React application with TypeScript
- Company listing and management interface
- Form validation
- Responsive design

## Prerequisites

- .NET 8 SDK
- Node.js and npm
- Docker and Docker Compose
- PostgreSQL (if running locally without Docker)

## Getting Started

1. Clone the repository:
```bash
git clone git@github.com:ctaran/company-api.git
cd company-api
```

2. Start the PostgreSQL database using Docker:
```bash
cd CompanyApi
docker-compose up -d
```

3. Run the API:
```bash
cd src/CompanyApi/CompanyApi
dotnet run
```

4. In a new terminal, start the React client:
```bash
cd company-client
npm install
npm start
```

5. Access the applications:
- API Swagger UI: http://localhost:5020/swagger/index.html
- React Client: http://localhost:3000

## API Endpoints

- GET /api/companies - Get all companies
- GET /api/companies/{id} - Get a specific company
- GET /api/companies/isin/{isin} - Get a company by ISIN
- POST /api/companies - Create a new company
- PUT /api/companies/{id} - Update a company
- DELETE /api/companies/{id} - Delete a company

## Database Configuration

The API uses PostgreSQL with the following default configuration:
- Host: localhost
- Database: companydb
- Username: postgres
- Password: postgres

You can modify these settings in `CompanyApi/src/CompanyApi/CompanyApi/appsettings.json`.

## Development

### Backend
To add new migrations:
```bash
cd CompanyApi/src/CompanyApi/CompanyApi
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Frontend
To run tests:
```bash
cd company-client
npm test
```

## License

MIT 