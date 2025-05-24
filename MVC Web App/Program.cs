using Castle.DynamicProxy;
using CL_ProyectoFinalPOO.Aspectos;
using CL_ProyectoFinalPOO.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVC_ProyectoFinalPOO.Services;
using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures; // Added for ITempDataDictionaryFactory
using CL_ProyectoFinalPOO.Clases; // Make sure to include this for Baraja

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add HttpContextAccessor for access to HttpContext (needed by interceptors)
builder.Services.AddHttpContextAccessor();

// Register the factory for TempData (needed by InterceptorValidacion if it uses TempData)
builder.Services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();

// Register your interceptors as Transient for proper request-scoped dependencies
builder.Services.AddTransient<AuthInterceptor>();
builder.Services.AddTransient<InterceptorValidacion>();
builder.Services.AddTransient<InterceptorCargaArchivo>(); // Register your file loading interceptor

// Register real service instances
// HomeService and JuegoService are registered as Singletons, meaning one instance
// will be shared across the entire application lifecycle. Be sure this is
// the desired behavior, especially for JuegoService if game state is per-user.
builder.Services.AddSingleton<HomeService>();
builder.Services.AddSingleton<JuegoService>();

// --- Baraja and ReglasService Configuration with Interception ---
// Register Baraja and apply the InterceptorCargaArchivo
builder.Services.AddSingleton(provider =>
{
    var proxyGenerator = new ProxyGenerator();
    var interceptor = provider.GetRequiredService<InterceptorCargaArchivo>();

    // Create a proxy for the Baraja class.
    // The CargarCartas method MUST be 'virtual' for Castle.DynamicProxy to intercept it.
    var barajaInstance = proxyGenerator.CreateClassProxy<Baraja>(interceptor);
    return barajaInstance;
});

// Register ReglasService directly so it can be injected by other services (like ReglasController)
// This is crucial because ReglasController directly asks for ReglasService, not IReglasService.
builder.Services.AddScoped<ReglasService>(); // <--- ADDED THIS LINE

// Register IReglasService, which depends on the proxied Baraja instance.
// Using Scoped for IReglasService as it's common for request-scoped services.
builder.Services.AddScoped<IReglasService, ReglasService>();
// --- End Baraja and ReglasService Configuration ---


// Register IHomeService with AuthInterceptor (as Scoped as it's per-request/per-user in MVC)
builder.Services.AddScoped<IHomeService>(provider =>
{
    var generator = new ProxyGenerator();
    var interceptor = provider.GetRequiredService<AuthInterceptor>();
    var real = provider.GetRequiredService<HomeService>();
    return generator.CreateInterfaceProxyWithTarget<IHomeService>(real, interceptor);
});

// Register IJuegoService with *both* interceptors (Scoped as it's per-request/per-user in MVC)
builder.Services.AddScoped<IJuegoService>(provider =>
{
    var generator = new ProxyGenerator();
    var authInterceptor = provider.GetRequiredService<AuthInterceptor>();
    var interceptorValidacion = provider.GetRequiredService<InterceptorValidacion>(); // Clarified variable name
    var real = provider.GetRequiredService<JuegoService>();

    // Pass interceptors in the desired order of execution.
    // For example, authentication/authorization first, then validation.
    return generator.CreateInterfaceProxyWithTarget<IJuegoService>(real, authInterceptor, interceptorValidacion);
});

// --- Configure Session ---
builder.Services.AddDistributedMemoryCache(); // Required for in-memory session store
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// --- End Configure Session ---

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- Use Session (MUST be before UseAuthorization and MapControllerRoute) ---
app.UseSession();
// --- End Use Session ---

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();