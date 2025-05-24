using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MVC_ProyectoFinalPOO.Services
{
    public class ReglasService : IReglasService 
    {
        private readonly Baraja _baraja; 

        public ReglasService(Baraja baraja)
        {
            _baraja = baraja;
        }

        public List<CartaJuego> ObtenerCartasJuego()
        {
            try
            {

                _baraja.CargarCartas(Baraja._rutaArchivoCartas);

                return _baraja.CartasJuego;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ReglasService ObtenerCartasJuego", ex);
            }
        }

        public List<CartaPremio> ObtenerCartasPremio()
        {
            try
            {
                _baraja.CargarCartas(Baraja._rutaArchivoCartas);

                return _baraja.CartasPremio;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ReglasService ObtenerCartasPremio", ex);
            }
        }

        public List<CartaCastigo> ObtenerCartasCastigo()
        {
            try
            {
                _baraja.CargarCartas(Baraja._rutaArchivoCartas);
                return _baraja.CartasCastigo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ReglasService ObtenerCartasCastigo", ex);
            }
        }
    }
}