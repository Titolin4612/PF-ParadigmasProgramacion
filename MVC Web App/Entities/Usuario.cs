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
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nickname { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoLogin { get; set; }

        public ICollection<Partida> Partidas { get; set; } = new List<Partida>();
        public Estadistica? Estadistica { get; set; }
    }
}
