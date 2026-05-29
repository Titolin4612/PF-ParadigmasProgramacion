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

namespace MVC_ProyectoFinalPOO.Entities
{
    public class Estadistica
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int PartidasJugadas { get; set; }
        public int PartidasGanadas { get; set; }
        public int PartidasPerdidas { get; set; }
        public int PuntosTotales { get; set; }
        public int MejorPuntuacion { get; set; }
        public double PromedioPuntos { get; set; }

        public DateTime UltimaPartida { get; set; } = DateTime.UtcNow;
    }
}
