using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Models;
using MVC_ProyectoFinalPOO.Services;
using System.Text.Json;
using System.Diagnostics;

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;

        public HomeController()
        {
            _homeService = HomeService.Instance; // Uso del Singleton
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

            ViewBag.Players = _homeService.ObtenerJugadores();
            return View();
        }

        [HttpPost]
        public IActionResult AgregarJugador(string nickname, int apuesta)
        {
            try
            {
                _homeService.AgregarJugador(nickname, apuesta);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult EliminarJugador()
        {
            try
            {
                _homeService.EliminarJugador();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Play()
        {
            Debug.WriteLine("HomeController.Play: Iniciando proceso.");
            try
            {
                var jugadores = _homeService.ValidarJugadores();
                HttpContext.Session.SetString("ListaJugadoresConfig", JsonSerializer.Serialize(jugadores));
                Debug.WriteLine("HomeController.Play: Configuración de jugadores guardada en sesión. Redirigiendo a JuegoController.");
                return RedirectToAction("Index", "Juego");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR en HomeController.Play: {ex.Message} {ex.StackTrace}");
                TempData["ErrorGlobal"] = "Error al intentar iniciar el juego: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Privacy() => View();
        public IActionResult About() => View();
        public IActionResult Reglas()
        {
            return View("Reglas");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}