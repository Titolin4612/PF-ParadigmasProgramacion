using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Clases;

namespace CL_ProyectoFinalPOO.Interfaces
{
    public interface IHomeService
    {
        void AgregarJugadorConfigurado(string nickname, int apuesta);
        void EliminarUltimoJugadorConfigurado();
        List<Jugador> ValidarConfiguracionJugadoresParaJuego();
        List<Jugador> ObtenerJugadoresConfigurados();
        void LimpiarConfiguracionJugadores();
        bool BuscarUsuario(string usuario, string contraseña = null);
        void RegistrarUsuario(string usuario, string contraseña);
    }
}
