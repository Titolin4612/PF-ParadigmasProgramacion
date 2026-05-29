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

using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace MVC_ProyectoFinalPOO.Services
{
    public class ReglasService : IReglasService
    {
        private readonly Baraja _baraja;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        public ReglasService(Baraja baraja, IMemoryCache cache)
        {
            _baraja = baraja;
            _cache = cache;
        }

        public List<CartaJuego> ObtenerCartasJuego()
        {
            try
            {
                return _cache.GetOrCreate("CartasJuego", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                    _baraja.CargarCartas(Baraja._rutaArchivoCartas);
                    return _baraja.CartasJuego;
                }) ?? new List<CartaJuego>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ReglasService ObtenerCartasJuego", ex);
            }
        }

        public List<CartaPremio> ObtenerCartasPremio()
        {
            try
            {
                return _cache.GetOrCreate("CartasPremio", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                    _baraja.CargarCartas(Baraja._rutaArchivoCartas);
                    return _baraja.CartasPremio;
                }) ?? new List<CartaPremio>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ReglasService ObtenerCartasPremio", ex);
            }
        }

        public List<CartaCastigo> ObtenerCartasCastigo()
        {
            try
            {
                return _cache.GetOrCreate("CartasCastigo", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                    _baraja.CargarCartas(Baraja._rutaArchivoCartas);
                    return _baraja.CartasCastigo;
                }) ?? new List<CartaCastigo>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ReglasService ObtenerCartasCastigo", ex);
            }
        }
    }
}
