using System;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;

namespace CL_ProyectoFinalPOO.Eventos
{
    public class Publisher_Eventos_Juego
    {
        public delegate void DelegadoEventoGeneral(); // Para eventos sin parámetros específicos
        public delegate void DelegadoEventoCambioLider(Jugador nuevoLider); // Para evento con el nuevo líder

        public event DelegadoEventoGeneral AgotadasPremio;
        public event DelegadoEventoGeneral AgotadasCastigo;
        public event DelegadoEventoGeneral AgotadasResto;
        public event DelegadoEventoCambioLider CambioLider;

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
    }
}