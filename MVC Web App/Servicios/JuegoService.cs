using CL_ProyectoFinalPOO.Interfaces;
using CL_ProyectoFinalPOO.Eventos;
using CL_ProyectoFinalPOO.Clases;

namespace MVC_ProyectoFinalPOO.Servicios
{
    public class JuegoService
    {
        private Juego _juego;

        public JuegoService()
        {
            _juego = new Juego();
        }

        public void IniciarJuego(List<Jugador> jugadores)
        {
            Juego.Jugadores = jugadores;
            _juego.IniciarRonda();
        }

        public void ReiniciarJuego()
        {
            _juego.ReiniciarJuego();
        }

        public Juego ObtenerJuego()
        {
            return _juego;
        }
    }
}