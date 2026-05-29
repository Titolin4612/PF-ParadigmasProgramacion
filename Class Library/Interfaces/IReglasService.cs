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

using System.Collections.Generic;
using CL_ProyectoFinalPOO.Clases;

namespace CL_ProyectoFinalPOO.Interfaces
{
    public interface IReglasService
    {
        List<CartaJuego> ObtenerCartasJuego();
        List<CartaPremio> ObtenerCartasPremio();
        List<CartaCastigo> ObtenerCartasCastigo();
    }
}