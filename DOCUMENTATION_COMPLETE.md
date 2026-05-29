# Guía Rápida - Blessings & Curses

## 🚀 Inicio Rápido

```bash
# Ejecutar la aplicación
cd "MVC Web App"
dotnet run

# Abrir en navegador
http://localhost:5141
```

## 🐳 Docker

```bash
# Build y ejecución
docker-compose up -d

# Ver logs
docker-compose logs -f app

# Detener
docker-compose down
```

## 🧪 Tests

```bash
# Ejecutar todos los tests
dotnet test

# Con coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📁 Estructura de Archivos (Fases 2-4)

```
PF-ParadigmasProgramacion/
├── 📄 CHANGELOG.md                    # Registro completo de cambios
├── 📄 DOCUMENTATION_COMPLETE.md       # Esta documentación
│
├── Class Library/
│   ├── Clases/
│   │   ├── Carta.cs                 # Clase base (null safety fixes)
│   │   ├── CartaJuego.cs           # + switch expression
│   │   ├── CartaPremio.cs
│   │   ├── CartaCastigo.cs
│   │   ├── Jugador.cs               # Null safety
│   │   ├── Juego.cs                 # Random.Shared, switch expression
│   │   ├── Baraja.cs                # Async I/O, AppContext.BaseDirectory
│   │   └── Historial.cs
│   ├── Eventos/
│   │   ├── Publisher_Eventos_Juego.cs    # Eventos nullable
│   │   ├── Publisher_Eventos_Jugador.cs   # Eventos nullable
│   │   └── Publisher_Eventos_Cartas.cs   # Eventos nullable
│   ├── Aspectos/
│   │   ├── InterceptorCargaArchivo.cs     # ILogger
│   │   └── InterceptorValidacion.cs       # ILogger
│   └── Interfaces/
│       ├── IJuegoService.cs         # Removido duplicado
│       └── IHomeService.cs          # + GenerarToken()
│
├── MVC Web App/
│   ├── Controllers/
│   │   ├── HomeController.cs        # Primary constructor, JWT
│   │   ├── JuegoController.cs       # Primary constructor, ILogger, record CartaRevelada
│   │   └── ReglasController.cs      # Primary constructor, IJuegoService DI, ILogger
│   ├── Services/
│   │   ├── HomeService.cs           # BCrypt, EF Core, JWT generation
│   │   ├── JuegoService.cs          # Primary constructor
│   │   ├── ReglasService.cs         # IMemoryCache, primary constructor
│   │   └── LeaderboardService.cs    # NUEVO - Leaderboard
│   ├── Data/
│   │   └── AppDbContext.cs          # NUEVO - EF Core context
│   ├── Entities/
│   │   ├── Usuario.cs               # NUEVO
│   │   ├── Partida.cs               # NUEVO
│   │   └── Estadistica.cs           # NUEVO
│   └── Program.cs                   # Serilog, JWT, EF Core, Health Checks, MemoryCache
│
├── Tests/CL_ProyectoFinalPOO.Tests/
│   ├── CartaJuegoTests.cs           # 10 tests
│   ├── CartaPremioTests.cs          # 9 tests
│   ├── CartaCastigoTests.cs         # 9 tests
│   ├── JugadorTests.cs              # 14 tests
│   ├── JuegoTests.cs                # 17 tests
│   ├── BarajaTests.cs               # 5 tests
│   └── BarajaAsyncTests.cs          # 6 tests (NUEVO)
│
├── Dockerfile                       # NUEVO - Multi-stage build
├── docker-compose.yml               # NUEVO
└── .github/workflows/
    └── ci-cd.yml                   # NUEVO - GitHub Actions
```

## 🔑 APIs y Endpoints

### Health Checks
- `GET /health` - Estado general
- `GET /health/ready` - Verifica DB SQLite

### Autenticación (JWT)
- Tokens generados en Login/Signup
- Almacenados en sesión como `JwtToken`

### Leaderboard
```csharp
// Usage en servicios
var top10 = await leaderboardService.GetTopPlayersAsync(10);
var miPosicion = await leaderboardService.GetPlayerRankAsync("Nickname");
await leaderboardService.ActualizarEstadisticasAsync("Nickname", puntos, esGanador);
```

## ⚙️ Configuración

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  },
  "Jwt": {
    "Key": "TuClaveSecretaDeAlMenos32Caracteres!",
    "Issuer": "BlessingsAndCurses",
    "Audience": "BlessingsAndCursesUsers",
    "ExpiryMinutes": 60
  }
}
```

## 🏗️ Tecnologías por Fase

| Fase | Tecnología |
|------|-----------|
| Fase 1 | Encoding UTF-8 |
| Fase 2 | xUnit, Moq, ILogger, Nullable Reference Types |
| Fase 3 | .NET 10, C# 12 (Primary Constructors, Switch Expressions, Records) |
| Fase 4 | BCrypt, JWT, EF Core, SQLite, Serilog, Docker, GitHub Actions |

## 📊 Métricas del Proyecto

| Métrica | Valor |
|---------|-------|
| Tests | 70+ |
| Clases/Interfaces | 26 |
| Líneas de código (aprox) | 3000+ |
| Warnings (antes) | 36 |
| Warnings (después) | ~16 |
| Errores compilación | 0 |
| Proyectos | 3 (Class Library, MVC App, Tests) |

## 🐛 Debugging

```bash
# Ver logs de la aplicación
tail -f logs/app-*.log

# Ver logs de Docker
docker-compose logs -f app

# Resetear base de datos SQLite
rm MVC\ Web\ App/app.db
```

## 🔄 Próximos Pasos Sugeridos

1. **XML Documentation** - Añadir `<summary>` a todos los métodos públicos
2. **Integrar LeaderboardService** en views
3. **Persistencia real de partidas** - Guardar al FinalizarJuego
4. **Docker deployment** a producción
5. **CI/CD** - Configurar GitHub Secrets para Docker Hub
