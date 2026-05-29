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

using System;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;

namespace CL_ProyectoFinalPOO.Eventos
{
    public class Publisher_Eventos_Juego
    {
        public delegate void DelegadoEventoGeneral();
        public delegate void DelegadoEventoParametroJugador(Jugador jugador);

        public event DelegadoEventoGeneral? InicioPartida;
        public event DelegadoEventoParametroJugador? FinPartida;

        public void NotificarInicioPartida()
        {
            InicioPartida?.Invoke();
        }

        public void NotificarFinPartida(Jugador ganador)
        {
            FinPartida?.Invoke(ganador);
        }
    }
}