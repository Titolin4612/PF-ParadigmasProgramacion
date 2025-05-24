using Castle.DynamicProxy;
using CL_ProyectoFinalPOO.Clases; // Para Jugador, Juego
using CL_ProyectoFinalPOO.Eventos; // Para Publisher_Eventos_Juego
using CL_ProyectoFinalPOO.Interfaces; // Para IJuegoService
using Microsoft.AspNetCore.Http;
using System;
using System.Linq; // Para Any()

namespace CL_ProyectoFinalPOO.Aspectos
{
    public class InterceptorValidacion : IInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InterceptorValidacion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new Exception(nameof(httpContextAccessor));
        }

        public void Intercept(IInvocation invocation)
        {
            // Dejar que el método original (Ej: FinalizarJuego en JuegoService) se ejecute primero
            invocation.Proceed();

            // Solo actuar para el método FinalizarJuego de la interfaz IJuegoService
            if (invocation.Method.DeclaringType == typeof(IJuegoService) && invocation.Method.Name == "FinalizarJuego")
            {
                // invocation.InvocationTarget es la instancia real de JuegoService
                var juegoService = invocation.InvocationTarget as IJuegoService;

                if (juegoService == null)
                {
                    // Esto no debería ocurrir si el proxy se creó correctamente con CreateInterfaceProxyWithTarget
                    Console.WriteLine("Error en ValidacionGuardadoInterceptor: invocation.InvocationTarget no es IJuegoService.");
                    return;
                }

                Juego juegoActual = juegoService.ObtenerInstanciaJuegoActual(); // Usamos el método de la interfaz

                if (juegoActual == null)
                {
                    Console.WriteLine("Error en ValidacionGuardadoInterceptor: No se pudo obtener juegoActual desde IJuegoService.");
                    return;
                }

                var publisher = juegoActual.PublicadorJuego;
                var ganador = invocation.ReturnValue as Jugador; // El ganador es retornado por FinalizarJuego

                if (publisher == null)
                {
                    // Esto tampoco debería ocurrir si juegoActual está bien inicializado
                    Console.WriteLine("Error en ValidacionGuardadoInterceptor: No se pudo obtener publisher del juegoActual.");
                    return;
                }

                bool esValidoParaGuardar = true;
                string mensajeValidacion = "Los datos del juego y del ganador parecen correctos.";

                // Lógica de Validación (Ejemplos)
                if (ganador == null)
                {
                    esValidoParaGuardar = false;
                    mensajeValidacion = "No se pudo determinar un ganador. No se guardan datos.";
                }
                else if (ganador.Puntos < 1) // Ejemplo: un ganador debe tener al menos 1 punto
                {
                    esValidoParaGuardar = false;
                    mensajeValidacion = $"Validación fallida: El ganador {ganador.Nickname} tiene {ganador.Puntos} puntos (se requiere > 0).";
                }

                if (esValidoParaGuardar)
                {
                    string mensajeExito = $"Guardado BD Interceptor: ¡Datos validados y guardados exitosamente!";
                    if (_httpContextAccessor.HttpContext != null)
                    {
                        _httpContextAccessor.HttpContext.Items["SimulacionBDMensaje"] = mensajeExito;
                        _httpContextAccessor.HttpContext.Items["SimulacionBDMensajeTipo"] = "success";
                    }
                }
                else
                {
                    string mensajeFallo = $"Guardado BD Interceptor: Error de validación, No se guardaron los datos.";
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