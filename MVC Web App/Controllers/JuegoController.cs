using Microsoft.AspNetCore.Mvc;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Eventos; // Necesario para Publisher_Eventosjuego
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System;
using System.Diagnostics;
using MVC_ProyectoFinalPOO.Servicios;
using MVC_ProyectoFinalPOO.Services;

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class JuegoController : Controller
    {
        private static readonly JuegoService _juegoService = new();
        private readonly HomeService _homeService;

        public JuegoController()
        {
            _homeService = HomeService.Instance; // Inyectar HomeService para limpiar configuración
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SalirYReiniciar()
        {
            try
            {
                Debug.WriteLine("JuegoController.SalirYReiniciar: Iniciando proceso de reinicio.");

                // Obtener el juego actual desde la sesión
                var juegoJson = HttpContext.Session.GetString("Juego");
                if (!string.IsNullOrEmpty(juegoJson))
                {
                    var juego = JsonSerializer.Deserialize<Juego>(juegoJson);
                    if (juego != null)
                    {
                        // Llamar al método ReiniciarJuego
                        juego.ReiniciarJuego();
                        Debug.WriteLine("JuegoController.SalirYReiniciar: Juego reiniciado correctamente.");
                    }
                }

                // Limpiar la configuración de jugadores en HomeService
                _homeService.LimpiarConfiguracionJugadores();
                Debug.WriteLine("JuegoController.SalirYReiniciar: Configuración de jugadores limpiada.");

                // Limpiar la sesión
                HttpContext.Session.Remove("Juego");
                HttpContext.Session.Remove("ListaJugadoresConfig");
                Debug.WriteLine("JuegoController.SalirYReiniciar: Sesión limpiada.");

                // Redirigir a la página principal
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR en JuegoController.SalirYReiniciar: {ex.Message} {ex.StackTrace}");
                TempData["ErrorGlobal"] = "Error al reiniciar el juego: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}