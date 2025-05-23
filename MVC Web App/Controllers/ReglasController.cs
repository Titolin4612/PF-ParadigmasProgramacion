using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Services; 
using System;
using System.Collections.Generic; 

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class ReglasController : Controller
    {
        private readonly ReglasService _reglasService;
        private readonly JuegoService _juegoService; 

        public ReglasController()
        {
            _reglasService = new ReglasService();
            _juegoService = JuegoService.Instance; 
        }

        public IActionResult BarajaCatalogo()
        {
            try
            {
                ViewData["CartasJuego"] = _reglasService.ObtenerCartasJuego();
                ViewData["CartasPremio"] = _reglasService.ObtenerCartasPremio();
                ViewData["CartasCastigo"] = _reglasService.ObtenerCartasCastigo();
                ViewBag.HayJuegoActivo = JuegoController.juegoIniciado;
    

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                ViewBag.HayJuegoActivo = JuegoController.juegoIniciado;
    
                return View();
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}