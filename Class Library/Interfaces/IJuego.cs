using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Clases;

namespace CL_ProyectoFinalPOO.Interfaces
{
    internal interface IJuego
    {
        void Revolver<T>(List<T> lista);
        void BarajarCartas();
        Carta ObtenerCarta();
        void RepartirCartasIniciales(int numeroCartasPorJugador);
        int AplicarEfectoCartas(Carta carta);
        Jugador ObtenerLider();
    }
}
