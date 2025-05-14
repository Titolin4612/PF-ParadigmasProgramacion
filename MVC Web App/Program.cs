using CL_ProyectoFinalPOO.Aspectos;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces; // Crear esta carpeta y los servicios
using Castle.DynamicProxy;
using MVC_ProyectoFinalPOO.Servicios;
using Microsoft.AspNetCore.Builder; // Necesario para WebApplicationBuilder
using Microsoft.Extensions.DependencyInjection; // Necesario para IServiceCollection
using Microsoft.Extensions.Hosting; // Necesario para IHostEnvironment

var builder = WebApplication.CreateBuilder(args);

// --- Configuraci�n de Servicios ---

// 1. Servicios MVC
builder.Services.AddControllersWithViews();

// 2. Servicios de Sesi�n
builder.Services.AddDistributedMemoryCache(); // Necesario para sesi�n en memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duraci�n de la sesi�n
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 3. Inyecci�n de Dependencias para tus clases
// Proxy Generator para Castle Dynamic Proxy
builder.Services.AddSingleton<ProxyGenerator>();

// Interceptores (como singletons o transitorios seg�n necesidad)
builder.Services.AddSingleton<InterceptorAutenticacion>();
builder.Services.AddSingleton<InterceptorCargaArchivo>();
builder.Services.AddSingleton<InterceptorValidacion>();

// Baraja: Carga las cartas una vez.
// Podr�amos hacer que IBarajaProvider cargue las cartas.
// Hacemos que Baraja sea una clase normal, no est�tica para cargar.
builder.Services.AddSingleton<Baraja>(provider =>
{
    var baraja = new Baraja();
    // Para la carga de archivos en web, es mejor usar IWebHostEnvironment
    var env = provider.GetRequiredService<IWebHostEnvironment>();
    // Asumimos que cartas.json est� en la ra�z del proyecto o wwwroot
    // Ajusta la ruta seg�n donde est� tu JSON. Si est� en wwwroot, puedes usar ContentRootPath.
    // Si Baraja._rutaArchivoCartas se vuelve no est�tico y se configura:
    // Baraja._rutaArchivoCartas = Path.Combine(env.ContentRootPath, "cartas.json"); 
    // Por ahora, CargarCartas usa la ruta est�tica. Aseg�rate que el archivo est� accesible.
    // Idealmente, la ruta se pasar�a como argumento o se configurar�a.

    // Aqu� aplicar�amos el interceptor si IBarajaService tuviera el m�todo CargarCartas
    // Por ahora, para que CargarCartas() sea interceptado, Baraja debe ser proxieda
    // Y el m�todo debe ser virtual.
    var generator = provider.GetRequiredService<ProxyGenerator>();
    var interceptorCarga = provider.GetRequiredService<InterceptorCargaArchivo>();
    // Para interceptar Baraja directamente, necesitar�amos una interfaz IBaraja con CargarCartas()
    // y registrarla: services.AddSingleton<IBaraja, Baraja>(); y luego proxy.
    // O si CargarCartas es virtual en Baraja:
    var proxiedBaraja = generator.CreateClassProxy<Baraja>(
        new ProxyGenerationOptions(),
        new object[] { /* constructores de Baraja si los tiene */ },
        interceptorCarga // Y otros interceptores si Baraja tiene m�s m�todos interceptables
    );
    try
    {
        proxiedBaraja.CargarCartas(); // Ahora esta llamada es interceptada
                                      // Baraja.CartasJuego, etc., ser�n llenadas.
    }
    catch (Exception ex)
    {
        // Manejar error de carga inicial, quiz�s loggear y lanzar para detener la app si es cr�tico.
        Console.WriteLine($"Error CR�TICO al cargar baraja inicial: {ex.Message}");
        throw; // Detener la aplicaci�n si las cartas son esenciales.
    }
    return proxiedBaraja; // Devolvemos la instancia (proxieda o no) que tiene las listas est�ticas llenas.
                          // En realidad, solo necesitamos que se ejecute CargarCartas una vez.
                          // Las listas CartasJuego, etc., son est�ticas en Baraja.
                          // As� que no necesitamos devolver la instancia proxieda para el uso de esas listas.
                          // Lo importante es que proxiedBaraja.CargarCartas() se ejecute.
});


// Servicio de Juego: Administra la l�gica y el estado del juego en sesi�n.
builder.Services.AddScoped<IJuegoService, JuegoService>(); // Scoped porque depende de HttpContext (sesi�n)

// Para IHttpContextAccessor (necesario para acceder a HttpContext en servicios)
builder.Services.AddHttpContextAccessor();


// --- Fin Configuraci�n de Servicios ---

var app = builder.Build();

// --- Configuraci�n del Pipeline HTTP ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); // Si usas autenticaci�n/autorizaci�n de ASP.NET Core

app.UseSession(); // �Importante! Activar el middleware de sesi�n

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Cargar la baraja una vez al inicio (para poblar las listas est�ticas de Baraja)
// Esto se hace ahora a trav�s de la resoluci�n del singleton Baraja.
// Al solicitar Baraja por primera vez, su factory method se ejecuta, llamando a CargarCartas().
using (var scope = app.Services.CreateScope())
{
    var barajaInstance = scope.ServiceProvider.GetRequiredService<Baraja>();
    // En este punto, Baraja.CargarCartas() (proxieda) ya deber�a haberse ejecutado.
    if (Baraja.CartasJuego == null || !Baraja.CartasJuego.Any())
    {
        Console.WriteLine("ALERTA: Las cartas maestras no se cargaron despu�s de la inicializaci�n del servicio Baraja.");
    }
}


app.Run();