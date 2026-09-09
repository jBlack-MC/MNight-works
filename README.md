# MNight-works

Last updated: 2026-09-09

Recent changes
--------------
- Added simple JWT authentication (AuthController) with register/login endpoints.
- Introduced a User model and registered Users DbSet in the EF Core AppDbContext.
- Linked Restaurant to an owner via OwnerId and a JsonIgnored Owner reference to avoid JSON cycles.
- Added migrations to create the Users table.
- Updated wwwroot/script.js to use restaurant-specific endpoints (hardcoded restaurantId = 2).

Hybrid solution with an ASP.NET Core backend and a .NET MAUI mobile client.

Overview
--------
This repository contains a backend Web API and a cross-platform mobile client:

- MNight-works: ASP.NET Core Web API project
- MNightWorks.Shared: shared models used by both backend and client
- MNightWorks.Mobile: .NET MAUI mobile application (Android, iOS, MacCatalyst, Windows)

Summary
-------
- Backend: ASP.NET Core Web API (project: MNight-works)
- Shared models: MNightWorks.Shared
- Mobile client: .NET MAUI app (project: MNightWorks.Mobile)

Prerequisites
-------------
- .NET 10 SDK
- Visual Studio 2026 with .NET MAUI and ASP.NET workloads
- Platform toolchains for target platforms (Android SDK, Xcode/macOS for iOS/MacCatalyst, Windows desktop workload)


Quick start (Visual Studio)
---------------------------
1. Open MNight-works.slnx in Visual Studio 2026.
2. Restore NuGet packages if prompted.
3. Set the startup project to MNight-works and run to start the API.
4. Start MNightWorks.Mobile from Visual Studio targeting an emulator or device.

CLI: build & run backend
------------------------
dotnet restore "MNight-works.slnx"
dotnet build "MNight-works.slnx"
dotnet run --project "MNight-works"

Connecting the mobile client
----------------------------
Configure the API endpoint used by the mobile app so it can reach the backend. For local development prefer running the backend and the mobile client on the same machine or use a reachable network endpoint (emulator host loopback varies by platform).

Configuration and secrets
-------------------------
- Do not commit real credentials to source control. The file MNight-works/appsettings.json contains a placeholder connection string.
- For local development use dotnet user-secrets (recommended) or environment variables. Example:

  dotnet user-secrets init
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=...;Database=...;Username=...;Password=..."

Platform notes
--------------
- The MAUI project is easiest to run from Visual Studio because emulators and device deployment are integrated.
- When testing mobile-to-local API calls, verify emulator networking rules: Android emulators may use 10.0.2.2 (emulator) to reach the host, Windows apps can use localhost, etc.

Local data and database files
----------------------------
- If using local SQLite files, avoid committing WAL/SHM files. The repository may contain development artifacts (e.g., resturant.db-wal). Remove or add these to .gitignore if appropriate.

Repository structure
--------------------
- MNight-works/           Backend web API project
- MNightWorks.Shared/     Shared models used by backend and client
- MNightWorks.Mobile/     .NET MAUI mobile application

Contributing
------------
Contributions are welcome. Please open PRs against master. Add a LICENSE file if you plan to change licensing.

Contact
-------
For project questions, open an issue in the repository.

Security: connection strings and secrets
--------------------------------------
- Do not commit real database credentials or connection strings to source control. The backend appsettings.json contains a placeholder for DefaultConnection.
- Use dotnet user-secrets for local development or environment variables in production. Example (in project directory):

  dotnet user-secrets init
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=...;Database=...;Username=...;Password=..."


Ignoring local database artifacts
--------------------------------
If you use a local SQLite database for development, exclude temporary WAL/SHM files from the repo. Add these lines to .gitignore if they are not already present:

  # SQLite temporary files
  *.db-wal
  *.db-shm

Recommended next steps
----------------------
- Add your Postgres connection string to user-secrets or environment variables rather than appsettings.json.
- Remove any committed WAL/SHM files from the repository history if they contain sensitive data using git rm and then commit.

