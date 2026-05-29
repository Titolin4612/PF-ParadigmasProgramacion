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

using MVC_ProyectoFinalPOO.Models;
using CL_ProyectoFinalPOO.Clases;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Diagnostics;
using CL_ProyectoFinalPOO.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MVC_ProyectoFinalPOO.Data;
using MVC_ProyectoFinalPOO.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MVC_ProyectoFinalPOO.Services
{
    public class HomeService : IHomeService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<HomeService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionKey = "ListaJugadoresConfig";

        public HomeService(AppDbContext dbContext, ILogger<HomeService> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        private List<Jugador> GetJugadoresFromSession()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return new List<Jugador>();

            var json = session.GetString(SessionKey);
            if (string.IsNullOrEmpty(json)) return new List<Jugador>();

            try
            {
                return JsonSerializer.Deserialize<List<Jugador>>(json) ?? new List<Jugador>();
            }
            catch
            {
                return new List<Jugador>();
            }
        }

        private void SaveJugadoresToSession(List<Jugador> jugadores)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString(SessionKey, JsonSerializer.Serialize(jugadores));
        }

        public void LimpiarConfiguracionJugadores()
        {
            _httpContextAccessor.HttpContext?.Session.Remove(SessionKey);
            _logger.LogInformation("Configuración de jugadores limpiada");
        }

        public bool BuscarUsuario(string usuario, string contraseña = null)
        {
            if (string.IsNullOrWhiteSpace(usuario))
            {
                return false;
            }

            var user = _dbContext.Usuarios.FirstOrDefault(u => u.Nickname.ToLower() == usuario.ToLower());

            if (user == null)
            {
                return false;
            }

            if (contraseña != null)
            {
                return BCrypt.Net.BCrypt.Verify(contraseña, user.PasswordHash);
            }
            return true;
        }

        public void RegistrarUsuario(string usuario, string contraseña)
        {
            if (string.IsNullOrWhiteSpace(usuario) || usuario.Length < 4)
            {
                throw new ArgumentException("El nickname debe tener al menos 4 caracteres.");
            }
            if (string.IsNullOrWhiteSpace(contraseña) || contraseña.Length < 6)
            {
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");
            }
            if (_dbContext.Usuarios.Any(u => u.Nickname.ToLower() == usuario.ToLower()))
            {
                throw new InvalidOperationException($"El usuario '{usuario}' ya está registrado.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(contraseña);

            var usuarioNuevo = new Usuario
            {
                Nickname = usuario,
                PasswordHash = passwordHash,
                FechaRegistro = DateTime.UtcNow,
                Estadistica = new Estadistica()
            };

            _dbContext.Usuarios.Add(usuarioNuevo);
            _dbContext.SaveChanges();

            _logger.LogInformation("Usuario '{Usuario}' registrado exitosamente", usuario);
        }

        public string GenerarToken(string usuario)
        {
            var user = _dbContext.Usuarios.FirstOrDefault(u => u.Nickname.ToLower() == usuario.ToLower());
            if (user == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

            user.UltimoLogin = DateTime.UtcNow;
            _dbContext.SaveChanges();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Nickname),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void AgregarJugadorConfigurado(string nickname, int apuesta)
        {
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 4)
                throw new ArgumentException("El Nickname es inválido. Debe tener al menos 4 caracteres.");
            if (apuesta < 10 || apuesta > 1000)
                throw new ArgumentException("La apuesta debe estar entre 10 y 1000 puntos.");

            Juego juegoReglas = new Juego();
            var jugadoresActuales = GetJugadoresFromSession();

            if (jugadoresActuales.Count >= juegoReglas.JugadoresMax)
                throw new InvalidOperationException($"No se pueden agregar más de {juegoReglas.JugadoresMax} jugadores.");

            if (jugadoresActuales.Any(j => j.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("El nickname ingresado ya está en uso por otro jugador en la configuración actual.");

            try
            {
                Juego juegoTemporalParaConstructor = new Juego();
                var nuevoJugador = new Jugador(nickname, apuesta, juegoTemporalParaConstructor);

                jugadoresActuales.Add(nuevoJugador);
                SaveJugadoresToSession(jugadoresActuales);
                _logger.LogInformation("Jugador '{Nickname}' agregado a la configuración", nickname);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar jugador '{Nickname}'", nickname);
                throw new Exception("Error interno del servicio al intentar agregar el jugador.", ex);
            }
        }

        public void EliminarUltimoJugadorConfigurado()
        {
            var jugadoresActuales = GetJugadoresFromSession();
            if (jugadoresActuales.Count == 0)
                throw new InvalidOperationException("No hay jugadores en la configuración actual para eliminar.");

            var jugadorEliminado = jugadoresActuales.Last();
            jugadoresActuales.RemoveAt(jugadoresActuales.Count - 1);
            SaveJugadoresToSession(jugadoresActuales);
            _logger.LogInformation("Último jugador '{Nickname}' eliminado de la configuración", jugadorEliminado.Nickname);
        }

        public List<Jugador> ValidarConfiguracionJugadoresParaJuego()
        {
            var jugadoresActuales = GetJugadoresFromSession();
            Juego juegoReglas = new Juego();
            int minJugadores = juegoReglas.JugadoresMin;
            int maxJugadores = juegoReglas.JugadoresMax;

            if (jugadoresActuales.Count < minJugadores || jugadoresActuales.Count > maxJugadores)
            {
                string mensajeError = $"Se requieren entre {minJugadores} y {maxJugadores} jugadores para iniciar el juego. Actualmente hay {jugadoresActuales.Count} jugadores configurados.";
                throw new InvalidOperationException(mensajeError);
            }

            return new List<Jugador>(jugadoresActuales);
        }

        public List<Jugador> ObtenerJugadoresConfigurados()
        {
            return GetJugadoresFromSession();
        }
    }
}
