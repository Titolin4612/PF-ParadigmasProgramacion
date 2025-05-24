using System;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;

namespace CL_ProyectoFinalPOO.Eventos
{
    public class Publisher_Eventos_Jugador
    {
        public delegate void DelegadoEventoParametroJugador(Jugador jugador);

        public event DelegadoEventoParametroJugador SinPuntos;
        public event DelegadoEventoParametroJugador CambioLider;

        public void NotificarJugadorSinPuntos(Jugador jugador)
        {
            SinPuntos?.Invoke(jugador);
        }

        public void NotificarCambioLider(Jugador nuevoLider, Juego juego)
        {
            CambioLider?.Invoke(nuevoLider);
            juego.NicknameLiderAnterior = nuevoLider.Nickname;
        }
    }
}