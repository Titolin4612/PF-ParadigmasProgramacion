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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO.Clases
{
    public class CartaJuego : Carta
    {
        public enum Rareza
        {
            Comun,
            Especial,
            Rara,
            Epica,
            Legendaria
        }

        // Atributos
        private Rareza rarezaCarta;
        // Valores rarezas, Reglas de negocio
        private static int raComun = -2;
        private static int raEspecial = -1;
        private static int raRara = 0;
        private static int raEpica = 1;
        private static int raLegendaria = 2;

        public Rareza RarezaCarta { get => rarezaCarta; set => rarezaCarta = value; }
        public static int RaComun { get => raComun;}
        public static int RaEspecial { get => raEspecial; }
        public static int RaRara { get => raRara;  }
        public static int RaEpica { get => raEpica; }
        public static int RaLegendaria { get => raLegendaria; }

        // Constructor
        public CartaJuego(string nombre, string descripcion, string mitologia, Rareza rareza, string imagenUrl) 
       : base(nombre, descripcion, mitologia, imagenUrl)
        {
            RarezaCarta = rareza;
        }

        public override int ObtenerPuntos()
        {
            return RarezaCarta switch
            {
                Rareza.Comun => RaComun,
                Rareza.Especial => RaEspecial,
                Rareza.Rara => RaRara,
                Rareza.Epica => RaEpica,
                Rareza.Legendaria => RaLegendaria,
                _ => 0
            };
        }

    }
}

