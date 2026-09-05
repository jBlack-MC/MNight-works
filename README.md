# MNight-works

Hybrid solution containing an ASP.NET Core backend and a .NET MAUI mobile client.

Summary
-------
- Backend: ASP.NET Core web API (project: MNight-works)
- Shared models: MNightWorks.Shared
- Mobile client: .NET MAUI app (project: MNightWorks.Mobile)

Prerequisites
-------------
- .NET 10 SDK
- Visual Studio 2026 with Mobile development (.NET MAUI) and ASP.NET workloads
- Platform toolchains for target platforms (Android SDK, Xcode/macOS for iOS/MacCatalyst, Windows desktop workload if targeting Windows)

Quick start (recommended)
-------------------------
1. Open the solution file MNight-works.slnx in Visual Studio 2026.
2. Restore NuGet packages if prompted.
3. Set the startup project(s): choose the backend project (MNight-works) to run the API and the MAUI project (MNightWorks.Mobile) to run the mobile client.
4. Use Visual Studio to select a target (e.g., Android emulator, Windows) and Run (F5) or Debug.

CLI (build & run backend)
-------------------------
1. Restore and build solution:

   dotnet restore "MNight-works.slnx"
   dotnet build "MNight-works.slnx"

2. Run the web API (from repo root):

   dotnet run --project "MNight-works"

Notes about the MAUI client
---------------------------
- The MAUI project is intended to be launched from Visual Studio because platform emulators and device deployment are best managed there. If you prefer CLI workflows, follow the official .NET MAUI documentation for platform-specific commands and tooling.
- App configuration (API endpoints, connection strings) is typically configured in the mobile project or via resources. Update these values before running the mobile client so it can reach the backend API.

Repository structure
--------------------
- MNight-works/               Backend web API project
- MNightWorks.Shared/         Shared models used by backend and client
- MNightWorks.Mobile/         .NET MAUI mobile application

Contributing
------------
Contributions, fixes and improvements are welcome. Please open pull requests against the master branch.

License
-------
This repository does not include an explicit license file. Add a LICENSE file if you intend to set one.
