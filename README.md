# HealthFitnessAPI

A RESTful API for a health and fitness social platform built with **ASP.NET Core 9.0**. Users can track fitness achievements, connect with friends, engage with a social feed, and earn badges — all backed by a clean, layered architecture.

## Features

- **JWT Authentication** — Access + refresh token flow with secure token revocation on logout/password change
- **Achievement System** — Achievements with multiple levels and gender-specific thresholds; users earn and display badges
- **Social Feed** — Paginated, sortable feed of friends' achievements with like support
- **Friend System** — Send, accept, decline, and remove friend connections
- **Role-Based Authorization** — `Admin` and `User` roles with granular endpoint access control
- **Background Jobs** — Quartz.NET scheduler cleans up expired refresh tokens hourly
- **Soft Deletes** — All core entities support soft deletion via EF Core query filters

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 9 / ASP.NET Core Web API |
| Database | SQL Server + Entity Framework Core 9 |
| Authentication | JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer) |
| Mapping | AutoMapper 14 |
| Validation | FluentValidation 12 |
| Scheduling | Quartz.NET 3.14 |
| API Docs | Scalar (OpenAPI) |
| Response Wrapping | AutoWrapper |

## Architecture

The project follows a clean, layered structure:

```
Controllers      → HTTP request handling, route definitions
Services         → Business logic
Repository       → Generic data access (Repository + Unit of Work pattern)
Entities         → EF Core models (code-first)
Model/Dtos       → Request/response DTOs
Model/Profiles   → AutoMapper mapping profiles
Middlewares      → JWT user ID extraction
ScheduledJobs    → Background task definitions
```

## Data Model

```
User ──────────── Friendship ──────────── User
  │
  └── UserAchievement ── Achievement ── AchievementLevelThreshold ── AchievementLevel
            │
            └── UserAchievementLike
```

- Achievements have **levels** (e.g., Bronze, Silver, Gold) with separate male/female thresholds
- Users earn achievements at a specific level, which appear on their public profile
- Friends can **like** each other's earned achievements

## API Overview

| Module | Endpoints |
|---|---|
| Auth | Login, refresh token, logout |
| User | CRUD, profile, public profile, feed, achievements, likes |
| Friendship | Get friends, pending requests, add, accept, decline, remove |
| Achievement | CRUD, levels, badge images |
| Achievement Level | CRUD |
| User Achievement | CRUD (admin), per-user view |
| File | Upload/retrieve achievement badge images |

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server (local or Docker)

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/HealthFitnessAPI.git
   cd HealthFitnessAPI
   ```

2. Update the connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=HealthFitnessDb;User ID=sa;Password=YourPassword;TrustServerCertificate=true;"
   }
   ```

3. Run the application (migrations are applied automatically on startup):
   ```bash
   cd HealthFitnessAPI
   dotnet run
   ```

4. Open the interactive API docs at `http://localhost:5000/scalar/v1`

5. Optionally seed mock data by calling `GET /api/admin/init` with the default admin credentials.

### Default Admin Account

Configured in `appsettings.json` under `DefaultAdmin`. The admin account is created automatically on first run.

## Security

- Passwords hashed with **PBKDF2-SHA256** (100,000 iterations, per-password salt)
- Fixed-time comparison to prevent timing attacks
- JWT keys and sensitive config should be moved to environment variables or a secrets manager before production deployment
