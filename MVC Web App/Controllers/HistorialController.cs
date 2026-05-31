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

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class HistorialController(ILeaderboardService leaderboardService) : Controller
    {
        private readonly ILeaderboardService _leaderboardService = leaderboardService;

        public async Task<IActionResult> Index()
        {
            var partidas = await _leaderboardService.GetHistorialPartidasAsync();
            return View(partidas);
        }
    }
}
