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

using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Services;
using System.Text.Json;
using System.Diagnostics;
using CL_ProyectoFinalPOO.Interfaces;

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class HomeController(IHomeService homeService, IJuegoService juegoService) : Controller
    {
        private readonly IHomeService _homeService = homeService;
        private readonly IJuegoService _juegoService = juegoService;

        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString("UsuarioSesion");
            if (!string.IsNullOrEmpty(usuario))
            {
                ViewBag.Players = _homeService.ObtenerJugadoresConfigurados();
            }
            else
            {
                ViewBag.Players = null;
            }

            ViewBag.Error = TempData["ErrorGeneral"] as string;
            ViewBag.Success = TempData["MensajeExito"] as string;
            return View();
        }

        [HttpPost]
        public IActionResult AgregarJugador(string nickname, int apuesta)
        {
            if (!SesionActiva())
            {
                TempData["ErrorEnLogin"] = "Debes iniciar sesión para agregar un jugador.";
                return RedirectToAction("Login");
            }

            try
            {
                _homeService.AgregarJugadorConfigurado(nickname, apuesta);
                TempData["MensajeExito"] = $"Jugador '{nickname}' agregado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorGeneral"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarJugador()
        {
            if (!SesionActiva())
            {
                TempData["ErrorEnLogin"] = "Debes iniciar sesión para eliminar un jugador.";
                return RedirectToAction("Login");
            }

            try
            {
                _homeService.EliminarUltimoJugadorConfigurado();
                TempData["MensajeExito"] = "Jugador eliminado.";
            }
            catch (Exception ex)
            {
                TempData["ErrorGeneral"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Play()
        {
            if (!SesionActiva())
            {
                TempData["ErrorEnLogin"] = "Debes iniciar sesión para jugar.";
                return RedirectToAction("Login");
            }

            try
            {
                var jugadores = _homeService.ValidarConfiguracionJugadoresParaJuego();
                HttpContext.Session.SetString("ListaJugadoresConfig", JsonSerializer.Serialize(jugadores));
                _juegoService.IniciarJuego(jugadores);
                return RedirectToAction("Index", "Juego");
            }
            catch (Exception ex)
            {
                TempData["ErrorGeneral"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Login()
        {
            ViewBag.Error = TempData["ErrorEnLogin"];
            return View();
        }

        [HttpPost]
        
        public IActionResult Login(string nickname, string contraseña)
        {
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 4 ||
                string.IsNullOrWhiteSpace(contraseña) || contraseña.Length < 6)
            {
                ViewBag.Error = "Credenciales inválidas.";
                return View();
            }

            if (!_homeService.BuscarUsuario(nickname))
            {
                TempData["ErrorEnRegistro"] = $"El usuario '{nickname}' no está registrado. Regístrate.";
                TempData["PerfilNickname"] = nickname;
                return RedirectToAction("Signup");
            }

            if (!_homeService.BuscarUsuario(nickname, contraseña))
            {
                ViewBag.Error = "Nickname o contraseña incorrectos.";
                return View();
            }

            HttpContext.Session.SetString("UsuarioSesion", nickname);
            var token = _homeService.GenerarToken(nickname);
            HttpContext.Session.SetString("JwtToken", token);
            TempData["MensajeExito"] = $"¡Bienvenido {nickname}!";
            return RedirectToAction("Index");
        }

        public IActionResult Signup()
        {
            ViewBag.Error = TempData["ErrorEnSignup"];
            ViewBag.Prompt = TempData["ErrorEnRegistro"];
            ViewBag.PerfilNickname = TempData["PerfilNickname"];
            return View();
        }

        [HttpPost]
        
        public IActionResult Signup(string nickname, string contraseña)
        {
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 4 ||
                string.IsNullOrWhiteSpace(contraseña) || contraseña.Length < 6)
            {
                TempData["ErrorEnSignup"] = "El nickname debe tener mínimo 4 caracteres y la contraseña 6.";
                TempData["PerfilNickname"] = nickname;
                return RedirectToAction("Signup");
            }

            if (_homeService.BuscarUsuario(nickname))
            {
                TempData["ErrorEnSignup"] = $"El nickname '{nickname}' ya está registrado.";
                return RedirectToAction("Signup");
            }

            try
            {
                _homeService.RegistrarUsuario(nickname, contraseña);
                HttpContext.Session.SetString("UsuarioSesion", nickname);
                var token = _homeService.GenerarToken(nickname);
                HttpContext.Session.SetString("JwtToken", token);
                TempData["MensajeExito"] = $"¡Cuenta creada! Bienvenido, {nickname}.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorEnSignup"] = ex.Message;
                TempData["PerfilNickname"] = nickname;
                return RedirectToAction("Signup");
            }
        }

        [HttpPost]
        
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["MensajeExito"] = "Has cerrado sesión correctamente.";
            return RedirectToAction("Login");
        }

        public IActionResult Privacy() => View();

        public IActionResult Reglas()
        {
            if (!SesionActiva())
            {
                TempData["ErrorEnLogin"] = "Debes iniciar sesión para ver las reglas.";
                return RedirectToAction("Login");
            }
            return View();
        }

        private bool SesionActiva()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioSesion"));
        }
    }
}
