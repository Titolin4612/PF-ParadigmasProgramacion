using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Services; // Necesario para ReglasService
using System;
using System.Collections.Generic; // Para List
// using CL_ProyectoFinalPOO.Clases; // No es estrictamente necesario aquí si solo pasas dynamic

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class ReglasController : Controller
    {
        private readonly ReglasService _reglasService;

        public ReglasController()
        {
            _reglasService = new ReglasService();
        }

        public IActionResult BarajaCatalogo()
        {
            try
            {
                ViewData["CartasJuego"] = _reglasService.ObtenerCartasJuego();
                ViewData["CartasPremio"] = _reglasService.ObtenerCartasPremio();
                ViewData["CartasCastigo"] = _reglasService.ObtenerCartasCastigo();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}