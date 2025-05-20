using Microsoft.AspNetCore.Builder; // Para WebApplication, etc.
using Microsoft.Extensions.DependencyInjection; // Para IServiceCollection
using Microsoft.Extensions.Hosting; // Para IHostEnvironment
using MVC_ProyectoFinalPOO.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor(); // Si lo necesitas para Session u otros servicios de HttpContext

// Registrar tu servicio usando la clase concreta
builder.Services.AddSingleton<ReglasService>();

// --- HABILITAR SESIONES ---
builder.Services.AddDistributedMemoryCache(); // Necesario para la sesión en memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo que la sesión puede estar inactiva
    options.Cookie.HttpOnly = true; // La cookie de sesión no es accesible por script del lado del cliente
    options.Cookie.IsEssential = true; // Necesario para cumplir con GDPR; la cookie de sesión es esencial
});
// --- FIN HABILITAR SESIONES ---

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- USAR SESIÓN (IMPORTANTE: DEBE IR ANTES DE UseAuthorization y MapControllerRoute) ---
app.UseSession();
// --- FIN USAR SESIÓN ---

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();