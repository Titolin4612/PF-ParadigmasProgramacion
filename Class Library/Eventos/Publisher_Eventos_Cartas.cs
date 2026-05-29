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
    public class Publisher_Eventos_Cartas
    {
        public delegate void DelegadoEventoGeneral();
        public delegate void DelegadoEventoParametroJugador2(Jugador jugador, Carta carta);

        public event DelegadoEventoGeneral? AgotadasPremio;
        public event DelegadoEventoGeneral? AgotadasCastigo;
        public event DelegadoEventoGeneral? AgotadasResto;
        public event DelegadoEventoParametroJugador2? CartasIniciales;

        public void NotificarAgotadasPremio()
        {
            AgotadasPremio?.Invoke();
        }

        public void NotificarAgotadasCastigo()
        {
            AgotadasCastigo?.Invoke();
        }

        public void NotificarAgotadasResto()
        {
            AgotadasResto?.Invoke();
        }

        public void NotificarCartasObtenidas(Jugador jugador, Carta carta)
        {
            CartasIniciales?.Invoke(jugador, carta);
        }
    }
}