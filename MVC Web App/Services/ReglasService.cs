using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MVC_ProyectoFinalPOO.Services
{
    public class ReglasService : IReglasService // Implement the interface
    {
        private readonly Baraja _baraja; // Dependency injection for Baraja

        public ReglasService(Baraja baraja)
        {
            _baraja = baraja;
        }

        public List<CartaJuego> ObtenerCartasJuego()
        {
            try
            {
                // Access CargarCartas via the instance and pass the static path
                _baraja.CargarCartas(Baraja._rutaArchivoCartas);
                // Access CartasJuego via the instance
                return _baraja.CartasJuego;
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
                _baraja.CargarCartas(Baraja._rutaArchivoCartas);
                // Access CartasPremio via the instance
                return _baraja.CartasPremio;
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
                _baraja.CargarCartas(Baraja._rutaArchivoCartas);
                // Access CartasCastigo via the instance
                return _baraja.CartasCastigo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReglasService.ObtenerCartasCastigo: Error - {ex.Message}");
                throw new Exception("Error en ReglasService ObtenerCartasCastigo", ex);
            }
        }
    }
}