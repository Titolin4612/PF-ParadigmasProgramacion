using Castle.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Aspectos;
using CL_ProyectoFinalPOO.Interfaces;
using MVC_ProyectoFinalPOO.Servicios;

        var builder = WebApplication.CreateBuilder(args);

        // Agregar servicios
        builder.Services.AddControllersWithViews();

        // Registrar Baraja con ArchivoCargaInterceptor
        builder.Services.AddSingleton<Baraja>(provider =>
        {
            var proxyGenerator = new ProxyGenerator();
            var baraja = new Baraja();
            return proxyGenerator.CreateClassProxyWithTarget<Baraja>(baraja, new InterceptorCargaArchivo());
        });

        // Registrar JuegoService con AutenticacionInterceptor y ValidacionInterceptor
        builder.Services.AddTransient<IJuegoService>(provider =>
        {
            var proxyGenerator = new ProxyGenerator();
            var baraja = provider.GetService<Baraja>();
            var juegoService = new JuegoService(baraja);
            return proxyGenerator.CreateInterfaceProxyWithTarget<IJuegoService>(
                juegoService,
                new InterceptorAutenticacion(),
                new InterceptorValidacion());
        });

        var app = builder.Build();

        // Configurar el pipeline
        app.UseRouting();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Juego}/{action=Index}/{id?}");

        app.Run();