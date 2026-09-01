# Hero Missions

A small full-stack demo built for interview purposes: dispatch heroes on missions and watch the
outcome roll in live.

- **Backend**: ASP.NET Core (.NET 10), vertical-slice architecture (MediatR + FluentValidation),
  Entity Framework Core over an in-memory SQLite database, Hangfire for background job scheduling,
  and SignalR for real-time notifications.
- **Frontend**: Angular 22 (standalone components), Angular Material, Tailwind CSS, and the CDK
  drag-and-drop module for assigning heroes to a mission.

## Domain

- **Hero** — a pre-seeded roster of original characters, each with five attributes: Strength,
  Speed, Intelligence, Durability and Energy.
- **Mission** — has a name, description, a set of required attribute totals, and a duration. Missions
  can be created, edited and deleted while `Pending`.
- Assigning one or more heroes to a mission moves it to `InProgress`, persists the assignment, and
  schedules a Hangfire job to run after the mission's duration elapses. That job sums the assigned
  heroes' attributes, compares them against the requirements, marks the mission `Succeeded` or
  `Failed`, and pushes a SignalR notification to every connected client.
- A recurring background job generates a brand new mission with randomized requirements every couple
  of minutes, so the mission list stays alive even without user interaction.

## Running with Docker (recommended)

Requires Docker Desktop (or another Docker engine) with Compose v2.

```bash
docker compose up --build
```

Then open **http://localhost:4200**. The frontend is served by nginx, which reverse-proxies
`/api/*` and `/hubs/*` to the backend container, so there's nothing else to configure.

The database is SQLite running entirely in memory - it is seeded with heroes on startup and is
**not persisted**; stopping the containers wipes all mission data, by design, so every run starts
from a clean slate.

## Running locally without Docker

### Backend

```bash
cd src/backend
dotnet run --project Interview.API
```

The API listens on the URL printed in the console (see `Interview.API/Properties/launchSettings.json`).
The Hangfire dashboard is available at `/hangfire` in the Development environment.

### Frontend

```bash
cd src/frontend
npm install
npm start
```

The dev server runs at http://localhost:4200 and talks to the API at the URL configured in
`src/environments/environment.development.ts` (defaults to `http://localhost:5299`).

## Project layout

```
src/backend/
  Interview.Domain          - entities & enums (Hero, Mission, MissionHero, MissionStatus)
  Interview.Application     - vertical-slice commands/queries (MediatR), DTOs, IAppDbContext
  Interview.Infrastructure  - EF Core DbContext, hero seed data, Hangfire jobs, SignalR hub
  Interview.API             - minimal API endpoints (one file per feature), Program.cs wiring
src/frontend/
  src/app/core              - models & HTTP/SignalR services
  src/app/features          - mission-list, mission-form-dialog, mission-assign (drag & drop)
```
