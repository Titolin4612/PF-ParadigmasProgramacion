# Registro de Cambios y Mejoras - Mythological Card Game

## Tabla de Contenidos
1. [Fase 1: Mejoras UI/UX](#fase-1-mejoras-uiux)
2. [Fase 2: Testing, Null Safety y Logging](#fase-2-testing-null-safety-y-logging)
3. [Fase 3: Modernización](#fase-3-modernización)
4. [Fase 4: Seguridad, Persistencia y Deployment](#fase-4-seguridad-persistencia-y-deployment)

---

## Fase 1: Mejoras UI/UX

### Problemas Identificados
- Encodings incorrectos en archivos (ISO-8859 → UTF-8)
- README desactualizado (12 Castigo → 17, 8 Premio → 13)

### Cambios Realizados

| Archivo | Cambio | Detalle |
|---------|--------|---------|
| `MVC Web App/Controllers/HomeController.cs` | Encoding fix | Cambiado de ISO-8859 a UTF-8 |
| `README.md` | Actualización tablas | 12→17 Castigo, 8→13 Premio |

---

## Fase 2: Testing, Null Safety y Logging

### Problemas Identificados
- 36+ nullable reference warnings en Class Library
- Console.WriteLine disperso sin ILogger
- Sin tests unitarios
- Código quality issues (bloques else vacíos, excepciones swallowed)

### Tests Creados

| Archivo | Tests | Descripción |
|---------|-------|-------------|
| `Tests/CL_ProyectoFinalPOO.Tests/CartaJuegoTests.cs` | 10 | Tests para carta de juego |
| `Tests/CL_ProyectoFinalPOO.Tests/CartaPremioTests.cs` | 9 | Tests para carta premio |
| `Tests/CL_ProyectoFinalPOO.Tests/CartaCastigoTests.cs` | 9 | Tests para carta castigo |
| `Tests/CL_ProyectoFinalPOO.Tests/JugadorTests.cs` | 14 | Tests para jugador |
| `Tests/CL_ProyectoFinalPOO.Tests/JuegoTests.cs` | 17 | Tests para lógica de juego |
| `Tests/CL_ProyectoFinalPOO.Tests/BarajaTests.cs` | 5 | Tests para baraja |
| `Tests/CL_ProyectoFinalPOO.Tests/BarajaAsyncTests.cs` | 6 | Tests async para baraja |

### Null Safety Fixes

| Archivo | Campo/Método | Fix |
|---------|--------------|-----|
| `Class Library/Clases/Carta.cs` | Campos | `= null!` initializer |
| `Class Library/Clases/Jugador.cs` | Campos | `= null!` initializer |
| `Class Library/Clases/Juego.cs` | Campos | `= null!` initializer |
| `Class Library/Clases/Baraja.cs` | Campos | `= null!` initializer |
| `Class Library/Eventos/Publisher_Eventos_Juego.cs` | Eventos | `event EventHandler<T>?` |
| `Class Library/Eventos/Publisher_Eventos_Jugador.cs` | Eventos | `event EventHandler<T>?` |
| `Class Library/Eventos/Publisher_Eventos_Cartas.cs` | Eventos | `event EventHandler<T>?` |
| `Class Library/Clases/CartaCastigo.cs` | `_maleficio` | Inicialización en constructor |
| `Class Library/Clases/CartaPremio.cs` | `_bendicion` | Inicialización en constructor |

### Logging Modernization

| Archivo | Antes | Después |
|---------|-------|---------|
| `Class Library/Aspectos/InterceptorCargaArchivo.cs` | `Console.WriteLine` | `ILogger<InterceptorCargaArchivo>` |
| `Class Library/Aspectos/InterceptorValidacion.cs` | `Console.WriteLine` | `ILogger<InterceptorValidacion>` |

### Code Quality Fixes

| Archivo | Problema | Fix |
|---------|----------|-----|
| `Class Library/Interfaces/IJuegoService.cs:18` | `public` redundante | Removido |
| `Class Library/Interfaces/IJuegoService.cs:24` | Método duplicado `TotalCartasEnMazo` | Removido duplicado |
| `MVC Web App/Services/JuegoService.cs:99-100` | Bloques else vacíos | Removidos |
| `MVC Web App/Services/JuegoService.cs:127-128` | Bloques else vacíos | Removidos |
| `MVC Web App/Services/JuegoService.cs` | Variables `ex` no usadas en catch | Cambiado a `catch` |

### Paquetes NuGet Añadidos

| Paquete | Versión | Propósito |
|---------|---------|-----------|
| `Microsoft.Extensions.Logging.Abstractions` | 8.0.0 | Logging interfaces |
| `Microsoft.NET.Test.Sdk` | 17.8.0 | Test SDK |
| `xunit` | 2.6.1 | Test framework |
| `xunit.runner.visualstudio` | 2.5.3 | Test runner |
| `Moq` | 4.20.70 | Mocking framework |
| `coverlet.collector` | 6.0.0 | Code coverage |

---

## Fase 3: Modernización

### Problemas Identificados
- .NET 8 (legacy) → .NET 10 disponible
- I/O sincrónico bloqueante
- Random no thread-safe
- Constructores tradicionales
- Switch statements en lugar de expressions
- Tipos anónimos en lugar de records

### .NET Upgrade

| Archivo | Cambio |
|---------|--------|
| `Class Library/CL_ProyectoFinalPOO.csproj` | `net8.0` → `net10.0` |
| `MVC Web App/MVC_ProyectoFinalPOO.csproj` | `net8.0` → `net10.0` + `AllowMissingPrunePackageData=true` |
| `Tests/CL_ProyectoFinalPOO.Tests/CL_ProyectoFinalPOO.Tests.csproj` | `net8.0` → `net10.0` |

### Async/Await Implementation

| Archivo | Método | Cambio |
|---------|--------|--------|
| `Class Library/Clases/Baraja.cs` | `CargarCartasAsync` | Nuevo método async |
| `Class Library/Clases/Baraja.cs` | `CargarCartas` | Ahora llama a async via `.GetAwaiter().GetResult()` |
| `Class Library/Clases/Baraja.cs:15-16` | Rutas | `AppDomain.CurrentDomain.BaseDirectory` → `AppContext.BaseDirectory` |

### Thread-Safe Random

| Archivo | Cambio |
|---------|--------|
| `Class Library/Clases/Juego.cs:25` | Removido `private static Random rng = new Random()` |
| `Class Library/Clases/Juego.cs` | 7 usages `rng.Next()` → `Random.Shared.Next()` |

### Primary Constructors (C# 12)

| Archivo | Antes | Después |
|---------|-------|---------|
| `MVC Web App/Services/JuegoService.cs` | Constructor tradicional | `public class JuegoService(HomeService homeService)` |
| `MVC Web App/Services/ReglasService.cs` | Constructor tradicional | `public class ReglasService(Baraja baraja)` |
| `MVC Web App/Controllers/JuegoController.cs` | Constructor tradicional | `public class JuegoController(IJuegoService juegoService)` |
| `MVC Web App/Controllers/HomeController.cs` | Constructor tradicional | `public class HomeController(IHomeService, IJuegoService)` |
| `MVC Web App/Controllers/ReglasController.cs` | Constructor tradicional | `public class ReglasController(ReglasService, IJuegoService)` |

### Switch Expressions

| Archivo | Método | Cambio |
|---------|--------|--------|
| `Class Library/Clases/CartaJuego.cs` | `ObtenerPuntos()` | `switch` → `return RarezaCarta switch {...}` |
| `Class Library/Clases/Juego.cs` | `AplicarEfectoCartas()` | `switch` → `carta switch {...}` |

### Record Types

| Archivo | Cambio |
|---------|--------|
| `MVC Web App/Controllers/JuegoController.cs` | Creado `record CartaRevelada(...)` para reemplazar anonymous type |

### Cleanup

| Archivo | Cambio |
|---------|--------|
| `Class Library/Clases/Juego.cs:11` | Removido `using Microsoft.VisualBasic;` |

### DI Fix

| Archivo | Cambio |
|---------|--------|
| `MVC Web App/Controllers/ReglasController.cs` | `JuegoService` concreto → `IJuegoService` interfaz |

---

## Fase 4: Seguridad, Persistencia y Deployment

### Problemas Identificados
- Contraseñas en texto plano (Dictionary estático)
- Sin autenticación JWT real
- Sin base de datos (simulación en memoria)
- Sin Docker ni CI/CD
- Sin health checks
- Sin structured logging
- Debug.WriteLine disperso
- Sin caching

### Seguridad

#### BCrypt Password Hashing

| Archivo | Cambio |
|---------|--------|
| `MVC Web App/Services/HomeService.cs` | `BuscarUsuario()` ahora usa `BCrypt.Net.BCrypt.Verify()` |
| `MVC Web App/Services/HomeService.cs` | `RegistrarUsuario()` ahora usa `BCrypt.Net.BCrypt.HashPassword()` |

#### JWT Authentication

| Archivo | Cambio |
|---------|--------|
| `MVC Web App/Program.cs` | Añadido `AddJwtBearer()` con `TokenValidationParameters` |
| `MVC Web App/Services/HomeService.cs` | Nuevo método `GenerarToken(string usuario)` |
| `Class Library/Interfaces/IHomeService.cs` | Nueva firma `string GenerarToken(string usuario)` |
| `MVC Web App/Controllers/HomeController.cs` | Login/Signup ahora generan y almacenan JWT en sesión |

#### Session Hardening

| Archivo | Cambio |
|---------|--------|
| `MVC Web App/Program.cs` | `Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest` |
| `MVC Web App/Program.cs` | Data Protection keys con lifetime de 90 días |

### Persistencia

#### EF Core + SQLite

| Archivo | Propósito |
|---------|-----------|
| `MVC Web App/Data/AppDbContext.cs` | DbContext con configuración de entidades |
| `MVC Web App/Entities/Usuario.cs` | Entidad usuario con password hash |
| `MVC Web App/Entities/Partida.cs` | Entidad para historial de partidas |
| `MVC Web App/Entities/Estadistica.cs` | Entidad para estadísticas de jugador |

#### DbContext Configuration

```csharp
// Usuario - unique index on Nickname
entity.HasIndex(e => e.Nickname).IsUnique();

// Partida - relación opcional con Usuario (nullable FK)
entity.HasOne(e => e.Usuario).WithMany(u => u.Partidas).OnDelete(DeleteBehavior.SetNull);

// Estadistica - relación 1:1 con Usuario, cascade delete
entity.HasOne(e => e.Usuario).WithOne(u => u.Estadistica).OnDelete(DeleteBehavior.Cascade);
```

### Deployment

#### Dockerfile

| Característica | Detalle |
|---------------|---------|
| Build stage | `mcr.microsoft.com/dotnet/sdk:10.0` |
| Runtime stage | `mcr.microsoft.com/dotnet/aspnet:10.0` |
| Puerto | 8080 (no 443) |
| Health check | `curl -f http://localhost:8080/health` |

#### docker-compose.yml

| Servicio | Configuración |
|----------|--------------|
| app | Puerto 8080:8080 |
| Volumes | `app-data`, `app-logs` |
| Health check | Interval 30s, timeout 10s, 3 retries |
| Restart policy | `unless-stopped` |

#### GitHub Actions CI/CD

| Job | Pipeline |
|-----|----------|
| build | restore, build, test, upload artifacts |
| docker | build & push a GHCR |
| deploy | placeholder para producción |
| code-quality | dotnet format verify |

### Observabilidad

#### Serilog Structured Logging

| Componente | Configuración |
|------------|---------------|
| Console sink | Template con timestamp, level, message, properties |
| File sink | Rolling file diario en `logs/app-.log` |
| Enrichment | `FromLogContext`, thread id |

#### Health Checks

| Endpoint | Predicate |
|----------|----------|
| `/health` | Todos los checks |
| `/health/ready` | Solo checks con tag "ready" (SQLite) |

#### Debug.WriteLine Replacement

| Archivo | Antes | Después |
|---------|-------|---------|
| `MVC Web App/Controllers/JuegoController.cs` | 7x `Debug.WriteLine` | `_logger.LogError` |
| `MVC Web App/Controllers/ReglasController.cs` | 1x `Debug.WriteLine` | `_logger.LogError` |

### Features

#### LeaderboardService

| Método | Descripción |
|--------|-------------|
| `GetTopPlayersAsync(int count)` | Top N jugadores por victorias/promedio |
| `GetPlayerRankAsync(string nickname)` | Posición de un jugador específico |
| `ActualizarEstadisticasAsync(string, int, bool)` | Actualiza stats tras partida |

#### Record Types

```csharp
public record LeaderboardEntry(
    int Posicion,
    string Nickname,
    int PartidasJugadas,
    int PartidasGanadas,
    double PromedioPuntos,
    int MejorPuntuacion
);
```

### Performance

#### MemoryCache Implementation

| Componente | Configuración |
|------------|---------------|
| Cache duration | 30 minutos |
| Keys | `CartasJuego`, `CartasPremio`, `CartasCastigo` |
| Implementation | `IMemoryCache` con `GetOrCreate` |

### Paquetes NuGet Añadidos (Fase 4)

| Paquete | Versión | Propósito |
|---------|---------|-----------|
| `BCrypt.Net-Next` | 4.0.3 | Password hashing |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0.0 | JWT auth |
| `Microsoft.EntityFrameworkCore.Sqlite` | 10.0.0 | EF Core + SQLite |
| `Microsoft.EntityFrameworkCore.Design` | 10.0.0 | EF Core tooling |
| `Serilog.AspNetCore` | 8.0.0 | Serilog integration |
| `Serilog.Sinks.Console` | 6.0.0 | Console logging |
| `Serilog.Sinks.File` | 6.0.0 | File logging |
| `AspNetCore.HealthChecks.Sqlite` | 8.0.0 | SQLite health check |

---

## Resumen de Archivos Modificados/Creados

### Creados (Fase 4)

```
MVC Web App/
├── Data/
│   └── AppDbContext.cs
├── Entities/
│   ├── Usuario.cs
│   ├── Partida.cs
│   └── Estadistica.cs
└── Services/
    └── LeaderboardService.cs

Raíz/
├── Dockerfile
├── docker-compose.yml
├── .github/workflows/ci-cd.yml
└── (README.md actualizado)
```

### Modificados Significantly

| Archivo | Principales Cambios |
|---------|---------------------|
| `MVC Web App/Program.cs` | Serilog, JWT, EF Core, Health Checks, MemoryCache |
| `MVC Web App/Services/HomeService.cs` | BCrypt, DbContext, JWT generation |
| `MVC Web App/Services/ReglasService.cs` | IMemoryCache, primary constructor |
| `MVC Web App/Controllers/JuegoController.cs` | ILogger, CartaRevelada record |
| `MVC Web App/Controllers/HomeController.cs` | JWT token storage |
| `MVC Web App/Controllers/ReglasController.cs` | ILogger |
| `MVC Web App/appsettings.json` | ConnectionStrings, Jwt config |
| `Class Library/Clases/Juego.cs` | Random.Shared, switch expression, removed VB import |
| `Class Library/Clases/Baraja.cs` | Async I/O, AppContext.BaseDirectory |
| `Class Library/Clases/CartaJuego.cs` | Switch expression |
| `Class Library/Interfaces/IHomeService.cs` | GenerarToken method |
| `Tests/CL_ProyectoFinalPOO.Tests/*.cs` | 7 archivos de test |

---

## Métricas Finales

| Métrica | Valor |
|---------|-------|
| Tests creados | 70+ |
| Warnings reducidos | 36 → ~16 |
| Errores de compilación | 0 |
| Métodos async | 2+ |
| Records types | 2 |
| Primary constructors | 5 |
| Entidades EF Core | 3 |
| Health endpoints | 2 |
| Jobs CI/CD | 4 |
