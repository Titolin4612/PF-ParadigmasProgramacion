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
        private List<Jugador> _listaJugadoresConfig = new List<Jugador>();

        // Singleton: Constructor privado
        public HomeService() { }

        // Singleton: Método para obtener la instancia
        public static HomeService Instance
        {
            get
            {
                
                if (_instance == null)
                {
                    _instance = new HomeService();
                }
                return _instance;
                
            }
        }

        public void LimpiarConfiguracionJugadores()
        {
            try
            {
                _listaJugadoresConfig.Clear();
                Debug.WriteLine("HomeService: Configuración de jugadores limpiada.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error en HomeService LimpiarConfiguracionJugadores", ex);
            }
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
                throw new Exception("Error en HomeService AgregarJugador", ex);
            }
        }

        public void EliminarJugador()
        {
            try
            {
                Debug.WriteLine($"HomeService.EliminarJugador: Eliminando jugador: {_listaJugadoresConfig.Last().Nickname}");
                _listaJugadoresConfig.RemoveAt(_listaJugadoresConfig.Count - 1);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("No hay jugadores para eliminar.");
            }
        }

        public List<Jugador> ValidarJugadores()
        {
                Juego juego1 = new Juego();
                if (_listaJugadoresConfig.Count < juego1.JugadoresMin || _listaJugadoresConfig.Count > juego1.JugadoresMax)
                {
                    Debug.WriteLine($"HomeService.ValidarJugadores: Número de jugadores ({_listaJugadoresConfig.Count}) no válido.");
                    throw new InvalidOperationException($"Se requieren entre {juego1.JugadoresMin} y {juego1.JugadoresMax} jugadores.");
                }
                return _listaJugadoresConfig;
        }

        public List<Jugador> ObtenerJugadores()
        {
            try
            {
                return _listaJugadoresConfig;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en HomeService ObtenerJugadores", ex);
            }
        }
    }
}