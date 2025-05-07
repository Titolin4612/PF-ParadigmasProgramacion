using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO.Clases
{
    public class CartaJuego : Carta
    {

        public enum Rareza
        {
            Comun,
            Especial,
            Rara,
            Epica,
            Legendaria
        }


        // Atributos
        private Rareza rarezaCarta;

        public Rareza RarezaCarta { get => rarezaCarta; set => rarezaCarta = value; }

        // Constructor
        public CartaJuego(string nombre, string descripcion, string mitologia, Rareza rareza)
            : base(nombre, descripcion, mitologia)
        {
            RarezaCarta = rareza;
        }

        // Método para modificar puntos según rareza
        public override int ActualizarPuntos()
        {
            return (int)RarezaCarta;
        }
    }
}

