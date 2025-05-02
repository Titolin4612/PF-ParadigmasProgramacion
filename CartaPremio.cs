using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO
{
    internal class CartaPremio : Carta
    {
        // Atributo
        private string _bendicion;

        // Accesor
        public string Bendicion { get => _bendicion; set => _bendicion = value; }

        // Constructor
        public CartaPremio(string nombre, string descripcion, string mitologia, string bendicion)
        {
            Bendicion = bendicion;
        }
    }
}
