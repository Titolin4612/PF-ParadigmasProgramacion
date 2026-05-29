
# 📝 ChangeLog
## Juego V 1.30 (Mayo 7 2025) SANTIAGO H

- Cambie en la clase Baraja para manejar 3 listas diferentes dependiendo el tipo de carta (Juego, Castigo, premio)
	
- Ahora la clase Resto solo incluye las cartas de tipo Juego, las de castigo y premio se manejarán con listas aparte en la clase juego
	
- Cambie el método AplicarEfectoCartas() para que imprima mejor los puntos obtenidos en esa carta y los puntos totales del jugador
	
- Cree las listas l_cartas_premio y l_cartas_castigo en la Clase juego, estas se rellenan en el constructor con los nuevos métodos de la clase Baraja ( CrearCartasCastigo() y CrearCartasPremio() )
	
- Cambie el método BarajarCartas() para que ahora no solo revuelva las cartas de resto si no también las de premio y castigo
	
- Cambie el constructor de Resto para evitar quemar valores
	
- Cambie durísimo la clase baraja y le agregue otro método que obtiene una lista con todas las cartas, algo similar a como antes funcionaba el Resto
	
- Añadí nuevos y modifiqué métodos y listas en la clase Juego, también cambié el constructor
	
- El método ObtenerCarta() pasó de estar en resto a estar en juego ya que al cambiar el funcionamiento de Resto quedaba obsoleto el método
	
- Modifiqué el funcionamiento del método BarajarCartas()
	
- Cambie los accesores de Carta por unos “Menos redundantes y mejor estructurados” similares a los que se hicieron en el proyecto del Multiplex
	
- Eliminé la interfaz ICartaEfecto ya que no estaba programada y por el momento sobraba, el método ActualizarPuntos(); que estaba en cada carta lo eliminé ya que no tenía estructura ni funcionalidad, luego se estructura mejor cada interfaz y métodos que llevaran
	
- Transferí el método de AplicarEfectoCartas de Jugador a Juego para intentar tener todos los métodos que regulen el funcionamiento del juego en la clase Juego, para eso modifique el funcionamiento para que devuelva un Int con el efecto de la carta
	
- También cree nuevos atributos estáticos en cada carta para dejar ahí el valor que van a sumar o restar y evitar quemar valores en los métodos que usen esos valores
	
- Añadí el método para repartir las cartas iniciales en cada juego, el método simplemente saca las 3 primeras cartas de la baraja actual y las entrega a un jugador y así repite hasta terminar con los jugadores
	
- Añadí también un par de atributos de reglas de negocio en el juego como numero max de jugadores y cartas por jugador
	
- Cambie el accesor de puntos ya que estaba haciendo que nadie pudiera pasar de 80 puntos, esta mal, hay que hacer una validación diferente si queremos controlar que al INICIAR los puntos estén en ese rango
	
- Hice mucha corrección de errores, falta revisar mucho pero creo que se logro avanzar bastante hoy, faltaron cosas por mencionar aquí ya que cada que me iba acordando iba poniendo, algunas se me pasaron
--- 
##  Juego V 1.34 (Mayo 9 2025) SANTIAGO H

- Cree un par de interfaces con los métodos ya creados para ir definiendo mejor la biblioteca

- A TODOS los métodos le cree bloque Try Catch para mejor control de errores y mejorar el cumplimiento de requisitos

- Cree la carpeta de Eventos para encaminar el creado de los eventos 
--- 
## Juego V 1.40 (Mayo 17 2025) SANTIAGO H
* Empecé a implementar en el MVC, Fueron avances cortos pero significativos en la parte del front de el Home, Ya se ve bonito al menos para la pantalla 2k y la 1080p. quizas en resoluciones mayores se pueda distorsionar un poco

* Los botones ya están bien pero aun no son funcionales, falta hacer el controller

* Corregí la NavBar que estaba terriblemente fea

---

## Cambios Comprehensivos (Mayo 2026) - opencode AI

### Fase 2: Testing, Null Safety y Logging

**Tests Unitarios:**
- Creados 7 archivos de test con 70+ tests (xUnit + Moq)
- CartaJuegoTests, CartaPremioTests, CartaCastigoTests, JugadorTests, JuegoTests, BarajaTests, BarajaAsyncTests

**Null Safety:**
- 36+ warnings CS8618/CS8602 corregidos con `= null!` y `?` nullable
- Eventos Publisher con `event EventHandler<T>?`

**Logging:**
- Console.WriteLine → ILogger en interceptores
- Microsoft.Extensions.Logging.Abstractions añadido

**Code Quality:**
- Removido `public` redundante en IJuegoService
- Removido método duplicado TotalCartasEnMazo
- Removidos bloques else vacíos en JuegoService
- Variables `ex` no usadas改为 discard pattern

### Fase 3: Modernización .NET 10

**.NET Upgrade:**
- net8.0 → net10.0 en 3 proyectos
- AllowMissingPrunePackageData=true para ASP.NET Core 10

**Async/Await:**
- Baraja.CargarCartasAsync() con File.ReadAllTextAsync
- AppDomain.CurrentDomain.BaseDirectory → AppContext.BaseDirectory

**Thread-Safety:**
- Random rng instance → Random.Shared (7 usages)

**C# 12 Modernizations:**
- Primary constructors en 5 classes (services + controllers)
- Switch expressions en CartaJuego.ObtenerPuntos() y Juego.AplicarEfectoCartas()
- Record CartaRevelada para DTO en JuegoController

**Cleanup:**
- Removido using Microsoft.VisualBasic en Juego.cs

**DI Fix:**
- ReglasController: JuegoService concreto → IJuegoService interfaz

### Fase 4: Seguridad, Persistencia y Deployment

**Seguridad:**
- BCrypt.Net-Next para password hashing
- JWT Bearer authentication con tokens
- Session hardening (SecurePolicy, DataProtection keys)

**Persistencia (EF Core + SQLite):**
- AppDbContext con 3 entidades: Usuario, Partida, Estadistica
- Unique index en Nickname
- Relaciones 1:1 (Usuario-Estadistica) y 1:N (Usuario-Partidas)

**Deployment:**
- Dockerfile multi-stage (SDK 10.0 → ASPNET 10.0, puerto 8080)
- docker-compose.yml con health checks
- GitHub Actions CI/CD (build, docker push, code quality)

**Observabilidad:**
- Serilog structured logging (console + rolling file)
- Health endpoints: /health, /health/ready
- Debug.WriteLine → ILogger en 8 lugares

**Features:**
- LeaderboardService con GetTopPlayersAsync, GetPlayerRankAsync, ActualizarEstadisticasAsync
- Record LeaderboardEntry

**Performance:**
- IMemoryCache con 30min TTL para baraja
- Cache keys: CartasJuego, CartasPremio, CartasCastigo

**NuGet Packages Añadidos:**
- BCrypt.Net-Next 4.0.3
- Microsoft.AspNetCore.Authentication.JwtBearer 10.0.0
- Microsoft.EntityFrameworkCore.Sqlite 10.0.0
- Serilog.AspNetCore 8.0.0, Serilog.Sinks.Console 6.0.0
- Serilog.Sinks.File 6.0.0, AspNetCore.HealthChecks.Sqlite 8.0.0

---

**Ver DOCUMENTATION_COMPLETE.md para documentación detallada de todos los cambios.**
