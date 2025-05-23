using CL_ProyectoFinalPOO.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using CL_ProyectoFinalPOO.Interfaces;

namespace MVC_ProyectoFinalPOO.Services
{
    public class JuegoService : IJuegoService
    {
        private Juego _juegoActual; // Instancia del juego en curso. Será gestionada por este servicio Singleton.
        private readonly HomeService _homeService; // Para limpiar la configuración de jugadores

        // Constructor para Inyección de Dependencias
        public JuegoService(HomeService homeService)
        {
            _homeService = homeService;
        }

        public bool EstaJuegoActivo()
        {
            return _juegoActual != null && CL_ProyectoFinalPOO.Clases.Juego.Jugadores != null && CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Any();
        }

        public void IniciarJuego(List<Jugador> jugadoresConfigurados)
        {
            if (jugadoresConfigurados == null || !jugadoresConfigurados.Any())
            {
                Debug.WriteLine("JuegoService.IniciarJuego: Intento de iniciar juego sin jugadores configurados.");
                throw new ArgumentException("Se requiere al menos un jugador configurado para iniciar el juego.");
            }

            try
            {
                Debug.WriteLine($"JuegoService.IniciarJuego: Iniciando juego con {jugadoresConfigurados.Count} jugadores.");
                _juegoActual = new Juego();

                CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Clear();
                CL_ProyectoFinalPOO.Clases.Juego.IndiceJugador = 0;

                foreach (var jConf in jugadoresConfigurados)
                {
                    var nuevoJugador = new Jugador(jConf.Nickname, jConf.ApuestaInicial, _juegoActual);
                    _juegoActual.AsignarPuntosSegunApuesta(nuevoJugador);
                    CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Add(nuevoJugador);
                    Debug.WriteLine($"JuegoService.IniciarJuego: Jugador '{nuevoJugador.Nickname}' añadido con {nuevoJugador.Puntos} puntos.");
                }

                _juegoActual.IniciarRonda();
                _homeService.LimpiarConfiguracionJugadores();
                Debug.WriteLine("JuegoService.IniciarJuego: Juego iniciado y configuración de HomeService limpiada.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.IniciarJuego: Error crítico - {ex.Message}");
                _juegoActual = null;
                throw new Exception("Error crítico en JuegoService al intentar iniciar el juego.", ex);
            }
        }

        public Jugador ObtenerJugadorActual()
        {
            if (!EstaJuegoActivo())
            {
                Debug.WriteLine("JuegoService.ObtenerJugadorActual: No hay juego activo.");
                return null;
            }
            return CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Count == 0 ? null : CL_ProyectoFinalPOO.Clases.Juego.Jugadores[CL_ProyectoFinalPOO.Clases.Juego.IndiceJugador];
        }

        // MODIFICADO: La firma de retorno ya no incluye 'object cartaViewModel'
        public (Carta carta, int puntos) CogerCarta()
        {
            if (!EstaJuegoActivo())
            {
                Debug.WriteLine("JuegoService.CogerCarta: Intento de coger carta sin juego activo.");
                throw new InvalidOperationException("El juego no ha sido iniciado o no hay jugadores.");
            }

            var jugadorActual = ObtenerJugadorActual();
            if (jugadorActual == null)
            {
                Debug.WriteLine("JuegoService.CogerCarta: No se pudo determinar el jugador actual.");
                throw new InvalidOperationException("No se pudo determinar el jugador actual.");
            }

            try
            {
                var carta = _juegoActual.ObtenerCarta();
                int puntosObtenidos = 0;

                if (carta != null)
                {
                    puntosObtenidos = _juegoActual.AplicarEfectoCartas(carta);

                    jugadorActual.Puntos += puntosObtenidos;
                    jugadorActual.L_cartas_jugador.Add(carta);

                    // ELIMINADO: La creación del objeto anónimo cartaViewModel se ha quitado de aquí.
                    // El controlador será responsable de construir el ViewModel si es necesario.

                    Debug.WriteLine($"JuegoService.CogerCarta: Jugador '{jugadorActual.Nickname}' cogió '{carta.Nombre}'. Puntos efecto: {puntosObtenidos}. Puntos totales: {jugadorActual.Puntos}.");
                }
                else
                {
                    Debug.WriteLine("JuegoService.CogerCarta: No se pudo obtener una carta (mazo posiblemente vacío).");
                }

                _juegoActual.ValidarYDispararEventos(null);

                // MODIFICADO: Se devuelve solo la carta y los puntos.
                return (carta, puntosObtenidos);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.CogerCarta: Error - {ex.Message}");
                throw new Exception("Error en JuegoService al procesar CogerCarta.", ex);
            }
        }

