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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVC_ProyectoFinalPOO.Data;
using MVC_ProyectoFinalPOO.Entities;

namespace MVC_ProyectoFinalPOO.Services
{
    public interface ILeaderboardService
    {
        Task<List<LeaderboardEntry>> GetTopPlayersAsync(int count = 10);
        Task<LeaderboardEntry?> GetPlayerRankAsync(string nickname);
        Task<List<PartidaHistorialEntry>> GetHistorialPartidasAsync(int count = 50);
        Task GuardarPartidaAsync(string nombreGanador, int puntosGanador, int cantidadJugadores, int cartasJugadas);
        Task ActualizarEstadisticasAsync(string nickname, int puntos, bool esGanador);
    }

    public record LeaderboardEntry(
        int Posicion,
        string Nickname,
        int PartidasJugadas,
        int PartidasGanadas,
        double PromedioPuntos,
        int MejorPuntuacion
    );

    public record PartidaHistorialEntry(
        DateTime Fecha,
        string NombreGanador,
        int PuntosGanador,
        int CantidadJugadores,
        int CartasJugadas
    );

    public class LeaderboardService : ILeaderboardService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<LeaderboardService> _logger;

        public LeaderboardService(AppDbContext dbContext, ILogger<LeaderboardService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<LeaderboardEntry>> GetTopPlayersAsync(int count = 10)
        {
            try
            {
                var estadisticas = await _dbContext.Estadisticas
                    .Where(e => e.PartidasJugadas > 0)
                    .OrderByDescending(e => e.PartidasGanadas)
                    .ThenByDescending(e => e.PromedioPuntos)
                    .Take(count)
                    .ToListAsync();

                var entries = new List<LeaderboardEntry>();
                var posicion = 1;

                foreach (var est in estadisticas)
                {
                    var usuario = await _dbContext.Usuarios.FindAsync(est.UsuarioId);
                    if (usuario != null)
                    {
                        entries.Add(new LeaderboardEntry(
                            posicion++,
                            usuario.Nickname,
                            est.PartidasJugadas,
                            est.PartidasGanadas,
                            est.PromedioPuntos,
                            est.MejorPuntuacion
                        ));
                    }
                }

                return entries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener leaderboard");
                return new List<LeaderboardEntry>();
            }
        }

        public async Task<LeaderboardEntry?> GetPlayerRankAsync(string nickname)
        {
            try
            {
                var usuario = await _dbContext.Usuarios
                    .FirstOrDefaultAsync(u => u.Nickname.ToLower() == nickname.ToLower());

                if (usuario?.Estadistica == null)
                    return null;

                var posicion = await _dbContext.Estadisticas
                    .CountAsync(e => e.PartidasJugadas > 0 &&
                        (e.PartidasGanadas > usuario.Estadistica.PartidasGanadas ||
                         (e.PartidasGanadas == usuario.Estadistica.PartidasGanadas &&
                          e.PromedioPuntos > usuario.Estadistica.PromedioPuntos))) + 1;

                return new LeaderboardEntry(
                    posicion,
                    usuario.Nickname,
                    usuario.Estadistica.PartidasJugadas,
                    usuario.Estadistica.PartidasGanadas,
                    usuario.Estadistica.PromedioPuntos,
                    usuario.Estadistica.MejorPuntuacion
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rank del jugador {Nickname}", nickname);
                return null;
            }
        }

        public async Task<List<PartidaHistorialEntry>> GetHistorialPartidasAsync(int count = 50)
        {
            try
            {
                return await _dbContext.Partidas
                    .AsNoTracking()
                    .OrderByDescending(p => p.Fecha)
                    .Take(count)
                    .Select(p => new PartidaHistorialEntry(
                        p.Fecha,
                        p.NombreGanador,
                        p.PuntosGanador,
                        p.CantidadJugadores,
                        p.CartasJugadas
                    ))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial de partidas");
                return new List<PartidaHistorialEntry>();
            }
        }

        public async Task GuardarPartidaAsync(string nombreGanador, int puntosGanador, int cantidadJugadores, int cartasJugadas)
        {
            try
            {
                var usuario = await _dbContext.Usuarios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Nickname.ToLower() == nombreGanador.ToLower());

                _dbContext.Partidas.Add(new Partida
                {
                    NombreGanador = nombreGanador,
                    PuntosGanador = puntosGanador,
                    CantidadJugadores = cantidadJugadores,
                    CartasJugadas = cartasJugadas,
                    Fecha = DateTime.UtcNow,
                    UsuarioId = usuario?.Id
                });

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Partida guardada para ganador {NombreGanador}", nombreGanador);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar partida para ganador {NombreGanador}", nombreGanador);
                throw;
            }
        }

        public async Task ActualizarEstadisticasAsync(string nickname, int puntos, bool esGanador)
        {
            try
            {
                var usuario = await _dbContext.Usuarios
                    .Include(u => u.Estadistica)
                    .FirstOrDefaultAsync(u => u.Nickname.ToLower() == nickname.ToLower());

                if (usuario == null)
                {
                    _logger.LogWarning("Usuario {Nickname} no encontrado para actualizar estadísticas", nickname);
                    return;
                }

                if (usuario.Estadistica == null)
                {
                    usuario.Estadistica = new Estadistica();
                    _dbContext.Estadisticas.Add(usuario.Estadistica);
                }

                usuario.Estadistica.PartidasJugadas++;
                usuario.Estadistica.PuntosTotales += puntos;
                usuario.Estadistica.PromedioPuntos = (double)usuario.Estadistica.PuntosTotales / usuario.Estadistica.PartidasJugadas;
                usuario.Estadistica.UltimaPartida = DateTime.UtcNow;

                if (esGanador)
                {
                    usuario.Estadistica.PartidasGanadas++;
                }
                else
                {
                    usuario.Estadistica.PartidasPerdidas++;
                }

                if (puntos > usuario.Estadistica.MejorPuntuacion)
                {
                    usuario.Estadistica.MejorPuntuacion = puntos;
                }

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Estadísticas actualizadas para {Nickname}", nickname);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar estadísticas para {Nickname}", nickname);
            }
        }
    }
}
