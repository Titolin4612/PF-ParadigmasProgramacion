using Microsoft.AspNetCore.Mvc;
using CL_ProyectoFinalPOO.Clases; // Asegúrate que tus clases de carta están aquí
using MVC_ProyectoFinalPOO.Services;

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class JuegoController : Controller
    {
        private readonly HomeService _homeService;
        private readonly JuegoService _juegoService;
        public static bool juegoIniciado = false;

        public JuegoController()
        {
            _homeService = HomeService.Instance;
            _juegoService = JuegoService.Instance;
        }

        public IActionResult Index()
        {
            try
            {
                ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
                if (!juegoIniciado)
                {
                    List<Jugador> jugadoresConfigurados = _homeService.ObtenerJugadores();
                    if (jugadoresConfigurados != null && jugadoresConfigurados.Any())
                    {
                        Juego.Jugadores.Clear();
                        Juego.IndiceJugador = 0;
                        _juegoService.IniciarJuego(jugadoresConfigurados);
                        juegoIniciado = true;
                    }
                }

                var jugadoresEnPartida = _juegoService.ObtenerJugadores();
                if (jugadoresEnPartida == null || !jugadoresEnPartida.Any())
                {
                    ViewBag.MensajeError = "No hay jugadores configurados para la partida. Vuelve a la pantalla inicial.";
                    juegoIniciado = false;
                }

                ViewBag.Jugadores = jugadoresEnPartida;
                ViewBag.JugadorActual = _juegoService.ObtenerJugadorActual();
                ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
                ViewBag.JuegoTerminado = _juegoService.JuegoTerminado();
                ViewBag.TotalCartasEnMazo = _juegoService.TotalCartas();

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = "Error al cargar el juego: " + ex.Message;
                juegoIniciado = false;
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

                object cartaReveladaParaViewBag = null;

                if (carta != null)
                {
                    // Inicializar todas las propiedades posibles a null o valores por defecto
                    string tipoCarta = "desconocido";
                    string nombre = carta.Nombre;
                    string mitologia = carta.Mitologia;
                    string descripcion = carta.Descripcion;
                    // Asumiendo que ImagenUrl ya es la ruta correcta o solo el nombre del archivo
                    // Si es solo el nombre, la vista se encargará de Url.Content()
                    string imagenArteUrl = carta.ImagenUrl;
                    string rareza = null;
                    string bendicion = null;
                    string maleficio = null;

                    if (carta is CartaJuego juego)
                    {
                        tipoCarta = "juego";
                        rareza = juego.RarezaCarta.ToString();
                    }
                    else if (carta is CartaPremio premio)
                    {
                        tipoCarta = "premio";
                        bendicion = premio.Bendicion;
                    }
                    else if (carta is CartaCastigo castigo)
                    {
                        tipoCarta = "castigo";
                        maleficio = castigo.Maleficio;
                    }

                    cartaReveladaParaViewBag = new
                    {
                        TipoCarta = tipoCarta,
                        Nombre = nombre,
                        Mitologia = mitologia,
                        Descripcion = descripcion,
                        ImagenArteUrl = imagenArteUrl,
                        Puntos = puntos,
                        Rareza = rareza,
                        Bendicion = bendicion,
                        Maleficio = maleficio
                    };
                }
                else
                {
                    ViewBag.MensajeError = "No quedan más cartas en el mazo o no se pudo obtener una carta.";
                }

                ViewBag.CartaRevelada = cartaReveladaParaViewBag;
                ViewBag.Jugadores = _juegoService.ObtenerJugadores();
                ViewBag.JugadorActual = jugadorActual;
                ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
                ViewBag.JuegoTerminado = _juegoService.JuegoTerminado();
                ViewBag.TotalCartasEnMazo = _juegoService.TotalCartas();

                if (_juegoService.JuegoTerminado())
                {
                    Finalizar();
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = "Error al coger carta: " + ex.Message;
                ViewBag.CartaRevelada = null;
                ViewBag.Jugadores = _juegoService.ObtenerJugadores();
                ViewBag.JugadorActual = _juegoService.ObtenerJugadorActual();
                ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
                ViewBag.JuegoTerminado = _juegoService.JuegoTerminado();
                ViewBag.TotalCartasEnMazo = _juegoService.TotalCartas();
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult SiguienteTurno()
        {
            
            _juegoService.PasarTurno();
            ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Finalizar()
        {
            _juegoService.FinalizarJuego();
            var ganador = _juegoService.FinalizarJuego();
            ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
            ViewBag.Jugadores = _juegoService.ObtenerJugadores();
            ViewBag.CartaRevelada = null;

            if (ganador != null)
            {
                ViewBag.MensajeError = $"🎉 ¡GANADOR! {ganador.Nickname} con {ganador.Puntos} puntos. 🎉";
            }
            else
            {
                ViewBag.MensajeError = "La partida ha finalizado.";
            }
            juegoIniciado = false;
            ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
            ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
            ViewBag.JuegoTerminado = _juegoService.JuegoTerminado();
            return View("Index");
        }

        [HttpPost]
        public IActionResult NuevaRonda()
        {
            var jugadoresActualesEnServicio = _juegoService.ObtenerJugadores();

            if (jugadoresActualesEnServicio != null && jugadoresActualesEnServicio.Any() && jugadoresActualesEnServicio.Count() >= 2)
            {
         
                _juegoService.ComenzarNuevaRondaConJugadoresActuales();
                juegoIniciado = true; 
            } else
            {
                ViewBag.MensajeError = "Error al inciiar nueva ronda, no hay suficientes jugadores";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reiniciar() 
        {
            Juego.Jugadores.Clear();
            Juego.IndiceJugador = 0;
            _juegoService.ReiniciarJuego(); 
            juegoIniciado = false;
            return RedirectToAction("Index", "Home"); 
        }
    }
}
