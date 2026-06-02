# 📝 ChangeLog - Blessings & Curses

## V 1.50 (May 29 2026) - Chanda

### Phase 5: Documentation and Licensing

**AGPL-3.0 License:**
- Created LICENSE file with GNU Affero General Public License v3.0-or-later
- Added copyright headers to 31 source files
- Updated README.md with license section and AGPL network use notice
- All dependencies (Castle.Core, EF Core, Serilog, etc.) verified AGPL-compatible
- Build passes with 0 errors

**Documentation:**
- Translated README.md from Spanish to English
- Translated CHANGELOG.md from Spanish to English

---

## V 1.50 (May 29 2026) - Chanda

### Phase 2: Testing, Null Safety and Logging

**Unit Tests:**
- Created 7 test files with 70+ tests using xUnit and Moq
- Implemented tests for: CartaJuegoTests, CartaPremioTests, CartaCastigoTests, JugadorTests, JuegoTests, BarajaTests, BarajaAsyncTests

**Null Safety Warning Fixes:**
- Fixed 36+ CS8618/CS8602 warnings across the entire Class Library
- Added null-forgiving operator `= null!` in class fields
- Converted Publisher events to nullable format `event EventHandler<T>?`

**Logging Refactoring:**
- Replaced Console.WriteLine with ILogger in InterceptorCargaArchivo and InterceptorValidacion
- Added Microsoft.Extensions.Logging.Abstractions package

**Code Improvements:**
- Removed redundant `public` modifier from IJuegoService
- Removed duplicate TotalCartasEnMazo method from IJuegoService
- Removed empty else blocks in JuegoService
- Improved exception handling using discard pattern for unused variables

### Phase 3: .NET 10 Modernization

**.NET Upgrade:**
- Updated from .NET 8.0 to .NET 10.0 across all 3 projects (Class Library, MVC App, Tests)
- Added AllowMissingPrunePackageData=true for ASP.NET Core 10 compatibility

**Async/Await Implementation:**
- Created CargarCartasAsync method in Baraja using File.ReadAllTextAsync
- Updated CargarCartas to call async version
- Changed AppDomain.CurrentDomain.BaseDirectory to AppContext.BaseDirectory

**Thread-Safety:**
- Replaced Random instance with Random.Shared in Juego.cs (7 usages)

**C# 12 Features:**
- Implemented primary constructors in HomeService, JuegoService, ReglasService
- Implemented primary constructors in HomeController, JuegoController, ReglasController
- Converted switch statements to switch expressions in CartaJuego and Juego
- Created record CartaRevelada for DTOs in JuegoController

**Code Cleanup:**
- Removed Microsoft.VisualBasic import from Juego.cs

**Dependency Injection Improvements:**
- ReglasController now injects IJuegoService instead of concrete implementation

### Phase 4: Security and Authentication

**Password Hashing:**
- Implemented BCrypt.Net-Next for password hashing in HomeService

**JWT Authentication:**
- Added JWT Bearer authentication in Program.cs
- Added GenerarToken method in IHomeService and HomeService
- Configured TokenValidationParameters with issuer, audience and signing key

**Session Reinforcement:**
- Added Cookie.SecurePolicy.SameAsRequest
- Configured Data Protection keys with 90-day lifetime

### Phase 4: Data Persistence

**Entity Framework Core + SQLite:**
- Created AppDbContext.cs with entity configuration
- Created entities: Usuario, Partida, Estadistica
- Implemented unique index on Usuario.Nickname
- Configured relationships: Usuario(1:1)Estadistica, Usuario(1:N)Partidas

### Phase 4: Deployment and Containers

**Docker:**
- Created multi-stage Dockerfile (SDK 10.0 → ASPNET 10.0, port 8080)
- Implemented health check with curl
- Created docker-compose.yml with health checks and persistent volumes

**CI/CD:**
- Created GitHub Actions pipeline with jobs: build, docker, deploy, code-quality

### Phase 4: Observability

**Structured Logging:**
- Integrated Serilog with console and rotating file sinks
- Daily logs in logs/ directory

**Health Checks:**
- Endpoint /health for overall status
- Endpoint /health/ready to verify SQLite connection

**Debug.WriteLine Replacement:**
- Converted 8 usages of Debug.WriteLine to ILogger.LogError in controllers

### Phase 4: Features and Performance

**Leaderboard:**
- Created LeaderboardService with methods: GetTopPlayersAsync, GetPlayerRankAsync, ActualizarEstadisticasAsync
- Created record LeaderboardEntry

**Caching:**
- Implemented IMemoryCache with 30-minute TTL for deck
- Cache keys: CartasJuego, CartasPremio, CartasCastigo

### NuGet Packages Added

- BCrypt.Net-Next 4.0.3
- Microsoft.AspNetCore.Authentication.JwtBearer 10.0.0
- Microsoft.EntityFrameworkCore.Sqlite 10.0.0
- Microsoft.EntityFrameworkCore.Design 10.0.0
- Serilog.AspNetCore 8.0.0
- Serilog.Sinks.Console 6.0.0
- Serilog.Sinks.File 6.0.0
- AspNetCore.HealthChecks.Sqlite 8.0.0

### Infrastructure

- Fixed DI: HomeService and JuegoService changed from Singleton to Scoped
- Ensured AppDbContext is correctly injected as scoped

---

**See DOCUMENTATION_COMPLETE.md for detailed documentation of all changes.**