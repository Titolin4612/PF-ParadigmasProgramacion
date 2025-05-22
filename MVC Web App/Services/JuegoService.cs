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

        public Jugador ObtenerJugadorActual()
        {
            return Juego.Jugadores.Count == 0 ? null : Juego.Jugadores[Juego.IndiceJugador];
        }

        public (Carta carta, int puntos) CogerCarta()
        {
            var jugadorActual = ObtenerJugadorActual();
            var carta = juego.ObtenerCarta();

            int puntos = juego.AplicarEfectoCartas(carta);

            jugadorActual.Puntos += puntos;
            jugadorActual.L_cartas_jugador.Add(carta); // También le agregas la carta

            return (carta, puntos);
        }

        public int AplicarCarta(Carta carta)
        {
            return juego.AplicarEfectoCartas(carta);
        }

        public void PasarTurno()
        {
            if (juego == null) throw new InvalidOperationException("El juego no ha sido iniciado.");

            Jugador LiderInicial = juego.ObtenerLider(); 
            juego.ValidarYDispararEventos(LiderInicial); 

            if (!JuegoTerminado() && Juego.Jugadores != null && Juego.Jugadores.Any())
            {
                juego.PasarTurno();
            }
        }

        public Jugador FinalizarJuego()
        {

            juego.FinalizarJuego();
            ObtenerHistorial();
            return juego.FinalizarJuego();
        }

        public List<Jugador> ObtenerJugadores()
        {
            return Juego.Jugadores;
        }

        public List<string> ObtenerHistorial()
        {
            return juego.Historial.ObtenerNotificaciones().ToList();
        }

        public bool JuegoTerminado()
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

        public int TotalCartas()
        {
            return Juego.L_cartas_resto.Count + Juego.L_cartas_castigo.Count + Juego.L_cartas_premio.Count;
        }

        public void ReiniciarJuego()
        {
            juego = null;

        }

        public void ComenzarNuevaRondaConJugadoresActuales()
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
    }
}
