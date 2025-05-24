using Castle.DynamicProxy;
using CL_ProyectoFinalPOO.Aspectos;
using CL_ProyectoFinalPOO.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVC_ProyectoFinalPOO.Services;
using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures; 
using CL_ProyectoFinalPOO.Clases; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();

builder.Services.AddTransient<AuthInterceptor>();
builder.Services.AddTransient<InterceptorValidacion>();
builder.Services.AddTransient<InterceptorCargaArchivo>(); 

builder.Services.AddSingleton<HomeService>();
builder.Services.AddSingleton<JuegoService>();

builder.Services.AddSingleton(provider =>
{
    var proxyGenerator = new ProxyGenerator();
    var interceptor = provider.GetRequiredService<InterceptorCargaArchivo>();


    var barajaInstance = proxyGenerator.CreateClassProxy<Baraja>(interceptor);
    return barajaInstance;
});

builder.Services.AddScoped<ReglasService>(); 

builder.Services.AddScoped<IReglasService, ReglasService>();

builder.Services.AddScoped<IHomeService>(provider =>
{
    var generator = new ProxyGenerator();
    var interceptor = provider.GetRequiredService<AuthInterceptor>();
    var real = provider.GetRequiredService<HomeService>();
    return generator.CreateInterfaceProxyWithTarget<IHomeService>(real, interceptor);
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
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();