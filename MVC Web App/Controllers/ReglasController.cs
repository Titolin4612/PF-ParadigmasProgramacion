using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Services;
using System;
using System.Diagnostics;

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class ReglasController : Controller
    {
        private readonly ReglasService _reglasService;
        private readonly JuegoService _juegoService; 

        public ReglasController(ReglasService reglasService, JuegoService juegoService)
        {
            _reglasService = reglasService;
            _juegoService = juegoService;
        }

        public IActionResult BarajaCatalogo()
        {
            try
            {
                ViewData["CartasJuego"] = _reglasService.ObtenerCartasJuego();
                ViewData["CartasPremio"] = _reglasService.ObtenerCartasPremio();
                ViewData["CartasCastigo"] = _reglasService.ObtenerCartasCastigo();
                ViewBag.HayJuegoActivo = _juegoService.EstaJuegoActivo();

                return View();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReglasController.BarajaCatalogo: Error - {ex.Message}");
                ViewBag.Error = "Error al cargar el catálogo de cartas: " + ex.Message;
                ViewBag.HayJuegoActivo = _juegoService.EstaJuegoActivo(); 
                return View(); 
            }
        }


        public IActionResult Index()
        {
            ViewBag.HayJuegoActivo = _juegoService.EstaJuegoActivo();
            return View();
        }
    }
}