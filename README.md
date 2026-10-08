# GarmentsAPI

A RESTful API for managing garment business operations, including employees, customers, vendors, inventory, purchases, sales, users, and work assignments.

Built with **ASP.NET Core 8 Web API**, **Dapper**, **MySQL**, and **JWT Authentication**.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet\&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?logo=dotnet\&logoColor=white)](https://learn.microsoft.com/aspnet/core)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql\&logoColor=white)](https://www.mysql.com/)
[![Dapper](https://img.shields.io/badge/Dapper-ORM-512BD4)](https://github.com/DapperLib/Dapper)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

## Overview

**GarmentsAPI** provides the backend services required for a garment management application.

The API follows a repository-based architecture and exposes HTTP endpoints that can be consumed by web applications, mobile applications, desktop applications, or other services.

The project includes:

* User authentication and authorization
* JWT-based security
* Employee management
* Customer management
* Vendor management
* Inventory management
* Purchase management
* Sales management
* Work assignment management
* MySQL database integration
* Swagger/OpenAPI documentation

## Technology Stack

| Technology        | Purpose                     |
| ----------------- | --------------------------- |
| ASP.NET Core 8    | REST API framework          |
| C#                | Programming language        |
| .NET 8            | Runtime                     |
| Dapper            | Data access                 |
| MySQL             | Database                    |
| JWT Bearer        | Authentication              |
| BCrypt            | Password hashing            |
| Swagger / OpenAPI | API documentation           |
| ClosedXML         | Excel-related functionality |

The project targets `net8.0` and includes packages for Dapper, MySQL, JWT authentication, BCrypt, Swagger, and ClosedXML.

## Project Structure

```text
GarmentsAPI/
│
├── Controllers/
│   ├── EmployeeController.cs
│   ├── InventoryController.cs
│   ├── VendorController.cs
│   ├── CustomerController.cs
│   ├── PurchaseController.cs
│   ├── SalesController.cs
│   └── UserController.cs
│
├── Properties/
│
├── Customer.cs
├── CustomerRepository.cs
├── Employeee.cs
├── EmployeeRepository.cs
├── InventoryItem.cs
├── InventoryRepository.cs
├── PurchaseOrder.cs
├── PurchaseRepository.cs
├── SalesOrder.cs
├── SalesRepository.cs
├── User.cs
├── UserRepository.cs
├── Vendor.cs
├── VendorRepository.cs
├── WorkAssignment.cs
├── WorkAssignmentRepository.cs
│
├── CreateUserRequest.cs
├── LoginRequest.cs
│
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── GarmentsAPI.csproj
├── GarmentsAPI.slnx
└── garmentsdb.sql
```

## Main Modules

### 👤 User Management

Provides user registration/login and JWT-based authentication.

Features include:

* User login
* Password hashing
* JWT token generation
* Role-based authorization
* Authenticated API access

### 👨‍💼 Employee Management

Manage employee information and employee-related operations.

### 👥 Customer Management

Maintain customer records and retrieve customer information through REST APIs.

### 🏭 Vendor Management

Manage garment-business vendors and supplier information.

### 📦 Inventory Management

Manage inventory items and stock information.

### 🛒 Purchase Management

Manage purchase orders and purchasing-related information.

### 💰 Sales Management

Manage sales orders and sales-related operations.

### 👷 Work Assignment

Manage assignments of work to employees.

## Authentication

The API uses **JWT Bearer Authentication**.

After successful login, the API returns a JWT token.

For protected endpoints, send the token using the HTTP `Authorization` header:

```http
Authorization: Bearer YOUR_JWT_TOKEN
```

JWT validation includes:

* Issuer validation
* Audience validation
* Token lifetime validation
* Signing-key validation
* Role claims
* User identity claims

## Database

The application uses **MySQL**.

A database script is included in the repository:

```text
garmentsdb.sql
```

### Create the Database

1. Install MySQL Server.
2. Open MySQL Workbench or another MySQL client.
3. Create the database.
4. Import:

```text
garmentsdb.sql
```

Example:

```sql
CREATE DATABASE garmentsdb;
```

Then import the SQL file into the database.

## Configuration

Update the connection string and JWT configuration in:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "GarmentDB": "Server=localhost;Database=garmentsdb;User=root;Password=YOUR_PASSWORD;"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "GarmentsAPI",
    "Audience": "GarmentsAPIUsers"
  }
}
```

### Important

Do **not** commit real database passwords or production JWT secret keys to GitHub.

For production deployments, use environment variables or a secure secret-management solution.

## Prerequisites

Before running the project, install:

* .NET 8 SDK
* MySQL Server 8.x
* Git
* Visual Studio 2022, Visual Studio Code, or another C# IDE

Verify .NET:

```bash
dotnet --version
```

Verify Git:

```bash
git --version
```

## Installation

Clone the repository:

```bash
git clone https://github.com/pankajbiodata/GarmentsAPI.git
```

Navigate to the project:

```bash
cd GarmentsAPI
```

Restore NuGet packages:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

## Run the API

Start the development server:

```bash
dotnet run
```

The API will start on the URLs displayed by the .NET runtime.

You can also run it from Visual Studio using the project or solution file.

## Swagger API Documentation

Swagger is enabled in the application.

After starting the API, open:

```text
/swagger
```

For example:

```text
https://localhost:xxxx/swagger
```

Swagger provides an interactive interface for:

* Viewing API endpoints
* Inspecting request/response models
* Testing API calls
* Testing authenticated endpoints

## Health Check

The root endpoint returns:

```text
Garments API is running
```

Example:

```http
GET /
```

## API Architecture

The application uses a controller/repository architecture:

```text
Client Application
       │
       ▼
   REST API
       │
       ▼
 Controllers
       │
       ▼
 Repositories
       │
       ▼
    Dapper
       │
       ▼
     MySQL
```

This separation keeps HTTP/API handling separate from database access.

## Example API Request

### Login

```http
POST /api/User/login
Content-Type: application/json
```

Example request:

```json
{
  "username": "admin",
  "password": "your-password"
}
```

The returned JWT token can then be supplied to protected endpoints:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

## Using the API from a Mobile or Web Application

The API can be consumed by applications built with technologies such as:

* React Native
* Flutter
* React
* Angular
* .NET MAUI
* Android
* iOS

Example JavaScript request:

```javascript
const response = await fetch(
  "https://your-server.com/api/customers",
  {
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json"
    }
  }
);

