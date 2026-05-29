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
    public class Partida
    {
        public int Id { get; set; }
        public required string NombreGanador { get; set; }
        public int PuntosGanador { get; set; }
        public int CantidadJugadores { get; set; }
        public int CartasJugadas { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
