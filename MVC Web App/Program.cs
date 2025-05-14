using CL_ProyectoFinalPOO.Aspectos;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces; // Crear esta carpeta y los servicios
using Castle.DynamicProxy;
using MVC_ProyectoFinalPOO.Servicios;
using Microsoft.AspNetCore.Builder; // Necesario para WebApplicationBuilder
using Microsoft.Extensions.DependencyInjection; // Necesario para IServiceCollection
using Microsoft.Extensions.Hosting; // Necesario para IHostEnvironment

var builder = WebApplication.CreateBuilder(args);

// --- Configuración de Servicios ---

// 1. Servicios MVC
builder.Services.AddControllersWithViews();

// 2. Servicios de Sesión
builder.Services.AddDistributedMemoryCache(); // Necesario para sesión en memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 3. Inyección de Dependencias para tus clases
// Proxy Generator para Castle Dynamic Proxy
builder.Services.AddSingleton<ProxyGenerator>();

// Interceptores (como singletons o transitorios según necesidad)
builder.Services.AddSingleton<InterceptorAutenticacion>();
builder.Services.AddSingleton<InterceptorCargaArchivo>();
builder.Services.AddSingleton<InterceptorValidacion>();

// Baraja: Carga las cartas una vez.
// Podríamos hacer que IBarajaProvider cargue las cartas.
// Hacemos que Baraja sea una clase normal, no estática para cargar.
builder.Services.AddSingleton<Baraja>(provider =>
{
    var baraja = new Baraja();
    // Para la carga de archivos en web, es mejor usar IWebHostEnvironment
    var env = provider.GetRequiredService<IWebHostEnvironment>();
    // Asumimos que cartas.json está en la raíz del proyecto o wwwroot
    // Ajusta la ruta según donde esté tu JSON. Si está en wwwroot, puedes usar ContentRootPath.
    // Si Baraja._rutaArchivoCartas se vuelve no estático y se configura:
    // Baraja._rutaArchivoCartas = Path.Combine(env.ContentRootPath, "cartas.json"); 
    // Por ahora, CargarCartas usa la ruta estática. Asegúrate que el archivo esté accesible.
    // Idealmente, la ruta se pasaría como argumento o se configuraría.

    // Aquí aplicaríamos el interceptor si IBarajaService tuviera el método CargarCartas
    // Por ahora, para que CargarCartas() sea interceptado, Baraja debe ser proxieda
    // Y el método debe ser virtual.
    var generator = provider.GetRequiredService<ProxyGenerator>();
    var interceptorCarga = provider.GetRequiredService<InterceptorCargaArchivo>();
    // Para interceptar Baraja directamente, necesitaríamos una interfaz IBaraja con CargarCartas()
    // y registrarla: services.AddSingleton<IBaraja, Baraja>(); y luego proxy.
    // O si CargarCartas es virtual en Baraja:
    var proxiedBaraja = generator.CreateClassProxy<Baraja>(
        new ProxyGenerationOptions(),
        new object[] { /* constructores de Baraja si los tiene */ },
        interceptorCarga // Y otros interceptores si Baraja tiene más métodos interceptables
    );
    try
    {
        proxiedBaraja.CargarCartas(); // Ahora esta llamada es interceptada
                                      // Baraja.CartasJuego, etc., serán llenadas.
    }
    catch (Exception ex)
    {
        // Manejar error de carga inicial, quizás loggear y lanzar para detener la app si es crítico.
        Console.WriteLine($"Error CRÍTICO al cargar baraja inicial: {ex.Message}");
        throw; // Detener la aplicación si las cartas son esenciales.
    }
    return proxiedBaraja; // Devolvemos la instancia (proxieda o no) que tiene las listas estáticas llenas.
                          // En realidad, solo necesitamos que se ejecute CargarCartas una vez.
                          // Las listas CartasJuego, etc., son estáticas en Baraja.
                          // Así que no necesitamos devolver la instancia proxieda para el uso de esas listas.
                          // Lo importante es que proxiedBaraja.CargarCartas() se ejecute.
});


// Servicio de Juego: Administra la lógica y el estado del juego en sesión.
builder.Services.AddScoped<IJuegoService, JuegoService>(); // Scoped porque depende de HttpContext (sesión)

// Para IHttpContextAccessor (necesario para acceder a HttpContext en servicios)
builder.Services.AddHttpContextAccessor();


// --- Fin Configuración de Servicios ---

var app = builder.Build();

// --- Configuración del Pipeline HTTP ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); // Si usas autenticación/autorización de ASP.NET Core

app.UseSession(); // ¡Importante! Activar el middleware de sesión

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Cargar la baraja una vez al inicio (para poblar las listas estáticas de Baraja)
// Esto se hace ahora a través de la resolución del singleton Baraja.
// Al solicitar Baraja por primera vez, su factory method se ejecuta, llamando a CargarCartas().
using (var scope = app.Services.CreateScope())
{
    var barajaInstance = scope.ServiceProvider.GetRequiredService<Baraja>();
    // En este punto, Baraja.CargarCartas() (proxieda) ya debería haberse ejecutado.
    if (Baraja.CartasJuego == null || !Baraja.CartasJuego.Any())
    {
        Console.WriteLine("ALERTA: Las cartas maestras no se cargaron después de la inicialización del servicio Baraja.");
    }
}


app.Run();