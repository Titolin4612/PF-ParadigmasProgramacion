using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Jugador
    {
        // Atributos
        private string _nickname;
        private byte _puntos;
        private List<Carta> l_cartas_jugador;

        // Accesores
        public string Nickname
        {
            get => _nickname;
            set => _nickname = value = !(string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value)) && !(value.Length < 4) ? value
                : throw new Exception("Error, el nickname es invalido.");
        }
        public byte Puntos
        {
            get => _puntos;
            set => _puntos = value >= 50 && value <= 80 ? value
                : throw new Exception("Los puntos para iniciar no se encuentran dentro del rango esperado.");
        }
        internal List<Carta> L_cartas_jugador { get => l_cartas_jugador; set => l_cartas_jugador = value; }

        public Jugador(string nickname, byte puntos)
        {
            Nickname = nickname;
            L_cartas_jugador = new List<Carta>();
            Puntos = _puntos;
        }
    
    }
}
