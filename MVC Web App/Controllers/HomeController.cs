// MVC_ProyectoFinalPOO/Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Services;
using System.Text.Json;
using System;
using System.Diagnostics;
using CL_ProyectoFinalPOO.Clases; // Necesario para el tipo List<Jugador> en sesión


namespace MVC_ProyectoFinalPOO.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;
        private readonly JuegoService _juegoService;

        public HomeController(HomeService homeService, JuegoService juegoService)
        {
            _homeService = homeService;
            _juegoService = juegoService;
        }

        public IActionResult Index(string error = null)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
            }
            else if (TempData.ContainsKey("ErrorGlobal"))
            {
                ViewBag.Error = TempData["ErrorGlobal"] as string;
            }

            ViewBag.Players = _homeService.ObtenerJugadoresConfigurados();
            return View();
        }

        [HttpPost]
        public IActionResult AgregarJugador(string nickname, int apuesta)
        {
            try
            {
                _homeService.AgregarJugadorConfigurado(nickname, apuesta);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex) // Capturar excepciones específicas
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
            catch (InvalidOperationException ex) // Capturar excepciones específicas
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
            catch (Exception ex) // Captura general para errores inesperados del servicio
            {
                Debug.WriteLine($"HomeController.AgregarJugador: Error inesperado - {ex.Message}");
                // Para el usuario, un mensaje más genérico puede ser mejor
                return RedirectToAction("Index", new { error = "Ocurrió un error al agregar el jugador." });
            }
        }

        [HttpPost]
        public IActionResult EliminarJugador()
        {
            try
            {
                _homeService.EliminarUltimoJugadorConfigurado();
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeController.EliminarJugador: Error inesperado - {ex.Message}");
                return RedirectToAction("Index", new { error = "Ocurrió un error al eliminar el jugador." });
            }
        }

        [HttpPost]
        public IActionResult Play()
        {
            try
            {
                var jugadoresConfigurados = _homeService.ValidarConfiguracionJugadoresParaJuego();

                // Guardar en sesión por si el usuario refresca la página de Juego directamente
                // Aunque idealmente, el estado del juego debería ser manejado completamente por JuegoService,
                // esta es una forma de rehidratación simple.
                HttpContext.Session.SetString("ListaJugadoresConfig", JsonSerializer.Serialize(jugadoresConfigurados));

                // IniciarJuego en JuegoService ahora también llama a _homeService.LimpiarConfiguracionJugadores()
                _juegoService.IniciarJuego(jugadoresConfigurados);

                // La configuración de jugadores se limpia dentro de IniciarJuego (si tiene éxito)
                // No es necesario HttpContext.Session.Remove("ListaJugadoresConfig"); aquí, se puede hacer en JuegoController.Index si se usa.

                return RedirectToAction("Index", "Juego");
            }
            catch (InvalidOperationException ex) // De ValidarConfiguracionJugadoresParaJuego
            {
                TempData["ErrorGlobal"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex) // De IniciarJuego o serialización
            {
                Debug.WriteLine($"HomeController.Play: Error al iniciar juego - {ex.Message}");
                TempData["ErrorGlobal"] = "Error crítico al intentar iniciar el juego: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Privacy() => View();

        public IActionResult About() => View();

        public IActionResult Reglas()
        {
            return View("Reglas"); // Asume que existe una vista Reglas.cshtml en Views/Home o Views/Shared
        }
    }
}