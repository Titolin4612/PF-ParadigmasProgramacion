// MVC_ProyectoFinalPOO/Controllers/JuegoController.cs
using Microsoft.AspNetCore.Mvc;
using CL_ProyectoFinalPOO.Clases;
using MVC_ProyectoFinalPOO.Services;
using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic; // Para List<T>

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class JuegoController : Controller
    {
        private readonly JuegoService _juegoService;
        // Ya no se necesita HomeService aquí si toda la configuración y limpieza post-inicio
        // es manejada por HomeController y JuegoService internamente.

        // Constructor para Inyección de Dependencias
        public JuegoController(JuegoService juegoService)
        {
            _juegoService = juegoService;
        }

        private void CargarViewBagComun(string mensajeErrorPersonalizado = null)
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
            else if (TempData.ContainsKey("ErrorGlobalJuego")) // Usar un TempData específico para JuegoController
            {
                ViewBag.MensajeError = TempData["ErrorGlobalJuego"] as string;
            }
        }

        public IActionResult Index()
        {
            try
            {
                if (!_juegoService.EstaJuegoActivo())
                {
                    // Si el usuario navega aquí directamente o después de un reinicio completo.
                    // HomeController es responsable de llamar a _juegoService.IniciarJuego().
                    CargarViewBagComun("No hay partida en curso. Por favor, configura los jugadores en la pantalla de inicio.");
                    // Asegurar que la vista Index.cshtml maneje bien este estado (ej. ocultar botones de acción de juego).
                    // ViewBag.JuegoTerminado será true en este caso por la lógica de _juegoService.JuegoTerminado().
                    return View("Index");
                }

                CargarViewBagComun();
                return View("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoController.Index: Error crítico - {ex.Message}");
                CargarViewBagComun("Error crítico al cargar la página del juego: " + ex.Message);
                ViewBag.JuegoTerminado = true; // En caso de error, asumir terminado para la UI.
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult CogerCarta()
        {
            if (!_juegoService.EstaJuegoActivo())
            {
                TempData["ErrorGlobal"] = "No se puede coger carta, la partida no está activa."; // Error para Home
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var (_, _, cartaViewModel) = _juegoService.CogerCarta();
                ViewBag.CartaRevelada = cartaViewModel;

                if (cartaViewModel == null && !_juegoService.JuegoTerminado())
                {
                    CargarViewBagComun("No quedan más cartas en el mazo o no se pudo obtener una carta.");
                }
                else
                {
                    CargarViewBagComun();
                }

                // La vista Index.cshtml ya reacciona a ViewBag.JuegoTerminado para mostrar
                // los botones de fin de juego. No es necesario llamar a FinalizarJuego() aquí explícitamente.
                // El servicio ya actualizó el estado del juego.
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
            if (!_juegoService.EstaJuegoActivo())
            {
                TempData["ErrorGlobal"] = "No se puede pasar turno, la partida no está activa.";
                return RedirectToAction("Index", "Home");
            }
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

        // El botón "Finalizar" en la UI si existe, debería ser para forzar fin si el juego aún no ha terminado.
        // Si el juego ya terminó (ViewBag.JuegoTerminado == true), la UI muestra "Nueva Ronda", "Reiniciar", "Ver Resumen".
        // Esta acción es si se quiere un botón explícito de "Terminar Juego Ahora".
        // La vista Index.cshtml actualmente no parece tener un botón para "Finalizar" la partida en curso,
        // sino que reacciona a _juegoService.JuegoTerminado().
        // Por lo tanto, el método [HttpPost] Finalizar() que tenías podría ser innecesario.
        // Si decides mantenerlo, debería llamarse por un botón específico.
        // Lo comentaré por ahora, ya que la lógica de terminación parece bien manejada por el estado.
        /*
        [HttpPost]
        public IActionResult FinalizarPartidaActual() // Nombre más descriptivo
        {
            if (!_juegoService.EstaJuegoActivo())
            {
                 TempData["ErrorGlobalJuego"] = "No hay juego activo para finalizar.";
                 return RedirectToAction("Index");
            }
            try
            {
                _juegoService.FinalizarJuego(); // El servicio actualiza su estado interno.
                // Redirigir a Index, que mostrará el estado de juego terminado.
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoController.FinalizarPartidaActual: Error - {ex.Message}");
                CargarViewBagComun("Error al intentar finalizar el juego: " + ex.Message);
                return View("Index");
            }
        }
        */

        [HttpPost]
        public IActionResult NuevaRonda()
        {
            // Se asume que esta acción solo está disponible si el juego anterior terminó.
            // EstaJuegoActivo podría seguir siendo true si _juegoActual no se ha anulado.
            if (!_juegoService.EstaJuegoActivo() || !_juegoService.JuegoTerminado())
            {
                TempData["ErrorGlobalJuego"] = "No se puede iniciar una nueva ronda en este momento.";
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
                TempData["ErrorGlobalJuego"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex) // Errores inesperados.
            {
                Debug.WriteLine($"JuegoController.NuevaRonda: Error crítico - {ex.Message}");
                TempData["ErrorGlobalJuego"] = "Error crítico al iniciar nueva ronda: " + ex.Message;
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
                TempData["ErrorGlobal"] = "Ocurrió un error al reiniciar el juego, por favor intente de nuevo: " + ex.Message;
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
                    // Opcionalmente, forzar finalización si se intenta ver resumen de juego activo.
                    // _juegoService.FinalizarJuego();
                    // O mostrar un mensaje de que el juego no ha terminado.
                    TempData["ErrorGlobalJuego"] = "La partida aún no ha finalizado para ver el resumen.";
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