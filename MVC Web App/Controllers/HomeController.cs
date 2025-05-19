using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Models;
using CL_ProyectoFinalPOO.Clases;
using System.Diagnostics;

namespace MVC_ProyectoFinalPOO.Controllers
{
    public class HomeController : Controller
    {
        // --- Estado en memoria ---
        private static Juego juego;
        private static List<Jugador> jugadores = new List<Jugador>();
        private static bool _barajaLoaded = false;

        public IActionResult Index(string error = null)
        {
            ViewBag.Players = jugadores;
            ViewBag.Error = error;
            return View();
        }

        [HttpPost]
        public IActionResult AddPlayer(string nickname, int apuesta)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 4)
                return RedirectToAction("Index", new { error = "Nickname inválido (>=4 chars)." });
            if (apuesta < 10 || apuesta > 1000)
                return RedirectToAction("Index", new { error = "Apuesta entre 10 y 1000." });
            if (jugadores.Count >= 4)
                return RedirectToAction("Index", new { error = "Máximo 4 jugadores." });

            // Inicializar juego y cargar baraja la primera vez
            if (!_barajaLoaded)
            {
                new Baraja().CargarCartas();       
                juego = new Juego();
                _barajaLoaded = true;
            }

            // Crear jugador (se asignan puntos según apuesta)
            try
            {
                var jugador = new Jugador(nickname, apuesta, juego);
                jugadores.Add(jugador);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemovePlayer()
        {
            if (jugadores.Count == 0)
                return RedirectToAction("Index", new { error = "No hay jugadores para eliminar." });

            jugadores.RemoveAt(0);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Play()
        {
            try
            {
                // Validar que el juego esté inicializado
                if (juego == null)
                {
                    return RedirectToAction("Index", new { error = "Agrega los jugadores para poder jugar." });
                }

                // Validar número de jugadores
                if (jugadores.Count < juego.JugadoresMin || jugadores.Count > juego.JugadoresMax)
                {
                    return RedirectToAction("Index", new { error = $"Se requieren entre {juego.JugadoresMin} y {juego.JugadoresMax} jugadores." });
                }

                // Validar que todos los jugadores tengan apuesta válida
                foreach (var player in jugadores)
                {
                    if (player.Puntos < 10 || player.Puntos > 1000)
                    {
                        return RedirectToAction("Index", new { error = "Todos los jugadores deben tener una apuesta entre 10 y 1000 puntos." });
                    }
                }

                // Empieza la partida
                //juego.BarajarCartas();
                //juego.RepartirCartasIniciales(juego.CartasPorJugador);
                return RedirectToAction("Index", "Juego");
            }
            catch (Exception ex)
            {
                // Esto capturará cualquier otro error inesperado
                return RedirectToAction("Index", new { error = "Ocurrió un error al iniciar el juego. Por favor intente nuevamente." });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public static List<Jugador> ObtenerJugadores()
        {
            return jugadores;
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
