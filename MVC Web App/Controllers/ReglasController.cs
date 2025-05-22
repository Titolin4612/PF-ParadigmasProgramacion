using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Services; // Necesario para ReglasService y JuegoService
using System;
using System.Collections.Generic; // Para List
// using CL_ProyectoFinalPOO.Clases; // No es estrictamente necesario aquí si solo pasas dynamic

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class ReglasController : Controller
    {
        private readonly ReglasService _reglasService;
        private readonly JuegoService _juegoService; // Añadir referencia a JuegoService

        public ReglasController()
        {
            _reglasService = new ReglasService();
            _juegoService = JuegoService.Instance; // Obtener instancia de JuegoService (asumiendo Singleton)
        }

        public IActionResult BarajaCatalogo()
        {
            try
            {
                ViewData["CartasJuego"] = _reglasService.ObtenerCartasJuego();
                ViewData["CartasPremio"] = _reglasService.ObtenerCartasPremio();
                ViewData["CartasCastigo"] = _reglasService.ObtenerCartasCastigo();

                // --- INICIO DE LA MODIFICACIÓN ---
                // Determinar si hay un juego activo.
                // Usaremos la variable estática 'juegoIniciado' de JuegoController como indicador.
                // Si prefieres un método en JuegoService (ej. _juegoService.ExistePartidaActiva()),
                // podrías llamarlo aquí en su lugar.
                ViewBag.HayJuegoActivo = JuegoController.juegoIniciado;
                // --- FIN DE LA MODIFICACIÓN ---

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                // --- INICIO DE LA MODIFICACIÓN (Opcional pero recomendado en catch) ---
                // Incluso si hay un error cargando cartas, aún podrías querer saber si hay un juego activo
                // para decidir si mostrar el botón "Ir al Juego" (aunque podría no ser relevante si hay error).
                // O puedes optar por no mostrarlo si hay un error general.
                // Por consistencia, lo añadimos aquí también.
                ViewBag.HayJuegoActivo = JuegoController.juegoIniciado;
                // --- FIN DE LA MODIFICACIÓN ---
                return View();
            }
        }

        public IActionResult Index()
        {
            // Si la vista Index de Reglas también necesita saber si hay un juego activo
            // para algún otro propósito, puedes añadir la lógica aquí también.
            // Ejemplo: ViewBag.HayJuegoActivo = JuegoController.juegoIniciado;
            return View();
        }
    }
}