using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Clases;

namespace CL_ProyectoFinalPOO.Interfaces
{
    public interface IJuegoService
    {
        void IniciarJuego(List<Jugador> jugadoresConfigurados);
        bool EstaJuegoActivo();
        Jugador ObtenerJugadorActual();
        (Carta carta, int puntos) CogerCarta();
        void PasarTurno();
        Jugador FinalizarJuego();
        public int TotalCartasEnMazo();
        List<Jugador> ObtenerJugadores();
        List<string> ObtenerHistorial();
        bool JuegoTerminado();
        void ComenzarNuevaRondaConJugadoresActuales();
        void ReiniciarJuego();
    }
}
