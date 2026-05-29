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
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Interfaces;

namespace CL_ProyectoFinalPOO.Clases
{
    public class CartaCastigo : Carta
    {
        // Atributo
        private string _maleficio;
        // Valor carta
        private static int vCastigo = -5;
        
        // Accesor
        public string Maleficio
        {
            get => _maleficio;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Error, el maleficio es inválido.");
                _maleficio = value;
            }
        }
        public static int VCastigo { get => vCastigo; }

        // Constructor
        public CartaCastigo(string nombre, string descripcion, string mitologia, string maleficio, string imagenUrl) 
        : base(nombre, descripcion, mitologia, imagenUrl)
        {
            Maleficio = maleficio;
        }

        public override int ObtenerPuntos()
        {
            return VCastigo;
        }

    }
}
