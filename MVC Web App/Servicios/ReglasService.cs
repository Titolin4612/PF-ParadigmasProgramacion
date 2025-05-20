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
            Baraja.CargarCartas();
            return Baraja.CartasJuego;
        }

        public List<CartaPremio> ObtenerCartasPremio()
        {
            Baraja.CargarCartas();
            return Baraja.CartasPremio;
        }

        public List<CartaCastigo> ObtenerCartasCastigo()
        {
            Baraja.CargarCartas();
            return Baraja.CartasCastigo;
        }
    }
}