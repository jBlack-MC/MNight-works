# MNight-works

Hybrid solution with an ASP.NET Core backend and a .NET MAUI mobile client.

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
3. Start the backend (MNight-works) to run the API.
4. Start the mobile client (MNightWorks.Mobile) on an emulator or device.

CLI: build & run backend
------------------------
dotnet restore "MNight-works.slnx"
dotnet build "MNight-works.slnx"
dotnet run --project "MNight-works"

Connecting the mobile client
----------------------------
- Configure API endpoint URLs used by the mobile app before running so the client can reach the backend (see configuration files or source constants in MNightWorks.Mobile).
- Prefer running the MAUI client from Visual Studio for emulator/device deployment.

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
- If you use a local SQLite database for development, exclude temporary WAL/SHM files from the repo. Add these lines to .gitignore if they are not already present:

  # SQLite temporary files
  *.db-wal
  *.db-shm

Recommended next steps
----------------------
- Add your Postgres connection string to user-secrets or environment variables rather than appsettings.json.
- Remove any committed WAL/SHM files from the repository history if they contain sensitive data using git rm and then commit.

