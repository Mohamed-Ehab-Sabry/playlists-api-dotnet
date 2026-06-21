# Playlist Service API

A robust REST API built with **ASP.NET 10** to manage user playlists and songs. The service is architected using **Clean Architecture** principles and the **SOLID** design philosophy to ensure maintainability, testability, and clear separation of concerns.

---

## 🏗️ Architecture Overview

The solution is split into four decoupled projects:

| Layer | Project | Responsibility |
|---|---|---|
| Domain | `PlaylistService.Domain` | Entities, domain logic — zero external dependencies |
| Application | `PlaylistService.Application` | Use-case orchestration, repository interfaces |
| Infrastructure | `PlaylistService.Infrastructure` | EF Core `DbContext`, migrations, repository implementations |
| API | `PlaylistService.Api` | Thin controllers, DTOs, DI wiring |

---

## 🛠️ Tech Stack

| Concern | Choice | Reason |
|---|---|---|
| Framework | ASP.NET Core 10 | Latest LTS-aligned release; minimal API surface |
| Database | **SQLite** (via EF Core) | Zero-config, single-file DB — ideal for portable assessment submission; swap to PostgreSQL/SQL Server by changing the connection string and provider package |
| ORM | Entity Framework Core 10 | Code-first migrations, LINQ, strong .NET integration |
| Testing | xUnit + Moq + FluentAssertions | Industry-standard unit + integration testing |

> **Why SQLite?** 
> First of all, since the data is well-structured with known properties, a relational database is a natural fit. Because the business requires a quickly runnable, "anywhere" setup, SQLite was the go-to option. It requires no server installation, making the project runnable on any machine immediately after cloning—which directly satisfies the core requirement for this task. A better option for production would be MS SQL Server or PostgreSQL. However, because we utilized the Repository Pattern and EF Core abstractions, swapping to an enterprise database engine requires only a single configuration change, with zero impact on the business logic.
---

## 🚀 How to Run

### Prerequisites

| Tool | Version | Install |
|---|---|---|
| .NET SDK | **10.0** or later | https://dotnet.microsoft.com/download |
| Git | Any recent version | https://git-scm.com |

No database server, Docker, or IDE is required.

---

### Step 1 — Clone the repository

```bash
git clone <your-repo-url>
cd playlists-api-dotnet/PlaylistService
```

---

### Step 2 — Restore dependencies

```bash
dotnet restore
```

---

### Step 3 — Run the API

```bash
dotnet run --project PlaylistService.Api
```

On first launch the application will:
1. Create `playlist.db` (SQLite file) in the API project directory.
2. Apply all EF Core migrations automatically (`Database.MigrateAsync`).
3. Seed the database with sample songs via `DbSeeder`.

The API will be available at:
- **HTTP:** `http://localhost:5121`
- **HTTPS:** `https://localhost:7263`
- **OpenAPI spec:** `http://localhost:5121/openapi/v1.json` *(Development only)*

---

### Step 4 — Make your first request

The API uses a `UserId` HTTP header to identify the caller (simulating auth without a full auth server).

**Create a playlist:**

```bash
curl -X POST http://localhost:5121/api/playlists \
  -H "Content-Type: application/json" \
  -H "UserId: 00000000-0000-0000-0000-000000000001" \
  -d '{"name": "My First Playlist"}'
```

**Fetch playlists:**

```bash
curl http://localhost:5121/api/playlists \
  -H "UserId: 00000000-0000-0000-0000-000000000001"
```

**Add a song** (use a real Song ID returned by the seeder, or check the DB):

```bash
curl -X POST http://localhost:5121/api/playlists/{playlistId}/songs \
  -H "Content-Type: application/json" \
  -H "UserId: 00000000-0000-0000-0000-000000000001" \
  -d '{"songId": "<song-guid-here>"}'
```

**Update playlist name:**

```bash
curl -X PATCH http://localhost:5121/api/playlists/{playlistId} \
  -H "Content-Type: application/json" \
  -H "UserId: 00000000-0000-0000-0000-000000000001" \
  -d '{"name": "Renamed Playlist"}'
```

**Delete a playlist:**

```bash
curl -X DELETE http://localhost:5121/api/playlists/{playlistId} \
  -H "UserId: 00000000-0000-0000-0000-000000000001"
```

**Remove a song from a playlist:**

```bash
curl -X DELETE http://localhost:5121/api/playlists/{playlistId}/songs/{songId} \
  -H "UserId: 00000000-0000-0000-0000-000000000001"
```

---

### Step 5 — Run the tests

**Unit tests** (no network or DB required):

```bash
dotnet test PlaylistService.UnitTests
```

**Integration tests** (spins up an in-process test server using `WebApplicationFactory`):

```bash
dotnet test PlaylistService.IntegrationTests
```

**All tests at once:**

```bash
dotnet test
```

---

### Resetting the database

Simply delete `PlaylistService.Api/playlist.db` and restart the API — migrations and seeding run automatically.

---

## 📡 API Reference

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/playlists` | Create a new playlist |
| `GET` | `/api/playlists` | Get all playlists for the calling user |
| `PATCH` | `/api/playlists/{id}` | Update the name of a playlist |
| `DELETE` | `/api/playlists/{id}` | Soft-delete a playlist |
| `POST` | `/api/playlists/{id}/songs` | Add a song to a playlist |
| `DELETE` | `/api/playlists/{id}/songs/{songId}` | Remove a song from a playlist |

All endpoints require the `UserId` header (a valid GUID).

---

## 🗂️ Project Structure

```
PlaylistService/
├── PlaylistService.Api/             # Controllers, DTOs, Program.cs
├── PlaylistService.Application/     # Interfaces, use-case services
├── PlaylistService.Domain/          # Entities, domain rules
├── PlaylistService.Infrastructure/  # EF Core, migrations, repositories
├── PlaylistService.UnitTests/       # xUnit + Moq unit tests
├── PlaylistService.IntegrationTests/# xUnit + WebApplicationFactory tests
└── PlaylistService.slnx             # Solution file
```