# 📝 ChangeLog - Blessings & Curses

## V 1.30 (Mayo 7 2025) - Chanda

- Cambié en la clase Baraja para manejar 3 listas diferentes dependiendo el tipo de carta (Juego, Castigo, Premio)
- Ahora la clase Resto solo incluye las cartas de tipo Juego, las de Castigo y Premio se manejarán con listas aparte en la clase Juego
- Cambié el método AplicarEfectoCartas() para que imprima mejor los puntos obtenidos en esa carta y los puntos totales del jugador
- Creé las listas l_cartas_premio y l_cartas_castigo en la clase Juego, estas se rellenan en el constructor con los nuevos métodos de la clase Baraja (CrearCartasCastigo() y CrearCartasPremio())
- Cambié el método BarajarCartas() para que ahora no solo revuelva las cartas de resto sino también las de Premio y Castigo
- Cambié el constructor de Resto para evitar quemar valores
- Cambié la clase Baraja y le agregué otro método que obtiene una lista con todas las cartas, algo similar a como antes funcionaba el Resto
- Añadí nuevos y modifiqué métodos y listas en la clase Juego, también cambié el constructor
- El método ObtenerCarta() pasó de estar en Resto a estar en Juego ya que al cambiar el funcionamiento de Resto quedaba obsoleto el método
- Modifiqué el funcionamiento del método BarajarCartas()
- Cambié los accesores de Carta por unos menos redundantes y mejor estructurados, similares a los que se hicieron en el proyecto del Multiplex
- Eliminé la interfaz ICartaEfecto ya que no estaba programada y por el momento sobraba, el método ActualizarPuntos(); que estaba en cada carta lo eliminé ya que no tenía estructura ni funcionalidad
- Transferí el método de AplicarEfectoCartas de Jugador a Juego para intentar tener todos los métodos que regulen el funcionamiento del juego en la clase Juego
- Creé nuevos atributos estáticos en cada carta para dejar ahí el valor que van a sumar o restar y evitar quemar valores en los métodos que usen esos valores
- Añadí el método para repartir las cartas iniciales en cada juego, el método simplemente saca las 3 primeras cartas de la baraja actual y las entrega a un jugador
- Añadí atributos de reglas de negocio en el juego como número máximo de jugadores y cartas por jugador
- Cambié el accesor de puntos ya que estaba haciendo que nadie pudiera pasar de 80 puntos
- Hice mucha corrección de errores

---

## V 1.34 (Mayo 9 2025) - Chanda

- Creé un par de interfaces con los métodos ya creados para ir definiendo mejor la biblioteca
- A todos los métodos les creé bloque Try Catch para mejor control de errores y mejorar el cumplimiento de requisitos
- Creé la carpeta de Eventos para encaminar la creación de los eventos

---

## V 1.40 (Mayo 17 2025) - Chanda

- Empecé a implementar en el MVC, fueron avances cortos pero significativos en la parte del front del Home, ya se ve bonito
- Los botones ya están bien pero aún no son funcionales, falta hacer el controller
- Corregí la NavBar que estaba terriblemente fea

---

## V 1.50 (Mayo 29 2026) - Chanda

### Fase 2: Testing, Null Safety y Logging

**Tests Unitarios:**
- Creados 7 archivos de test con 70+ tests utilizando xUnit y Moq
- Implementados tests para: CartaJuegoTests, CartaPremioTests, CartaCastigoTests, JugadorTests, JuegoTests, BarajaTests, BarajaAsyncTests

**Corrección de Warnings Null Safety:**
- Corregidos 36+ warnings CS8618/CS8602 en toda la Class Library
- Añadido operador null-forgiving `= null!` en campos de clases
- Convertidos eventos Publisher a formato nullable `event EventHandler<T>?`

**Refactorización de Logging:**
- Reemplazado Console.WriteLine con ILogger en InterceptorCargaArchivo e InterceptorValidacion
- Añadido paquete Microsoft.Extensions.Logging.Abstractions

**Mejoras de Código:**
- Removido modificador `public` redundante en IJuegoService
- Removido método duplicado TotalCartasEnMazo en IJuegoService
- Removidos bloques else vacíos en JuegoService
- Mejorado manejo de excepciones usando discard pattern para variables no usadas

