// MVC_ProyectoFinalPOO/Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Services;
using System.Text.Json;
using System;
using System.Diagnostics;
using CL_ProyectoFinalPOO.Clases; // Necesario para el tipo List<Jugador> en sesi�n


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
            catch (ArgumentException ex) // Capturar excepciones espec�ficas
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
            catch (InvalidOperationException ex) // Capturar excepciones espec�ficas
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
            catch (Exception ex) // Captura general para errores inesperados del servicio
            {
                Debug.WriteLine($"HomeController.AgregarJugador: Error inesperado - {ex.Message}");
                // Para el usuario, un mensaje m�s gen�rico puede ser mejor
                return RedirectToAction("Index", new { error = "Ocurri� un error al agregar el jugador." });
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
                return RedirectToAction("Index", new { error = "Ocurri� un error al eliminar el jugador." });
            }
        }

        [HttpPost]
        public IActionResult Play()
        {
            try
            {
                var jugadoresConfigurados = _homeService.ValidarConfiguracionJugadoresParaJuego();

                // Guardar en sesi�n por si el usuario refresca la p�gina de Juego directamente
                // Aunque idealmente, el estado del juego deber�a ser manejado completamente por JuegoService,
                // esta es una forma de rehidrataci�n simple.
                HttpContext.Session.SetString("ListaJugadoresConfig", JsonSerializer.Serialize(jugadoresConfigurados));

                // IniciarJuego en JuegoService ahora tambi�n llama a _homeService.LimpiarConfiguracionJugadores()
                _juegoService.IniciarJuego(jugadoresConfigurados);

                // La configuraci�n de jugadores se limpia dentro de IniciarJuego (si tiene �xito)
                // No es necesario HttpContext.Session.Remove("ListaJugadoresConfig"); aqu�, se puede hacer en JuegoController.Index si se usa.

                return RedirectToAction("Index", "Juego");
            }
            catch (InvalidOperationException ex) // De ValidarConfiguracionJugadoresParaJuego
            {
                TempData["ErrorGlobal"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex) // De IniciarJuego o serializaci�n
            {
                Debug.WriteLine($"HomeController.Play: Error al iniciar juego - {ex.Message}");
                TempData["ErrorGlobal"] = "Error cr�tico al intentar iniciar el juego: " + ex.Message;
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