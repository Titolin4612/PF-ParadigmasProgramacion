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

// MVC_ProyectoFinalPOO/Services/JuegoService.cs
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC_ProyectoFinalPOO.Services
{
    public class JuegoService(HomeService homeService, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache) : IJuegoService
    {
        private const string SessionGameKey = "JuegoActualCacheKey";
        private static readonly TimeSpan GameSlidingExpiration = TimeSpan.FromMinutes(30);
        private readonly HomeService _homeService = homeService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMemoryCache _memoryCache = memoryCache;

        public Juego? ObtenerInstanciaJuegoActual()
        {
            return ObtenerJuegoActual();
        }

        public bool EstaJuegoActivo()
        {
            var juegoActual = ObtenerJuegoActual();
            return juegoActual != null && juegoActual.Jugadores != null && juegoActual.Jugadores.Any();
        }

        public void IniciarJuego(List<Jugador> jugadoresConfigurados)
        {
            if (jugadoresConfigurados == null || !jugadoresConfigurados.Any())
            {
                throw new ArgumentException("Se requiere al menos un jugador configurado para iniciar el juego.");
            }

            try
            {
                var juegoActual = new Juego(); 

                juegoActual.Jugadores.Clear();
                juegoActual.IndiceJugador = 0;

                foreach (var jConf in jugadoresConfigurados)
                {

                    var nuevoJugador = new Jugador(jConf.Nickname, jConf.ApuestaInicial, juegoActual);
                    juegoActual.AsignarPuntosSegunApuesta(nuevoJugador);

                    juegoActual.Jugadores.Add(nuevoJugador);
                }

                juegoActual.IniciarRonda();
                GuardarJuegoActual(juegoActual);
                _homeService.LimpiarConfiguracionJugadores();
            }
            catch (Exception ex)
            {
                EliminarJuegoActual();
                throw new Exception("Error crítico en JuegoService al intentar iniciar el juego.", ex);
            }
        }

        public Jugador ObtenerJugadorActual()
        {
            var juegoActual = ObtenerJuegoActual();
            if (!EsJuegoActivo(juegoActual))
            {
                return null;
            }
            return juegoActual!.Jugadores.Count == 0 ?
                null : juegoActual.Jugadores[juegoActual.IndiceJugador];
        }

        public (Carta? carta, int puntos) CogerCarta()
        {
            var juegoActual = ObtenerJuegoActual();
            if (!EsJuegoActivo(juegoActual))
            {
                throw new InvalidOperationException("El juego no ha sido iniciado o no hay jugadores.");
            }

            var jugadorActual = juegoActual!.Jugadores.Count == 0 ?
                null : juegoActual.Jugadores[juegoActual.IndiceJugador];
            if (jugadorActual == null)
            {
                throw new InvalidOperationException("No se pudo determinar el jugador actual.");
            }

            try
            {
                var carta = juegoActual.ObtenerCarta();
                int puntosObtenidos = 0;

                if (carta != null)
                {
                    puntosObtenidos = juegoActual.AplicarEfectoCartas(carta);
                    jugadorActual.Puntos += puntosObtenidos;
                    jugadorActual.L_cartas_jugador.Add(carta);
                }

                juegoActual.ValidarYDispararEventos(null);
                GuardarJuegoActual(juegoActual);
                return (carta, puntosObtenidos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService al procesar CogerCarta.", ex);
            }
        }

        public void PasarTurno()
        {
            var juegoActual = ObtenerJuegoActual();
            if (!EsJuegoActivo(juegoActual))
            {
                throw new InvalidOperationException("El juego no ha sido iniciado.");
            }
            try
            {
                Jugador liderInicial = juegoActual!.ObtenerLider();
                juegoActual.ValidarYDispararEventos(liderInicial);

                if (!JuegoTerminado() && juegoActual.Jugadores != null && juegoActual.Jugadores.Any())
                {
                    juegoActual.PasarTurno();
                }

                GuardarJuegoActual(juegoActual);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService al intentar pasar el turno.", ex);
            }
        }

        public virtual Jugador FinalizarJuego()
        {
            var juegoActual = ObtenerJuegoActual();
            if (!EsJuegoActivo(juegoActual) && juegoActual == null)
            {
                return null;
            }

            try
            {
                var ganador = juegoActual!.ObtenerLider();
                GuardarJuegoActual(juegoActual);
                return ganador;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService al finalizar el juego.", ex);
            }
        }

        public List<Jugador> ObtenerJugadores()
        {
            var juegoActual = ObtenerJuegoActual();
            if (juegoActual != null)
            {
                return juegoActual.Jugadores ?? new List<Jugador>();
            }
            return new List<Jugador>();
        }

        public List<string> ObtenerHistorial()
        {
            var juegoActual = ObtenerJuegoActual();
            if (juegoActual == null || juegoActual.Historial == null)
            {
                return new List<string>();
            }
            try
            {
                return juegoActual.Historial.ObtenerNotificaciones().ToList();
            }
            catch (Exception ex)
            {
                return new List<string> { $"Error al obtener historial: {ex.Message}" };
            }
        }

        public bool JuegoTerminado()
        {
            var juegoActual = ObtenerJuegoActual();
            if (!EsJuegoActivo(juegoActual))
            {
                return true;
            }
            try
            {

                bool mazosAgotados = juegoActual!.AgotadasResto && juegoActual.AgotadasCastigo && juegoActual.AgotadasPremio;
                bool pocosJugadores = juegoActual.Jugadores == null || juegoActual.Jugadores.Count < juegoActual.JugadoresMin;

                bool terminado = mazosAgotados || pocosJugadores;
                return terminado;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public int TotalCartasEnMazo()
        {
            var juegoActual = ObtenerJuegoActual();
            if (!EsJuegoActivo(juegoActual))
            {
                return 0;
            }
            try
            {
                return juegoActual!.L_cartas_resto.Count +
                       juegoActual.L_cartas_castigo.Count +
                       juegoActual.L_cartas_premio.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void ReiniciarJuego()
        {
            try
            {
                EliminarJuegoActual();

                _homeService.LimpiarConfiguracionJugadores();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService al reiniciar el juego.", ex);
            }
        }

        public void ComenzarNuevaRondaConJugadoresActuales()
        {
            var juegoActual = ObtenerJuegoActual();
            if (!EsJuegoActivo(juegoActual))
            {
                throw new InvalidOperationException("No hay un juego activo para iniciar una nueva ronda. Configure un nuevo juego.");
            }
            if (juegoActual!.Jugadores == null ||
                !juegoActual.Jugadores.Any() ||
                juegoActual.Jugadores.Count < juegoActual.JugadoresMin)
            {
                throw new InvalidOperationException("No hay suficientes jugadores actuales para comenzar una nueva ronda.");
            }

            try
            {
                juegoActual.IniciarRonda();
                GuardarJuegoActual(juegoActual);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService al comenzar una nueva ronda.", ex);
            }
        }

        private Juego? ObtenerJuegoActual()
        {
            var cacheKey = ObtenerCacheKey(crearSiNoExiste: false);
            if (cacheKey == null)
            {
                return null;
            }

            return _memoryCache.TryGetValue(cacheKey, out Juego? juego) ? juego : null;
        }

        private void GuardarJuegoActual(Juego juego)
        {
            var cacheKey = ObtenerCacheKey(crearSiNoExiste: true);
            if (cacheKey == null)
            {
                throw new InvalidOperationException("No se pudo acceder a la sesión para guardar la partida.");
            }

            _memoryCache.Set(cacheKey, juego, new MemoryCacheEntryOptions
            {
                SlidingExpiration = GameSlidingExpiration
            });
        }

        private void EliminarJuegoActual()
        {
            var cacheKey = ObtenerCacheKey(crearSiNoExiste: false);
            if (cacheKey != null)
            {
                _memoryCache.Remove(cacheKey);
            }

            _httpContextAccessor.HttpContext?.Session.Remove(SessionGameKey);
        }

        private string? ObtenerCacheKey(bool crearSiNoExiste)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                return null;
            }

            var sessionGameId = session.GetString(SessionGameKey);
            if (string.IsNullOrWhiteSpace(sessionGameId) && crearSiNoExiste)
            {
                sessionGameId = Guid.NewGuid().ToString("N");
                session.SetString(SessionGameKey, sessionGameId);
            }

            return string.IsNullOrWhiteSpace(sessionGameId) ? null : $"JuegoActual:{sessionGameId}";
        }

        private static bool EsJuegoActivo(Juego? juego)
        {
            return juego != null && juego.Jugadores != null && juego.Jugadores.Any();
        }
    }
}
