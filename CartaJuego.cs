using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO
{
    internal class CartaJuego : Carta
    {
        // Atributos
        private string _rareza;

        // Accesores
        public string Rareza { get => _rareza; set => _rareza = value; }

        // Constructor
        public CartaJuego(string nombre, string descripcion, string mitologia, string rareza)
        {
            Rareza = rareza;
        }
    }
}