        public void PasarTurno()
        {
            if (!EstaJuegoActivo())
            {
                Debug.WriteLine("JuegoService.PasarTurno: Intento de pasar turno sin juego activo.");
                throw new InvalidOperationException("El juego no ha sido iniciado.");
            }
            try
            {
                Jugador liderInicial = _juegoActual.ObtenerLider();
                _juegoActual.ValidarYDispararEventos(liderInicial);

                if (!JuegoTerminado() && CL_ProyectoFinalPOO.Clases.Juego.Jugadores != null && CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Any())
                {
                    _juegoActual.PasarTurno();
                    Debug.WriteLine($"JuegoService.PasarTurno: Turno pasado. Nuevo jugador actual: {ObtenerJugadorActual()?.Nickname ?? "Ninguno"}.");
                }
                else
                {
                    Debug.WriteLine("JuegoService.PasarTurno: No se pasó el turno, el juego terminó o no hay jugadores.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.PasarTurno: Error - {ex.Message}");
                throw new Exception("Error en JuegoService al intentar pasar el turno.", ex);
            }
        }

        public Jugador FinalizarJuego()
        {
            // La llamada a _juegoActual.FinalizarJuego() ya existe dentro del bloque try
            // No es necesario llamarla dos veces.
            if (!EstaJuegoActivo() && _juegoActual == null) // Si no está activo porque _juegoActual es null
            {
                Debug.WriteLine("JuegoService.FinalizarJuego: No hay juego (_juegoActual es null) para finalizar.");
                return null;
            }

            try
            {
                var ganador = _juegoActual.ObtenerLider(); // Esta es la llamada principal
                Debug.WriteLine($"JuegoService.FinalizarJuego: Juego finalizado. Ganador: {ganador?.Nickname ?? "Ninguno"}.");
                return ganador;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.FinalizarJuego: Error - {ex.Message}");
                throw new Exception("Error en JuegoService al finalizar el juego.", ex);
            }
        }

        public List<Jugador> ObtenerJugadores()
        {
            if (_juegoActual != null)
            {
                return CL_ProyectoFinalPOO.Clases.Juego.Jugadores ?? new List<Jugador>();
            }
            return new List<Jugador>();
        }

        public List<string> ObtenerHistorial()
        {
            if (_juegoActual == null || _juegoActual.Historial == null)
            {
                return new List<string>();
            }
            try
            {
                return _juegoActual.Historial.ObtenerNotificaciones().ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.ObtenerHistorial: Error - {ex.Message}");
                return new List<string> { $"Error al obtener historial: {ex.Message}" };
            }
        }

        public bool JuegoTerminado()
        {
            if (!EstaJuegoActivo())
            {
                Debug.WriteLine("JuegoService.JuegoTerminado: No hay juego activo, se considera terminado.");
                return true;
            }
            try
            {
                bool mazosAgotados = _juegoActual.AgotadasResto && _juegoActual.AgotadasCastigo && _juegoActual.AgotadasPremio;
                bool pocosJugadores = CL_ProyectoFinalPOO.Clases.Juego.Jugadores == null || CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Count < _juegoActual.JugadoresMin;

                bool terminado = mazosAgotados || pocosJugadores;

                Debug.WriteLineIf(terminado, "JuegoService.JuegoTerminado: El juego ha terminado.");
                Debug.WriteLineIf(!terminado, "JuegoService.JuegoTerminado: El juego NO ha terminado.");
                return terminado;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.JuegoTerminado: Error - {ex.Message}");
                return true;
            }
        }

        public int TotalCartasEnMazo()
        {
            if (!EstaJuegoActivo())
            {
                return 0;
            }
            try
            {
                return CL_ProyectoFinalPOO.Clases.Juego.L_cartas_resto.Count +
                       CL_ProyectoFinalPOO.Clases.Juego.L_cartas_castigo.Count +
                       CL_ProyectoFinalPOO.Clases.Juego.L_cartas_premio.Count;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.TotalCartasEnMazo: Error - {ex.Message}");
                return 0;
            }
        }

        public void ReiniciarJuego()
        {
            try
            {
                Debug.WriteLine("JuegoService.ReiniciarJuego: Reiniciando estado del juego.");
                _juegoActual = null;

                CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Clear();
                CL_ProyectoFinalPOO.Clases.Juego.IndiceJugador = 0;
                // Las barajas estáticas se recargarán cuando se cree un `new Juego()` en `IniciarJuego`.

                _homeService.LimpiarConfiguracionJugadores();
                Debug.WriteLine("JuegoService.ReiniciarJuego: Juego reiniciado y configuración de HomeService limpiada.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.ReiniciarJuego: Error - {ex.Message}");
                throw new Exception("Error en JuegoService al reiniciar el juego.", ex);
            }
        }

        public void ComenzarNuevaRondaConJugadoresActuales()
        {
            if (!EstaJuegoActivo())
            {
                Debug.WriteLine("JuegoService.ComenzarNuevaRonda: No hay juego activo para iniciar nueva ronda.");
                throw new InvalidOperationException("No hay un juego activo para iniciar una nueva ronda. Configure un nuevo juego.");
            }
            if (CL_ProyectoFinalPOO.Clases.Juego.Jugadores == null ||
                !CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Any() ||
                CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Count < _juegoActual.JugadoresMin)
            {
                Debug.WriteLine("JuegoService.ComenzarNuevaRonda: No hay suficientes jugadores para nueva ronda.");
                throw new InvalidOperationException("No hay suficientes jugadores actuales para comenzar una nueva ronda.");
            }

            try
            {
                Debug.WriteLine("JuegoService.ComenzarNuevaRonda: Iniciando nueva ronda con jugadores actuales.");
                _juegoActual.IniciarRonda();
                Debug.WriteLine("JuegoService.ComenzarNuevaRonda: Nueva ronda iniciada.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.ComenzarNuevaRonda: Error - {ex.Message}");
                throw new Exception("Error en JuegoService al comenzar una nueva ronda.", ex);
            }
        }
    }
}