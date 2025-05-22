using Microsoft.AspNetCore.Mvc;
using CL_ProyectoFinalPOO.Clases;
using MVC_ProyectoFinalPOO.Services;
using System.Text.Json;

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class JuegoController : Controller
    {
        private readonly HomeService _homeService;
        private readonly JuegoService _juegoService;
        public static bool juegoIniciado = false;
        public JuegoController()
        {
            _homeService = HomeService.Instance; // Uso del Singleton
            _juegoService = JuegoService.Instance; // Uso del Singleton
        }

        public IActionResult Index()
        {
            try
            {
                List<Jugador> jugadores = _homeService.ObtenerJugadores();

                if (!juegoIniciado)
                {
                    _juegoService.IniciarJuego(jugadores);
                    juegoIniciado = true;
                }

                if (jugadores == null || jugadores.Count == 0)
                {
                    ViewBag.MensajeError = "No hay jugadores configurados. Vuelve a la pantalla inicial.";
                    return View("Index");
                }

                ViewBag.Jugadores = _juegoService.ObtenerJugadores();
                ViewBag.JugadorActual = _juegoService.ObtenerJugadorActual();
                ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
                ViewBag.JuegoTerminado = _juegoService.JuegoTerminado();
                ViewBag.TotalCartasEnMazo = _juegoService.TotalCartas();

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = "Error iniciando el juego: " + ex.Message;
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult CogerCarta()
        {
            try
            {
                var (carta, puntos) = _juegoService.CogerCarta();
                var jugadorActual = _juegoService.ObtenerJugadorActual();

                object cartaRevelada;

                if (carta is CartaJuego juego)
                {
                    cartaRevelada = new
                    {
                        TipoCarta = "juego",
                        Nombre = juego.Nombre,
                        Mitologia = juego.Mitologia,
                        Descripcion = juego.Descripcion,
                        ImagenArteUrl = juego.ImagenUrl,
                        Rareza = juego.RarezaCarta.ToString(),
                        Puntos = puntos
                    };
                }
                else if (carta is CartaPremio premio)
                {
                    cartaRevelada = new
                    {
                        TipoCarta = "premio",
                        Nombre = premio.Nombre,
                        Mitologia = premio.Mitologia,
                        Descripcion = premio.Descripcion,
                        ImagenArteUrl = premio.ImagenUrl,
                        Bendicion = premio.Bendicion,
                        Puntos = puntos
                    };
                }
                else if (carta is CartaCastigo castigo)
                {
                    cartaRevelada = new
                    {
                        TipoCarta = "castigo",
                        Nombre = castigo.Nombre,
                        Mitologia = castigo.Mitologia,
                        Descripcion = castigo.Descripcion,
                        ImagenArteUrl = castigo.ImagenUrl,
                        Maleficio = castigo.Maleficio,
                        Puntos = puntos
                    };
                }
                else
                {
                    throw new Exception("Tipo de carta desconocido.");
                }

                ViewBag.CartaRevelada = cartaRevelada;
                ViewBag.Jugadores = _juegoService.ObtenerJugadores();
                ViewBag.JugadorActual = jugadorActual;
                ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
                ViewBag.JuegoTerminado = _juegoService.JuegoTerminado();
                ViewBag.TotalCartasEnMazo = _juegoService.TotalCartas();

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("Index");
            }
        }


        [HttpPost]
        public IActionResult SiguienteTurno()
        {
            _juegoService.PasarTurno();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Finalizar()
        {
            var ganador = _juegoService.FinalizarJuego();
            ViewBag.Jugadores = _juegoService.ObtenerJugadores();
            ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
            ViewBag.JuegoTerminado = true;
            ViewBag.MensajeError = $"🎉 GANADOR: {ganador.Nickname} con {ganador.Puntos} puntos";
            return View("Index");
        }

        [HttpPost]
        public IActionResult Reiniciar()
        {
            _juegoService.ReiniciarJuego();
            return RedirectToAction("Index", "Home");
        }
    }
}
