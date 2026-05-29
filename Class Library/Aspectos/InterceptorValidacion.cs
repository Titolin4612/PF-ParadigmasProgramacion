using Castle.DynamicProxy;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Eventos;
using CL_ProyectoFinalPOO.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CL_ProyectoFinalPOO.Aspectos
{
    public class InterceptorValidacion : IInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<InterceptorValidacion> _logger;

        public InterceptorValidacion(IHttpContextAccessor httpContextAccessor, ILogger<InterceptorValidacion> logger)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                    _logger.LogWarning("Error: invocation.InvocationTarget no es IJuegoService.");
                    return;
                }

                Juego? juegoActual = juegoService.ObtenerInstanciaJuegoActual();

                if (juegoActual == null)
                {
                    _logger.LogWarning("Error: No se pudo obtener juegoActual desde IJuegoService.");
                    return;
                }

                var publisher = juegoActual.PublicadorJuego;
                var ganador = invocation.ReturnValue as Jugador;

                if (publisher == null)
                {
                    _logger.LogWarning("Error: No se pudo obtener publisher del juegoActual.");
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