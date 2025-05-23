// MVC_ProyectoFinalPOO/Services/ReglasService.cs
using CL_ProyectoFinalPOO.Clases;
// Se eliminan using no necesarios como Microsoft.AspNetCore.Hosting, CultureInfo, etc.
// a menos que sean realmente utilizados por otros métodos no mostrados.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


namespace MVC_ProyectoFinalPOO.Services
{
    public class ReglasService
    {
        // Constructor público (ya lo tiene, implícito si no se define otro)
        // public ReglasService() {}

        public List<CartaJuego> ObtenerCartasJuego()
        {
            try
            {
                Baraja.CargarCartas(); // Carga/recarga desde el JSON. Baraja usa miembros estáticos.
                return Baraja.CartasJuego;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReglasService.ObtenerCartasJuego: Error - {ex.Message}");
                throw new Exception("Error en ReglasService ObtenerCartasJuego", ex);
            }
        }

        public List<CartaPremio> ObtenerCartasPremio()
        {
            try
            {
                Baraja.CargarCartas();
                return Baraja.CartasPremio;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReglasService.ObtenerCartasPremio: Error - {ex.Message}");
                throw new Exception("Error en ReglasService ObtenerCartasPremio", ex);
            }
        }

        public List<CartaCastigo> ObtenerCartasCastigo()
        {
            try
            {
                Baraja.CargarCartas();
                return Baraja.CartasCastigo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReglasService.ObtenerCartasCastigo: Error - {ex.Message}");
                throw new Exception("Error en ReglasService ObtenerCartasCastigo", ex);
            }
        }
    }
}