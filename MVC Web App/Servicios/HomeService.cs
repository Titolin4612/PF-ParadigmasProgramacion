using MVC_ProyectoFinalPOO.Models;
using CL_ProyectoFinalPOO.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace MVC_ProyectoFinalPOO.Services
{
    public class HomeService
    {
        private static HomeService _instance;
        private static readonly object _lock = new object();
        private List<Jugador> _listaJugadoresConfig = new List<Jugador>();

        // Singleton: Constructor privado
        private HomeService() { }

        // Singleton: Método para obtener la instancia
        public static HomeService Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new HomeService();
                    }
                    return _instance;
                }
            }
        }

        public void LimpiarConfiguracionJugadores()
        {
            _listaJugadoresConfig.Clear();
            Debug.WriteLine("HomeService: Configuración de jugadores limpiada.");
        }

        public void AgregarJugador(string nickname, int apuesta)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 4)
                throw new ArgumentException("Nickname inválido, debe tener mínimo 4 caracteres.");
            if (apuesta < 10 || apuesta > 1000)
                throw new ArgumentException("Apuesta entre 10 y 1000.");
            if (_listaJugadoresConfig.Count >= 4)
                throw new InvalidOperationException("Máximo 4 jugadores.");
            if (_listaJugadoresConfig.Any(j => j.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Ese nickname ya está en uso.");

            try
            {
                Juego juegoTemporalParaConstructor = new Juego();
                var nuevoJugador = new Jugador(nickname, apuesta, juegoTemporalParaConstructor);
                _listaJugadoresConfig.Add(nuevoJugador);
                Debug.WriteLine($"HomeService.AgregarJugador: Jugador '{nickname}' añadido a configuración. Puntos asignados: {nuevoJugador.Puntos}. Total en config: {_listaJugadoresConfig.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR en HomeService.AgregarJugador: {ex.Message} {ex.StackTrace}");
                throw new InvalidOperationException("Error añadiendo jugador: " + ex.Message, ex);
            }
        }

        public void EliminarJugador()
        {
            if (!_listaJugadoresConfig.Any())
                throw new InvalidOperationException("No hay jugadores para eliminar.");

            Debug.WriteLine($"HomeService.EliminarJugador: Eliminando jugador: {_listaJugadoresConfig.Last().Nickname}");
            _listaJugadoresConfig.RemoveAt(_listaJugadoresConfig.Count - 1);
        }

        public List<Jugador> ValidarJugadores()
        {
            Juego juegoDeReglas = new Juego();
            if (_listaJugadoresConfig.Count < juegoDeReglas.JugadoresMin || _listaJugadoresConfig.Count > juegoDeReglas.JugadoresMax)
            {
                Debug.WriteLine($"HomeService.ValidarJugadores: Número de jugadores ({_listaJugadoresConfig.Count}) no válido.");
                throw new InvalidOperationException($"Se requieren entre {juegoDeReglas.JugadoresMin} y {juegoDeReglas.JugadoresMax} jugadores.");
            }
            return _listaJugadoresConfig;
        }

        public List<Jugador> ObtenerJugadores()
        {
            return _listaJugadoresConfig;
        }
    }
}