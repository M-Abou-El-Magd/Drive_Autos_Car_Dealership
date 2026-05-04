# Car Dealership System API

## Overview
This is a complete ASP.NET Core Web API for a car dealership management system using clean architecture principles.

Features:
- User, UserProfile, Car, ClientInterest, RefreshToken entities
- One-to-One (User-UserProfile), One-to-Many (Seller-Car), Many-to-Many (Client-Car via ClientInterest)
- Role-based identities: Admin, SalesManager, Seller, Client
- Async service layer with `async/await`, repository-style clean methods
- DTO separation (Create/Update/Read) + data annotation validation
- JWT authentication + `Authorize` / role checks
- Hangfire scheduled job for daily inventory report
- Refresh token loop with HTTP-only cookie
- Swagger UI for API tests

## Technologies Used
- ASP.NET Core 10.0
- Entity Framework Core 10.0 (SQLite)
- Hangfire (scheduled jobs)
- JWT Bearer authentication
- Swagger (OpenAPI)
- Clean architecture with services, DTOs, controllers

## Database and Migrations

### Add migration and apply
```bash
cd /Users/mahmoudabouelmagd/Documents/web/Car_Dealership_System
dotnet tool install --global dotnet-ef --version 10.0.0
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Security: Why HTTP-only cookies are the standard

- HTTP-only cookies are not accessible to JavaScript (`HttpOnly=true`), reducing XSS attack surface.
- Combined with `Secure` and `SameSite=strict`, they prevent token theft and CSRF vectors.
- Using stateless JWT for auth token and issuing refresh tokens in HTTP-only cookies is a balance of user convenience and strong security.

## Running

### Backend

```bash
cd /Users/mahmoudabouelmagd/Downloads/Car_Dealership_System-main
dotnet run
```

Swagger UI: `http://localhost:5238/`
OpenAPI JSON: `http://localhost:5238/openapi`

### Frontend

```bash
cd /Users/mahmoudabouelmagd/Downloads/Car_Dealership_System-main/frontend
npm install
npm run dev
```

Open the frontend at `http://localhost:5173`

### API routes used by the frontend

- `POST /api/auth/login`
- `POST /api/users` (signup for Client and Seller)
- `GET /api/cars`
- `GET /api/cars/{id}`
- `POST /api/cars`
- `PUT /api/cars/{id}`
- `DELETE /api/cars/{id}`

### Signup details

- The frontend allows users to sign up as `Client` or `Seller`.
- Admin accounts cannot be created through public signup.
- User credentials are stored in the SQLite database used by the backend.

### Postman

- Added Postman collection: `postman_collection.json`
- Import this file into Postman.
- Use `POST /api/auth/login` first to obtain token and set `Authorization: Bearer <token>` for other requests.

## Default seeded users
- admin@example.com / password
- manager@example.com / password
- seller@example.com / password
- client@example.com / password

## Notes
- Controllers only return DTOs (never raw entities).
- All GET queries use `.AsNoTracking()` and `.Select(...)` projections.
- Role scopes enforced: `Delete` (Admin), `AddCar` (Seller).
