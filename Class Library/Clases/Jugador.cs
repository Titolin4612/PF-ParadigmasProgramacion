using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Interfaces;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Jugador : IJugador
    {
        // Atributos
        private string _nickname;
        private int _puntos;
        private int _apuestaInicial;
        private List<Carta> l_cartas_jugador;
        private bool perdio = false;
        public Juego Juego { get; set; }

        // Accesores
        public string Nickname
        {
            get => _nickname;
            set => _nickname = value = !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value)) || !(value.Length < 4) ? value
                : throw new Exception("Error, el nickname es invalido.");
        }

        public int Puntos
        {
            get => _puntos;
            set => _puntos = value;
        }

        public List<Carta> L_cartas_jugador { get => l_cartas_jugador; set => l_cartas_jugador = value; }

        public int ApuestaInicial
        {
            get => _apuestaInicial;
            set
            {
                if (value < 10 || value > 1000)
                    throw new Exception("La apuesta inicial debe estar entre 10 y 1000.");
                _apuestaInicial = value;
            }
        }

        public bool Perdio { get => perdio; set => perdio = value; }

        // Constructor
        public Jugador(string nickname, int apuestaInicial, Juego juego)
        {
            Nickname = nickname;
            ApuestaInicial = apuestaInicial;
            L_cartas_jugador = new List<Carta>();
            Juego = juego;
            Juego.AsignarPuntosSegunApuesta(this);
        }

        public Carta CogerCarta()
        {
            try
            { 
                    var carta = Juego.ObtenerCarta(); 
                    L_cartas_jugador.Add(carta); 
                    Puntos += Juego.AplicarEfectoCartas(carta);
                    return carta; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el metodo CogerCarta" + ex);
            }
        }

    }
}
