using CL_ProyectoFinalPOO.Clases;

namespace MVC_ProyectoFinalPOO.Services
{
    public class JuegoService
    {
        private static Juego juego;
        private static JuegoService _juego;

        public static JuegoService Instance
        {
            get
            {

                if (_juego == null)
                {
                    _juego = new JuegoService();
                }
                return _juego;

            }
        }

        public void IniciarJuego(List<Jugador> jugadoresConfigurados)
        {
            try
            {
                if (juego == null)
                {
                    juego = new Juego();
                }

                foreach (var jConf in jugadoresConfigurados)
                {
                    var nuevoJugador = new Jugador(jConf.Nickname, jConf.ApuestaInicial, juego);
                    juego.AsignarPuntosSegunApuesta(nuevoJugador);
                    Juego.Jugadores.Add(nuevoJugador);
                }
                juego.IniciarRonda();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService IniciarJuego", ex);
            }
        }

        public Jugador ObtenerJugadorActual()
        {
            try
            {
                return Juego.Jugadores.Count == 0 ? null : Juego.Jugadores[Juego.IndiceJugador];
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService ObtenerJugadorActual", ex);
            }
        }

        public (Carta carta, int puntos) CogerCarta()
        {
            try
            {
                var jugadorActual = ObtenerJugadorActual();
                var carta = juego.ObtenerCarta();

                int puntos = juego.AplicarEfectoCartas(carta);

                jugadorActual.Puntos += puntos;
                jugadorActual.L_cartas_jugador.Add(carta); // También le agregas la carta
                juego.ValidarYDispararEventos(null);

                return (carta, puntos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService CogerCarta", ex);
            }
        }

        public int AplicarCarta(Carta carta)
        {
            try
            {
                return juego.AplicarEfectoCartas(carta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService AplicarCarta", ex);
            }
        }

        public void PasarTurno()
        {
            try
            {
                if (juego == null) throw new InvalidOperationException("El juego no ha sido iniciado.");

                Jugador LiderInicial = juego.ObtenerLider();
                juego.ValidarYDispararEventos(LiderInicial);

                if (!JuegoTerminado() && Juego.Jugadores != null && Juego.Jugadores.Any())
                {
                    juego.PasarTurno();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService PasarTurno", ex);
            }
        }

        public Jugador FinalizarJuego()
        {
            try
            {
                ObtenerHistorial();
                return juego.FinalizarJuego();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService FinalizarJuego", ex);
            }
        }

        public List<Jugador> ObtenerJugadores()
        {
            try
            {
                return Juego.Jugadores;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService ObtenerJugadores", ex);
            }
        }

        public List<string> ObtenerHistorial()
        {
            try
            {
                return juego.Historial.ObtenerNotificaciones().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService ObtenerHistorial", ex);
            }
        }

        public bool JuegoTerminado()
        {
            try
            {
                if (juego.AgotadasResto && juego.AgotadasCastigo && juego.AgotadasPremio)
                {
                    return true;
                }

                if (Juego.Jugadores == null || Juego.Jugadores.Count < juego.JugadoresMin)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService JuegoTerminado", ex);
            }
        }

        public int TotalCartas()
        {
            try
            {
                return Juego.L_cartas_resto.Count + Juego.L_cartas_castigo.Count + Juego.L_cartas_premio.Count;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService TotalCartas", ex);
            }
        }

        public void ReiniciarJuego()
        {
            try
            {
                juego = null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService ReiniciarJuego", ex);
            }
        }

        public void ComenzarNuevaRondaConJugadoresActuales()
        {
            try
            {
                if (juego == null)
                {
                    throw new InvalidOperationException("El objeto de juego no existe para iniciar una nueva ronda.");
                }
                if (Juego.Jugadores == null || !Juego.Jugadores.Any())
                {
                    // Esto no debería pasar si se llega aquí desde una partida en curso.
                    // Podrías redirigir a IniciarJuego si es necesario, o lanzar error.
                    throw new InvalidOperationException("No hay jugadores actuales para comenzar una nueva ronda.");
                }

                juego.IniciarRonda();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en JuegoService ComenzarNuevaRondaConJugadoresActuales", ex);
            }
        }
    }
}
