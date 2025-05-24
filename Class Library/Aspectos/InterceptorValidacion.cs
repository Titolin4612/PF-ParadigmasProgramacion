using Castle.DynamicProxy;
using CL_ProyectoFinalPOO.Clases; 
using CL_ProyectoFinalPOO.Eventos; 
using CL_ProyectoFinalPOO.Interfaces; 
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace CL_ProyectoFinalPOO.Aspectos
{
    public class InterceptorValidacion : IInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InterceptorValidacion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            if (invocation.Method.DeclaringType == typeof(IJuegoService) && invocation.Method.Name == "FinalizarJuego")
            {
                var juegoService = invocation.InvocationTarget as IJuegoService;

                if (juegoService == null)
                {

                    Console.WriteLine("Error en ValidacionGuardadoInterceptor: invocation.InvocationTarget no es IJuegoService.");
                    return;
                }

                Juego juegoActual = juegoService.ObtenerInstanciaJuegoActual(); 

                if (juegoActual == null)
                {
                    Console.WriteLine("Error en ValidacionGuardadoInterceptor: No se pudo obtener juegoActual desde IJuegoService.");
                    return;
                }

                var publisher = juegoActual.PublicadorJuego;
                var ganador = invocation.ReturnValue as Jugador;

                if (publisher == null)
                {

                    Console.WriteLine("Error en ValidacionGuardadoInterceptor: No se pudo obtener publisher del juegoActual.");
                    return;
                }

                bool esValidoParaGuardar = true;
                string mensajeValidacion = "Los datos del juego y del ganador parecen correctos.";

                if (ganador == null)
                {
                    esValidoParaGuardar = false;
                    mensajeValidacion = "No se pudo determinar un ganador. No se guardan datos.";
                }
                else if (ganador.Puntos < 1) 
                {
                    esValidoParaGuardar = false;
                    mensajeValidacion = $"Validación fallida: El ganador {ganador.Nickname} tiene {ganador.Puntos} puntos (se requiere > 0).";
                }

                if (CL_ProyectoFinalPOO.Clases.Juego.Jugadores == null || !CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Any())
                {
                    esValidoParaGuardar = false;
                    mensajeValidacion = "Validación fallida: No hay jugadores registrados en el estado final del juego.";
                }

                if (esValidoParaGuardar)
                {
                    string mensajeExito = $"Guardado BD: ¡Datos de validados y guardados exitosamente!";
                    if (_httpContextAccessor.HttpContext != null)
                    {
                        _httpContextAccessor.HttpContext.Items["SimulacionBDMensaje"] = mensajeExito;
                        _httpContextAccessor.HttpContext.Items["SimulacionBDMensajeTipo"] = "success";
                    }
                }
                else
                {
                    string mensajeFallo = $"Guardado BD: Error de validación, No se guardaron los datos.";
                     if (_httpContextAccessor.HttpContext != null)
                    {
                        _httpContextAccessor.HttpContext.Items["SimulacionBDMensaje"] = mensajeFallo;
                        _httpContextAccessor.HttpContext.Items["SimulacionBDMensajeTipo"] = "error";
                    }
                }
            }
        }
    }
}