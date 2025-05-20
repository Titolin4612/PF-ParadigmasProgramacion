using Microsoft.AspNetCore.Mvc;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Eventos; // Necesario para Publisher_Eventosjuego
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System;
using System.Diagnostics;
using MVC_ProyectoFinalPOO.Servicios;

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class JuegoController : Controller
    {
        private static readonly JuegoService _juegoService = new();

        public IActionResult Index()
        {
            return View();
        }
    }
}