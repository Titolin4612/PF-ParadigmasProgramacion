using Microsoft.AspNetCore.Mvc;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Eventos; // Necesario para Publisher_Eventos_Juego
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System;
using System.Diagnostics;

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class JuegoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}