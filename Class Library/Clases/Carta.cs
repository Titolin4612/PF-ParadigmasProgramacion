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
    public abstract class Carta : IObtenerPuntos
    {
        private string _nombre = null!;
        private string _mitologia = null!;
        private string _descripcion = null!;
        private string _imagenUrl = null!;

        // Accesores
        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Error, el nombre es inválido.");
                _nombre = value;
            }
        }
        public string Mitologia
        {
            get => _mitologia;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Error, la mitología es inválida.");
                _mitologia = value;
            }
        }
        public string Descripcion
        {
            get => _descripcion;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Error, la descripción es inválida.");
                _descripcion = value;
            }
        }

        public string ImagenUrl { get => _imagenUrl; set => _imagenUrl = value; }

        // Constructor
        public Carta(string nombre, string descripcion, string mitologia, string imagenurl)
        {
            Nombre = nombre;
            Mitologia = mitologia;
            Descripcion = descripcion;
            ImagenUrl = imagenurl;
        }

        // Metodo abtract
        public abstract int ObtenerPuntos();

       
    }
}
