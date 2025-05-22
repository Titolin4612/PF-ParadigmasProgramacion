// ViewModel para la carta revelada (puede ir en un archivo separado)
using CL_ProyectoFinalPOO.Clases;
using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Services;
using MVC_ProyectoFinalPOO.Controllers;


public class JuegoController : Controller
{
    private readonly HomeService _homeService;
    private readonly JuegoService _juegoService;
    public static bool juegoIniciado = false; // Considerar alternativas para el estado en producción

    public JuegoController()
    {
        _homeService = HomeService.Instance;
        _juegoService = JuegoService.Instance;
    }

    public IActionResult Index()
    {
        try
        {
            // Lógica para iniciar el juego si no está iniciado
            if (!juegoIniciado)
            {
                List<Jugador> jugadoresConfigurados = _homeService.ObtenerJugadores();
                if (jugadoresConfigurados != null && jugadoresConfigurados.Any())
                {
                    _juegoService.IniciarJuego(jugadoresConfigurados);
                    juegoIniciado = true;
                }
            }

            var jugadoresEnPartida = _juegoService.ObtenerJugadores();
            if (jugadoresEnPartida == null || !jugadoresEnPartida.Any())
            {
                ViewBag.MensajeError = "No hay jugadores configurados para la partida. Vuelve a la pantalla inicial.";
                juegoIniciado = false; // Permitir reintentar configuración
                                       // No se puede continuar sin jugadores, así que podrías retornar a Home
                                       // return RedirectToAction("Index", "Home"); 
            }

            ViewBag.Jugadores = jugadoresEnPartida;
            ViewBag.JugadorActual = _juegoService.ObtenerJugadorActual();
            ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
            ViewBag.JuegoTerminado = _juegoService.JuegoTerminado();
            ViewBag.TotalCartasEnMazo = _juegoService.TotalCartas();

            // CartaRevelada viene del POST de CogerCarta, o es null si es GET de SiguienteTurno/Finalizar
            // La vista ya maneja ViewBag.CartaRevelada siendo null.

            return View("Index");
        }
        catch (Exception ex)
        {
            ViewBag.MensajeError = "Error al cargar el juego: " + ex.Message;
            juegoIniciado = false; // Resetear para permitir reintento
            return View("Index"); // Presentar el error en la misma vista
        }
    }

    [HttpPost]
    public IActionResult CogerCarta()
    {
        try
        {
            var (carta, puntos) = _juegoService.CogerCarta();
            var jugadorActual = _juegoService.ObtenerJugadorActual();

            CartaReveladaViewModel viewModel = null;

            if (carta != null) // Solo procesar si se obtuvo una carta
            {
                viewModel = new CartaReveladaViewModel
                {
                    Nombre = carta.Nombre,
                    Mitologia = carta.Mitologia,
                    Descripcion = carta.Descripcion,
                    // Asumiendo que ImagenUrl en tus clases Carta ya es la URL correcta (ej. ~/images/cards/...)
                    // Si ImagenUrl es solo el nombre del archivo, necesitarías construir la ruta aquí o en la vista.
                    ImagenArteUrl = carta.ImagenUrl,
                    Puntos = puntos
                };

                if (carta is CartaJuego juego)
                {
                    viewModel.TipoCarta = "juego";
                    viewModel.Rareza = juego.RarezaCarta.ToString();
                }
                else if (carta is CartaPremio premio)
                {
                    viewModel.TipoCarta = "premio";
                    viewModel.Bendicion = premio.Bendicion;
                }
                else if (carta is CartaCastigo castigo)
                {
                    viewModel.TipoCarta = "castigo";
                    viewModel.Maleficio = castigo.Maleficio;
                }
                else
                {
                    viewModel.TipoCarta = "desconocido"; // Para un tipo de carta no manejado explícitamente
                }
            }
            else
            {
                // No hay más cartas o error al obtenerla desde el servicio
                ViewBag.MensajeError = "No quedan más cartas en el mazo o no se pudo obtener una carta.";
            }


            ViewBag.CartaRevelada = viewModel; // Puede ser null si no se cogió carta
            ViewBag.Jugadores = _juegoService.ObtenerJugadores();
            ViewBag.JugadorActual = jugadorActual;
            ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
            ViewBag.JuegoTerminado = _juegoService.JuegoTerminado();
            ViewBag.TotalCartasEnMazo = _juegoService.TotalCartas();

            return View("Index");
        }
        catch (Exception ex)
        {
            ViewBag.MensajeError = "Error al coger carta: " + ex.Message;
            ViewBag.CartaRevelada = null;
            // Poblar el resto del ViewBag para que la vista no falle
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
        // ViewBag.CartaRevelada se limpiará naturalmente al hacer RedirectToAction
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Finalizar()
    {
        var ganador = _juegoService.FinalizarJuego();
        ViewBag.Jugadores = _juegoService.ObtenerJugadores();
        ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
        ViewBag.JuegoTerminado = true;
        ViewBag.CartaRevelada = null; // No hay carta al finalizar

        if (ganador != null)
        {
            ViewBag.MensajeError = $"🎉 ¡GANADOR! {ganador.Nickname} con {ganador.Puntos} puntos. 🎉";
        }
        else
        {
            ViewBag.MensajeError = "La partida ha finalizado.";
        }
        juegoIniciado = false; // Preparar para una nueva partida desde Home
        return View("Index");
    }

    [HttpPost]
    public IActionResult Reiniciar()
    {
        _juegoService.ReiniciarJuego(); // Esto debería limpiar el estado en JuegoService
        juegoIniciado = false;          // Resetear el flag del controlador
        return RedirectToAction("Index", "Home");
    }
}
