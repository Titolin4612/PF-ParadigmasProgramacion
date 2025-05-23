// MVC_ProyectoFinalPOO/Services/HomeService.cs
using MVC_ProyectoFinalPOO.Models; // O donde estén Jugador/Juego
using CL_ProyectoFinalPOO.Clases; // Necesario si Jugador y Juego están en este namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using CL_ProyectoFinalPOO.Interfaces; // Para Debug.WriteLine

namespace MVC_ProyectoFinalPOO.Services
{
    public class HomeService : IHomeService
    {
        // Esta lista mantiene el estado de los jugadores que se están configurando actualmente.
        // Dado que HomeService es un Singleton (gestionado por DI), esta lista es compartida a nivel de aplicación.
        // Es importante gestionarla adecuadamente (p.ej., limpiarla después de su uso).
        private List<Jugador> _listaJugadoresConfigActual = new List<Jugador>();
        private static Dictionary<string, string> _usuariosRegistrados = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);


        // Limpia la lista de jugadores que estaban en proceso de configuración.
        public void LimpiarConfiguracionJugadores()
        {
            try
            {
                _listaJugadoresConfigActual.Clear();
                Debug.WriteLine("HomeService: Configuración de jugadores actual ha sido limpiada.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeService: Error al limpiar configuración: {ex.Message}");
                // Considera no lanzar una nueva excepción genérica aquí si el Debug.WriteLine es suficiente
                // o si el error no es realmente "crítico" para la operación de limpieza.
                // throw new Exception("Error crítico en HomeService al intentar limpiar la configuración de jugadores.", ex);
            }
        }

        public bool BuscarUsuario(string usuario, string contraseña = null)
        {
            if (string.IsNullOrWhiteSpace(usuario))
            {
                Debug.WriteLine("HomeService.BuscarUsuario: El nombre de usuario está vacío o es nulo.");
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
            Debug.WriteLine($"HomeService.RegistrarUsuario: Usuario '{usuario}' registrado exitosamente.");
        }


        // Agrega un jugador a la lista de configuración actual, realizando validaciones previas.
        public void AgregarJugadorConfigurado(string nickname, int apuesta)
        {
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 4)
                throw new ArgumentException("El Nickname es inválido. Debe tener al menos 4 caracteres.");
            if (apuesta < 10 || apuesta > 1000) // Límites de apuesta de ejemplo
                throw new ArgumentException("La apuesta debe estar entre 10 y 1000 puntos.");

            Juego juegoReglas = new Juego();
            if (_listaJugadoresConfigActual.Count >= juegoReglas.JugadoresMax)
                throw new InvalidOperationException($"No se pueden agregar más de {juegoReglas.JugadoresMax} jugadores.");

            if (_listaJugadoresConfigActual.Any(j => j.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("El nickname ingresado ya está en uso por otro jugador en la configuración actual.");

            try
            {
                Juego juegoTemporalParaConstructor = new Juego(); // Esta instancia es para el constructor de Jugador
                var nuevoJugador = new Jugador(nickname, apuesta, juegoTemporalParaConstructor);

                _listaJugadoresConfigActual.Add(nuevoJugador);
                Debug.WriteLine($"HomeService.AgregarJugadorConfigurado: Jugador '{nickname}' (Apuesta: {apuesta}, Puntos: {nuevoJugador.Puntos}) añadido a la configuración. Total en config: {_listaJugadoresConfigActual.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeService: Error al agregar jugador '{nickname}': {ex.Message}");
                throw new Exception("Error interno del servicio al intentar agregar el jugador.", ex);
            }
        }

        // Elimina el último jugador que fue agregado a la lista de configuración.
        public void EliminarUltimoJugadorConfigurado()
        {
            if (_listaJugadoresConfigActual.Count == 0)
                throw new InvalidOperationException("No hay jugadores en la configuración actual para eliminar.");

            try
            {
                var jugadorEliminado = _listaJugadoresConfigActual.Last();
                _listaJugadoresConfigActual.RemoveAt(_listaJugadoresConfigActual.Count - 1);
                Debug.WriteLine($"HomeService.EliminarUltimoJugadorConfigurado: Jugador '{jugadorEliminado.Nickname}' eliminado de la configuración. Total restantes: {_listaJugadoresConfigActual.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeService: Error al eliminar último jugador: {ex.Message}");
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
                Debug.WriteLine($"HomeService.ValidarConfiguracionJugadoresParaJuego: Validación fallida. {mensajeError}");
                throw new InvalidOperationException(mensajeError);
            }

            Debug.WriteLine($"HomeService.ValidarConfiguracionJugadoresParaJuego: Configuración de {_listaJugadoresConfigActual.Count} jugadores validada exitosamente.");
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
                Debug.WriteLine($"HomeService: Error al obtener jugadores configurados: {ex.Message}");
                throw new Exception("Error interno del servicio al intentar obtener la lista de jugadores configurados.", ex);
            }
        }
    }
}