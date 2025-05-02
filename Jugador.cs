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
        private short _puntos;
        private List<Carta> l_cartas;

        // Accesores
        public string Nickname { get => _nickname; set => _nickname = value; }
        public short Puntos { get => _puntos; set => _puntos = value; }
        internal List<Carta> L_cartas { get => l_cartas; set => l_cartas = value; }
    }
}
