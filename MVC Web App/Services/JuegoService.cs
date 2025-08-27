// MVC_ProyectoFinalPOO/Services/JuegoService.cs
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace MVC_ProyectoFinalPOO.Services
{
    public class JuegoService : IJuegoService
    {
        private Juego _juegoActual;
        private readonly HomeService _homeService;

        public JuegoService(HomeService homeService)
        {
            _homeService = homeService;
        }

        public Juego ObtenerInstanciaJuegoActual()
        {
            return _juegoActual;
        }

        public bool EstaJuegoActivo()
        {
            return _juegoActual != null && _juegoActual.Jugadores != null && _juegoActual.Jugadores.Any();
        }

        public void IniciarJuego(List<Jugador> jugadoresConfigurados)
        {
            if (jugadoresConfigurados == null || !jugadoresConfigurados.Any())
            {
                throw new ArgumentException("Se requiere al menos un jugador configurado para iniciar el juego.");
            }

            try
            {
                _juegoActual = new Juego(); 

                _juegoActual.Jugadores.Clear();
                _juegoActual.IndiceJugador = 0;

                foreach (var jConf in jugadoresConfigurados)
                {

                    var nuevoJugador = new Jugador(jConf.Nickname, jConf.ApuestaInicial, _juegoActual);
                    _juegoActual.AsignarPuntosSegunApuesta(nuevoJugador);

                    _juegoActual.Jugadores.Add(nuevoJugador);
                }

                _juegoActual.IniciarRonda();
                _homeService.LimpiarConfiguracionJugadores();
            }
            catch (Exception ex)
            {
                _juegoActual = null;
                throw new Exception("Error crítico en JuegoService al intentar iniciar el juego.", ex);
            }
        }

        public Jugador ObtenerJugadorActual()
        {
            if (!EstaJuegoActivo())
            {
                return null;
            }
            return _juegoActual.Jugadores.Count == 0 ?
                null : _juegoActual.Jugadores[_juegoActual.IndiceJugador];
        }

        public (Carta carta, int puntos) CogerCarta()
        {
            if (!EstaJuegoActivo())
            {
                throw new InvalidOperationException("El juego no ha sido iniciado o no hay jugadores.");
            }

            var jugadorActual = ObtenerJugadorActual();
            if (jugadorActual == null)
            {
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
                }
                else
                {
                }

                _juegoActual.ValidarYDispararEventos(null); 
                return (carta, puntosObtenidos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService al procesar CogerCarta.", ex);
            }
        }

        public void PasarTurno()
        {
            if (!EstaJuegoActivo())
            {
                throw new InvalidOperationException("El juego no ha sido iniciado.");
            }
            try
            {
                Jugador liderInicial = _juegoActual.ObtenerLider();
                _juegoActual.ValidarYDispararEventos(liderInicial);

                if (!JuegoTerminado() && _juegoActual.Jugadores != null && _juegoActual.Jugadores.Any())
                {
                    _juegoActual.PasarTurno();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService al intentar pasar el turno.", ex);
            }
        }

        public virtual Jugador FinalizarJuego()
        {
            if (!EstaJuegoActivo() && _juegoActual == null)
            {
                return null;
            }

            try
            {
                var ganador = _juegoActual.ObtenerLider();
                return ganador;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService al finalizar el juego.", ex);
            }
        }

        public List<Jugador> ObtenerJugadores()
        {
            if (_juegoActual != null)
            {
                return _juegoActual.Jugadores ?? new List<Jugador>();
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
                return new List<string> { $"Error al obtener historial: {ex.Message}" };
            }
        }

        public bool JuegoTerminado()
        {
            if (!EstaJuegoActivo())
            {
                return true;
            }
            try
            {

                bool mazosAgotados = _juegoActual.AgotadasResto && _juegoActual.AgotadasCastigo && _juegoActual.AgotadasPremio;
                bool pocosJugadores = _juegoActual.Jugadores == null || _juegoActual.Jugadores.Count < _juegoActual.JugadoresMin;

                bool terminado = mazosAgotados || pocosJugadores;
                return terminado;
            }
            catch (Exception ex)
            {
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
                return _juegoActual.L_cartas_resto.Count +
                       _juegoActual.L_cartas_castigo.Count +
                       _juegoActual.L_cartas_premio.Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void ReiniciarJuego()
        {
            try
            {
                _juegoActual = null; 

                _homeService.LimpiarConfiguracionJugadores();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService al reiniciar el juego.", ex);
            }
        }

        public void ComenzarNuevaRondaConJugadoresActuales()
        {
            if (!EstaJuegoActivo())
            {
                throw new InvalidOperationException("No hay un juego activo para iniciar una nueva ronda. Configure un nuevo juego.");
            }
            if (_juegoActual.Jugadores == null ||
                !_juegoActual.Jugadores.Any() ||
                _juegoActual.Jugadores.Count < _juegoActual.JugadoresMin)
            {
                throw new InvalidOperationException("No hay suficientes jugadores actuales para comenzar una nueva ronda.");
            }

            try
            {
                _juegoActual.IniciarRonda();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService al comenzar una nueva ronda.", ex);
            }
        }
    }
}