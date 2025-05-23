// MVC_ProyectoFinalPOO/Controllers/JuegoController.cs
using Microsoft.AspNetCore.Mvc;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;
using MVC_ProyectoFinalPOO.Services;
using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic; // Para List<T>

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class JuegoController : Controller
    {
        private readonly IJuegoService _juegoService;
        public JuegoController(IJuegoService juegoService)
        {
            _juegoService = juegoService;
        }

        public void CargarViewBagComun(string mensajeErrorPersonalizado = null)
        {
            ViewBag.Jugadores = _juegoService.ObtenerJugadores();
            ViewBag.JugadorActual = _juegoService.ObtenerJugadorActual();
            ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
            ViewBag.JuegoTerminado = _juegoService.JuegoTerminado();
            ViewBag.TotalCartasEnMazo = _juegoService.TotalCartasEnMazo();

            if (!string.IsNullOrEmpty(mensajeErrorPersonalizado))
            {
                ViewBag.MensajeError = mensajeErrorPersonalizado;
            }
        }

        public IActionResult Index()
        {
            try
            {
                if (!_juegoService.EstaJuegoActivo())
                {
                    CargarViewBagComun("No hay partida en curso. Por favor, configura los jugadores en la pantalla de inicio.");
                    return View("Index");
                }

                CargarViewBagComun(); // 👈 Primero cargá todo

                return View("Index");
            }
            catch (Exception ex)
            {
                CargarViewBagComun("Error crítico al cargar la página del juego: " + ex.Message);
                ViewBag.JuegoTerminado = true;
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult CogerCarta()
        {
            if (!_juegoService.EstaJuegoActivo())
            {
                TempData["ErrorGeneral"] = "No se puede coger carta, la partida no está activa.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // MODIFICADO: Ahora recibe (Carta, int)
                var (cartaCogida, puntosObtenidos) = _juegoService.CogerCarta();

                if (cartaCogida != null)
                {
                    // MODIFICADO: Construir el objeto para ViewBag.CartaRevelada aquí
                    string tipoCartaStr = "desconocido";
                    string rareza = null, bendicion = null, maleficio = null;

                    if (cartaCogida is CartaJuego cj) { tipoCartaStr = "juego"; rareza = cj.RarezaCarta.ToString(); }
                    else if (cartaCogida is CartaPremio cp) { tipoCartaStr = "premio"; bendicion = cp.Bendicion; }
                    else if (cartaCogida is CartaCastigo cc) { tipoCartaStr = "castigo"; maleficio = cc.Maleficio; }

                    ViewBag.CartaRevelada = new
                    {
                        TipoCarta = tipoCartaStr,
                        cartaCogida.Nombre,
                        cartaCogida.Mitologia,
                        cartaCogida.Descripcion,
                        cartaCogida.ImagenUrl, // Asumiendo que ImagenUrl está en la clase base Carta o en todas las derivadas
                        Puntos = puntosObtenidos,
                        Rareza = rareza,
                        Bendicion = bendicion,
                        Maleficio = maleficio
                    };
                }
                else
                {
                    ViewBag.CartaRevelada = null;
                }

                // El resto de tu lógica para manejar el mensaje de error y la finalización del juego
                // (que proporcioné en la respuesta anterior) seguiría aquí.
                // Por ejemplo:
                string mensajeErrorCarta = null;
                if (ViewBag.CartaRevelada == null && !_juegoService.JuegoTerminado())
                {
                    mensajeErrorCarta = "No quedan más cartas en el mazo o no se pudo obtener una carta.";
                }

                CargarViewBagComun(mensajeErrorCarta);

                if (ViewBag.JuegoTerminado == true)
                {
                    ViewBag.CartaRevelada = null;
                    if (string.IsNullOrEmpty(ViewBag.MensajeError))
                    {
                        var ganador = _juegoService.FinalizarJuego();
                        ViewBag.MensajeGanador = (ganador != null)
                            ? $"🎉 ¡GANADOR! {ganador.Nickname} con {ganador.Puntos} puntos. 🎉"
                            : "La partida ha finalizado sin un ganador claro, o no hay jugadores.";
                    }
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoController.CogerCarta: Error - {ex.Message}");
                ViewBag.CartaRevelada = null;
                CargarViewBagComun("Error al coger carta: " + ex.Message);
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult SiguienteTurno()
        {
            try
            {
                _juegoService.PasarTurno();
                // Después de pasar turno, el estado se actualiza. Redirigir a Index para que
                // recargue los ViewBags con la información fresca.

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoController.SiguienteTurno: Error - {ex.Message}");
                // Si hay error, no redirigir, sino mostrar el error en la vista actual.
                ViewBag.CartaRevelada = null; // Limpiar cualquier carta mostrada.
                CargarViewBagComun("Error al pasar el turno: " + ex.Message);
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult NuevaRonda()
        {
            // Se asume que esta acción solo está disponible si el juego anterior terminó.
            // EstaJuegoActivo podría seguir siendo true si _juegoActual no se ha anulado.
            if (!_juegoService.EstaJuegoActivo() || !_juegoService.JuegoTerminado())
            {
                TempData["ErrorGeneralJuego"] = "No se puede iniciar una nueva ronda en este momento.";
                return RedirectToAction("Index");
            }
            try
            {
                _juegoService.ComenzarNuevaRondaConJugadoresActuales();
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex) // Errores esperados, como no suficientes jugadores.
            {
                Debug.WriteLine($"JuegoController.NuevaRonda: Operación inválida - {ex.Message}");
                TempData["ErrorGeneralJuego"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex) // Errores inesperados.
            {
                Debug.WriteLine($"JuegoController.NuevaRonda: Error crítico - {ex.Message}");
                TempData["ErrorGeneralJuego"] = "Error crítico al iniciar nueva ronda: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Reiniciar() // Volver a Home para configurar nuevo juego.
        {
            try
            {
                _juegoService.ReiniciarJuego(); // Anula _juegoActual y limpia config.
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoController.Reiniciar: Error - {ex.Message}");
                // Incluso si falla el reinicio en el servicio, intentamos llevar al usuario a Home.
                TempData["ErrorGeneral"] = "Ocurrió un error al reiniciar el juego, por favor intente de nuevo: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult VerResumen()
        {
            try
            {
                // Asegurarse que el juego esté efectivamente terminado o no activo para ver resumen.
                if (!_juegoService.JuegoTerminado() && _juegoService.EstaJuegoActivo())
                {
                    TempData["ErrorGeneralJuego"] = "La partida aún no ha finalizado para ver el resumen.";
                    return RedirectToAction("Index");
                }
                if (!_juegoService.EstaJuegoActivo() && !_juegoService.JuegoTerminado()) // Caso raro: no activo pero tampoco marcado como terminado
                {
                    CargarViewBagComun("No hay información de partida para mostrar resumen.");
                    ViewBag.MensajeGanador = "No hay partida finalizada.";
                    return View("FinJuego");
                }


                var jugadores = _juegoService.ObtenerJugadores();
                ViewBag.Jugadores = jugadores;
                ViewBag.HistorialJuego = _juegoService.ObtenerHistorial();
                ViewBag.JuegoTerminado = true; // Para la vista FinJuego, siempre debe ser true.

                var ganador = jugadores?.OrderByDescending(j => j.Puntos).FirstOrDefault();
                ViewBag.MensajeGanador = (ganador != null)
                    ? $"🎉 ¡GANADOR! {ganador.Nickname} con {ganador.Puntos} puntos. 🎉"
                    : "La partida ha finalizado sin un ganador claro, o no hay jugadores.";

                return View("FinJuego");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoController.VerResumen: Error - {ex.Message}");
                CargarViewBagComun("Error al generar el resumen del juego: " + ex.Message);
                return View("Index"); // Vuelve a Index con mensaje de error.
            }
        }
    }
}