### Fase 3: Modernización .NET 10

**Upgrade de .NET:**
- Actualizado de .NET 8.0 a .NET 10.0 en los 3 proyectos (Class Library, MVC App, Tests)
- Añadido AllowMissingPrunePackageData=true para compatibilidad con ASP.NET Core 10

**Implementación Async/Await:**
- Creado método CargarCartasAsync en Baraja usando File.ReadAllTextAsync
- Actualizado CargarCartas para llamar a versión async
- Cambiado AppDomain.CurrentDomain.BaseDirectory por AppContext.BaseDirectory

**Thread-Safety:**
- Reemplazado Random instance por Random.Shared en Juego.cs (7 usages)

**Características C# 12:**
- Implementados primary constructors en HomeService, JuegoService, ReglasService
- Implementados primary constructors en HomeController, JuegoController, ReglasController
- Convertidos switch statements a switch expressions en CartaJuego y Juego
- Creado record CartaRevelada para DTOs en JuegoController

**Limpieza de Código:**
- Removido import Microsoft.VisualBasic de Juego.cs

**Mejoras de Inyección de Dependencias:**
- ReglasController ahora inyecta IJuegoService en lugar de implementación concreta

### Fase 4: Seguridad y Autenticación

**Hash de Contraseñas:**
- Implementado BCrypt.Net-Next para hashing de contraseñas en HomeService

**Autenticación JWT:**
- Añadido JWT Bearer authentication en Program.cs
- Añadido método GenerarToken en IHomeService y HomeService
- Configurados TokenValidationParameters con issuer, audience y signing key

**Refuerzo de Sesión:**
- Añadido Cookie.SecurePolicy.SameAsRequest
- Configuradas Data Protection keys con lifetime de 90 días

### Fase 4: Persistencia de Datos

**Entity Framework Core + SQLite:**
- Creado AppDbContext.cs con configuración de entidades
- Creadas entidades: Usuario, Partida, Estadistica
- Implementado unique index en Usuario.Nickname
- Configuradas relaciones: Usuario(1:1)Estadistica, Usuario(1:N)Partidas

### Fase 4: Deployment y Contenedores

**Docker:**
- Creado Dockerfile multi-stage (SDK 10.0 → ASPNET 10.0, puerto 8080)
- Implementado health check con curl
- Creado docker-compose.yml con health checks y volúmenes persistentes

**CI/CD:**
- Creado pipeline GitHub Actions con jobs: build, docker, deploy, code-quality

### Fase 4: Observabilidad

**Logging Estructurado:**
- Integración de Serilog con sink de consola y archivo rotativo
- Logs diarios en directorio logs/

**Health Checks:**
- Endpoint /health para estado general
- Endpoint /health/ready para verificar conexión SQLite

**Reemplazo de Debug.WriteLine:**
- Convertidos 8 usages de Debug.WriteLine a ILogger.LogError en controladores

### Fase 4: Features y Performance

**Leaderboard:**
- Creado LeaderboardService con métodos: GetTopPlayersAsync, GetPlayerRankAsync, ActualizarEstadisticasAsync
- Creado record LeaderboardEntry

**Caching:**
- Implementado IMemoryCache con TTL de 30 minutos para baraja
- Keys de cache: CartasJuego, CartasPremio, CartasCastigo

### Paquetes NuGet Añadidos

- BCrypt.Net-Next 4.0.3
- Microsoft.AspNetCore.Authentication.JwtBearer 10.0.0
- Microsoft.EntityFrameworkCore.Sqlite 10.0.0
- Microsoft.EntityFrameworkCore.Design 10.0.0
- Serilog.AspNetCore 8.0.0
- Serilog.Sinks.Console 6.0.0
- Serilog.Sinks.File 6.0.0
- AspNetCore.HealthChecks.Sqlite 8.0.0

### Infraestructura

- Corregido DI: HomeService y JuegoService cambiados de Singleton a Scoped
- Asegurado que AppDbContext se inyecta correctamente como scoped

---

**Ver DOCUMENTATION_COMPLETE.md para documentación detallada de todos los cambios.**