const customers = await response.json();
```

## Security

The API implements JWT authentication and BCrypt password hashing.

For production deployment:

* Use HTTPS.
* Use a strong JWT signing key.
* Never commit secrets to Git.
* Use environment variables for credentials.
* Restrict MySQL access.
* Configure CORS for trusted applications only.
* Use production-specific configuration.
* Keep .NET and NuGet dependencies updated.

## Database Script

The repository contains the database initialization script:

```text
garmentsdb.sql
```

This can be used to recreate the application's MySQL database structure.

## Development

Build:

```bash
dotnet build
```

Run:

```bash
dotnet run
```

Clean:

```bash
dotnet clean
```

Restore packages:

```bash
dotnet restore
```

## Deployment

The API can be deployed to environments supporting ASP.NET Core, including:

* Windows Server + IIS
* Linux + Nginx
* Docker
* Azure
* AWS
* Other .NET-compatible hosting platforms

Publish the application using:

```bash
dotnet publish -c Release
```

The generated files can then be deployed to the target server.

## Related Application

This API is designed to act as the backend for a garment-management application.

It can be integrated with a web or mobile frontend to provide:

```text
Authentication
     │
     ├── Users
     ├── Employees
     ├── Customers
     ├── Vendors
     ├── Inventory
     ├── Purchases
     ├── Sales
     └── Work Assignments
```

## Contributing

Contributions, issues, and feature requests are welcome.

1. Fork the repository.
2. Create a feature branch.

```bash
git checkout -b feature/my-feature
```

3. Commit your changes.

```bash
git commit -m "Add my feature"
```

4. Push the branch.

```bash
git push origin feature/my-feature
```

5. Open a Pull Request.

## License

This project is licensed under the MIT License.

See the `LICENSE` file for details.

## Author

**Pankaj Kumar**

GitHub:
https://github.com/pankajbiodata

Repository:
https://github.com/pankajbiodata/GarmentsAPI

````

### Add it to GitHub

From your local `GarmentsAPI` folder:

```bash
git pull origin master
````

Create the file:

```bash
notepad README.md
```

Paste the README above and save it.

Then:

```bash
git add README.md
git commit -m "Add project README"
git push origin master
```

Your GitHub repository currently has **no README/description**, so adding this will significantly improve how the project is presented to visitors and potential employers/clients.

One thing I would change after this: **add GitHub repository topics and a short repository description** such as `aspnet-core`, `dotnet-8`, `web-api`, `mysql`, `dapper`, `jwt`, `garment-management`, and `inventory-management`. That will make the project easier to understand and discover.
