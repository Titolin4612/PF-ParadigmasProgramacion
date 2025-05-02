using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO
{
    internal class Jugador
    {
        // Atributos
        private string _nickname;
        private int _puntos;
        private List<Carta> l_cartas;

        // Accesores
        public string Nickname 
        { 
            get => _nickname; 
            set => _nickname = value = !(string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value)) && !(value.Length < 4) ? value 
                : throw new Exception("Error, el nickname es invalido.");
        } 
        public int Puntos 
        { 
            get => _puntos; 
            set => _puntos = (value >= 50 && value <= 80) ? value 
                : throw new Exception("Los puntos para iniciar no se encuentran dentro del rango esperado."); 
        }
        internal List<Carta> L_cartas { get => l_cartas; set => l_cartas = value; }

        public Jugador(string nickname)
        {
            Nickname = nickname;
            L_cartas = new List<Carta>();
            Puntos = new Random().Next(50, 81);
        }
    }
}
