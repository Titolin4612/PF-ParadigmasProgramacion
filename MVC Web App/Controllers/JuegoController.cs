using Microsoft.AspNetCore.Mvc;
using CL_ProyectoFinalPOO.Clases;
using System.Collections.Generic;

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class JuegoController : Controller
    {
        private static Juego juego;
        private static List<Jugador> jugadores;
        Baraja baraja;

        [HttpPost]
        public IActionResult Iniciar()
        {
            try
            {
                baraja.CargarCartas(); 
                juego = new Juego();
                jugadores = HomeController.ObtenerJugadores();

                juego.Jugadores = jugadores;
                juego.BarajarCartas();
                juego.RepartirCartasIniciales(juego.CartasPorJugador);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home", new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Jugadores = jugadores;
            ViewBag.Carta = null;
            return View();
        }

        [HttpPost]
        public IActionResult CogerCarta()
        {
            var jugador = jugadores[Juego.IndiceJugador];
            var carta = jugador.CogerCarta();
            Juego.IndiceJugador = (byte)((Juego.IndiceJugador + 1) % jugadores.Count);

            ViewBag.Jugadores = jugadores;
            ViewBag.Carta = carta;

            return View("Index");
        }
    }
}
