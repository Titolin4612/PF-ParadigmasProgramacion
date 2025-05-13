using CL_ProyectoFinalPOO.Interfaces;
using CL_ProyectoFinalPOO.Eventos;
using CL_ProyectoFinalPOO.Clases;

namespace MVC_ProyectoFinalPOO.Servicios
{
    public class JuegoService : IJuegoService
    {
        private readonly Baraja _baraja;

        public JuegoService(Baraja baraja)
        {
            _baraja = baraja;
        }

        public Baraja CargarBaraja()
        {
            _baraja.CargarCartas();
            return _baraja;
        }
    }
}
