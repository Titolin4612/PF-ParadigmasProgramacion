using Castle.DynamicProxy;
using CL_ProyectoFinalPOO.Aspectos;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MVC_ProyectoFinalPOO.Data;
using MVC_ProyectoFinalPOO.Services;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db"));

builder.Services.AddDataProtection()
    .SetApplicationName("BlessingsAndCurses")
    .SetDefaultKeyLifetime(TimeSpan.FromDays(90));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "BlessingsAndCurses",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "BlessingsAndCursesUsers",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "SuperSecretKeyThatShouldBeAtLeast32CharactersLong!"))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();

builder.Services.AddTransient<AuthInterceptor>();
builder.Services.AddTransient<InterceptorValidacion>();
builder.Services.AddTransient<InterceptorCargaArchivo>();

builder.Services.AddScoped<HomeService>();
builder.Services.AddScoped<JuegoService>();

builder.Services.AddSingleton(provider =>
{
    var proxyGenerator = new ProxyGenerator();
    var interceptor = provider.GetRequiredService<InterceptorCargaArchivo>();

    var barajaInstance = proxyGenerator.CreateClassProxy<Baraja>(interceptor);
    return barajaInstance;
});

builder.Services.AddMemoryCache();
builder.Services.AddScoped<ReglasService>();

builder.Services.AddScoped<IReglasService, ReglasService>();

builder.Services.AddScoped<IHomeService>(provider =>
{
    return provider.GetRequiredService<HomeService>();
});

builder.Services.AddScoped<IJuegoService>(provider =>
{
    var generator = new ProxyGenerator();
    var authInterceptor = provider.GetRequiredService<AuthInterceptor>();
    var interceptorValidacion = provider.GetRequiredService<InterceptorValidacion>();
    var real = provider.GetRequiredService<JuegoService>();

    return generator.CreateInterfaceProxyWithTarget<IJuegoService>(real, authInterceptor, interceptorValidacion);
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddHealthChecks()
    .AddSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db", name: "sqlite");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
