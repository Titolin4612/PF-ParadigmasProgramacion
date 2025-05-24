using System.Collections.Generic;
using CL_ProyectoFinalPOO.Clases;

namespace CL_ProyectoFinalPOO.Interfaces
{
    public interface IReglasService
    {
        List<CartaJuego> ObtenerCartasJuego();
        List<CartaPremio> ObtenerCartasPremio();
        List<CartaCastigo> ObtenerCartasCastigo();
    }
}