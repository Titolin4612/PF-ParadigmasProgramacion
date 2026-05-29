/*
 * Copyright (C) 2026 Santiago Hernandez M
 *
 * This file is part of Blessings & Curses.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * See the LICENSE file for details.
 */

using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Services;
using CL_ProyectoFinalPOO.Interfaces;
using System;
using Microsoft.Extensions.Logging;

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class ReglasController(ReglasService reglasService, IJuegoService juegoService, ILogger<ReglasController> logger) : Controller
    {
        private readonly ReglasService _reglasService = reglasService;
        private readonly IJuegoService _juegoService = juegoService;
        private readonly ILogger<ReglasController> _logger = logger;

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
                _logger.LogError(ex, "Error al cargar el catálogo de cartas");
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
