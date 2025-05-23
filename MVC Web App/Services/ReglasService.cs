// Services/ReglasService.cs
using CL_ProyectoFinalPOO.Clases;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace MVC_ProyectoFinalPOO.Services
{
    public class ReglasService
    {
        public List<CartaJuego> ObtenerCartasJuego()
        {
            try
            {
                Baraja.CargarCartas();
                return Baraja.CartasJuego;
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
                Baraja.CargarCartas();
                return Baraja.CartasPremio;
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
                Baraja.CargarCartas();
                return Baraja.CartasCastigo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ReglasService ObtenerCartasCastigo", ex);
            }
        }
    }
}