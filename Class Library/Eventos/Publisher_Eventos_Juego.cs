using System;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;

namespace CL_ProyectoFinalPOO.Eventos
{
    public class Publisher_Eventos_Juego
    {
        public delegate void DelegadoEventoGeneral(); 
        public delegate void DelegadoEventoParametroJugador(Jugador jugador); 
        public delegate void DelegadoEventoParametroJugador2(Jugador jugador, Carta carta); 
        public event DelegadoEventoGeneral AgotadasPremio;
        public event DelegadoEventoGeneral AgotadasCastigo;
        public event DelegadoEventoGeneral AgotadasResto;
        public event DelegadoEventoGeneral InicioPartida;
        public event DelegadoEventoParametroJugador2 CartasIniciales;
        public event DelegadoEventoParametroJugador SinPuntos;
        public event DelegadoEventoParametroJugador FinPartida;
        public event DelegadoEventoParametroJugador CambioLider;

        public void NotificarAgotadasPremio()
        {
            AgotadasPremio?.Invoke();
        }

        public void NotificarAgotadasCastigo()
        {
            AgotadasCastigo?.Invoke();
        }

        public void NotificarAgotadasResto()
        {
            AgotadasResto?.Invoke();
        }

        public void NotificarCambioLider(Jugador nuevoLider, Juego juego)
        {
            CambioLider?.Invoke(nuevoLider);
            juego.NicknameLiderAnterior = nuevoLider.Nickname;
        }

        public void NotificarInicioPartida()
        {
            InicioPartida?.Invoke();
        }
        public void NotificarCartasObtenidas(Jugador jugador, Carta carta)
        {
            CartasIniciales?.Invoke(jugador, carta);
        }

        public void NotificarJugadorSinPuntos(Jugador jugador)
        {
            SinPuntos.Invoke(jugador);
        }


        public void NotificarFinPartida(Jugador ganador)
        {
            FinPartida?.Invoke(ganador);
        }
    }
}