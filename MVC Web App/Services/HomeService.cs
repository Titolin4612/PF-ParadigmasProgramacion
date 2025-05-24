
using MVC_ProyectoFinalPOO.Models; 
using CL_ProyectoFinalPOO.Clases; 
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using CL_ProyectoFinalPOO.Interfaces; 

namespace MVC_ProyectoFinalPOO.Services
{
    public class HomeService : IHomeService
    {

        private List<Jugador> _listaJugadoresConfigActual = new List<Jugador>();
        private static Dictionary<string, string> _usuariosRegistrados = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public void LimpiarConfiguracionJugadores()
        {
            try
            {
                _listaJugadoresConfigActual.Clear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeService: Error al limpiar configuración: {ex.Message}");
            }
        }

        public bool BuscarUsuario(string usuario, string contraseña = null)
        {
            if (string.IsNullOrWhiteSpace(usuario))
            {
                return false;
            }

            if (_usuariosRegistrados.ContainsKey(usuario))
            {
                if (contraseña != null)
                {
                    return _usuariosRegistrados[usuario] == contraseña;
                }
                return true;
            }
            return false;
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
            if (_usuariosRegistrados.ContainsKey(usuario))
            {
                throw new InvalidOperationException($"El usuario '{usuario}' ya está registrado.");
            }

            _usuariosRegistrados.Add(usuario, contraseña);
        }

        public void AgregarJugadorConfigurado(string nickname, int apuesta)
        {
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 4)
                throw new ArgumentException("El Nickname es inválido. Debe tener al menos 4 caracteres.");
            if (apuesta < 10 || apuesta > 1000) 
                throw new ArgumentException("La apuesta debe estar entre 10 y 1000 puntos.");

            Juego juegoReglas = new Juego();
            if (_listaJugadoresConfigActual.Count >= juegoReglas.JugadoresMax)
                throw new InvalidOperationException($"No se pueden agregar más de {juegoReglas.JugadoresMax} jugadores.");

            if (_listaJugadoresConfigActual.Any(j => j.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("El nickname ingresado ya está en uso por otro jugador en la configuración actual.");

            try
            {
                Juego juegoTemporalParaConstructor = new Juego();
                var nuevoJugador = new Jugador(nickname, apuesta, juegoTemporalParaConstructor);

                _listaJugadoresConfigActual.Add(nuevoJugador);
            }
            catch (Exception ex)
            {
                throw new Exception("Error interno del servicio al intentar agregar el jugador.", ex);
            }
        }

        public void EliminarUltimoJugadorConfigurado()
        {
            if (_listaJugadoresConfigActual.Count == 0)
                throw new InvalidOperationException("No hay jugadores en la configuración actual para eliminar.");

            try
            {
                var jugadorEliminado = _listaJugadoresConfigActual.Last();
                _listaJugadoresConfigActual.RemoveAt(_listaJugadoresConfigActual.Count - 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Error interno del servicio al intentar eliminar el último jugador.", ex);
            }
        }
        public List<Jugador> ValidarConfiguracionJugadoresParaJuego()
        {
            Juego juegoReglas = new Juego();
            int minJugadores = juegoReglas.JugadoresMin;
            int maxJugadores = juegoReglas.JugadoresMax;

            if (_listaJugadoresConfigActual.Count < minJugadores || _listaJugadoresConfigActual.Count > maxJugadores)
            {
                string mensajeError = $"Se requieren entre {minJugadores} y {maxJugadores} jugadores para iniciar el juego. Actualmente hay {_listaJugadoresConfigActual.Count} jugadores configurados.";
                throw new InvalidOperationException(mensajeError);
            }

            return new List<Jugador>(_listaJugadoresConfigActual);
        }

        public List<Jugador> ObtenerJugadoresConfigurados()
        {
            try
            {
                return new List<Jugador>(_listaJugadoresConfigActual);
            }
            catch (Exception ex)
            {
                throw new Exception("Error interno del servicio al intentar obtener la lista de jugadores configurados.", ex);
            }
        }
    }
}