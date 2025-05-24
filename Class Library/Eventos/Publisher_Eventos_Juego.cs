using System;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;

namespace CL_ProyectoFinalPOO.Eventos
{
    public class Publisher_Eventos_Juego
    {
        public delegate void DelegadoEventoGeneral();
        public delegate void DelegadoEventoParametroJugador(Jugador jugador);

        public event DelegadoEventoGeneral InicioPartida;
        public event DelegadoEventoParametroJugador FinPartida;

        public void NotificarInicioPartida()
        {
            InicioPartida?.Invoke();
        }

        public void NotificarFinPartida(Jugador ganador)
        {
            FinPartida?.Invoke(ganador);
        }
    }
}