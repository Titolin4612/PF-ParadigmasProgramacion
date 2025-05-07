using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Juego
    {
        private static Random rng = new Random();

        // Suponiendo que esta propiedad sea inicializada desde afuera o internamente
        public Resto Resto { get; set; }

        public Juego()
        {
            Resto = new Resto();      // Creamos la baraja
            BarajarCartas();          // Ahora sí, barajamos las cartas
        }

        // Método para revolver una lista (NO es de extensión)
        public void Revolver<T>(List<T> lista)
        {
            int n = lista.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (lista[n], lista[k]) = (lista[k], lista[n]); // swap moderno
            }
        }

        // Método para revolver las cartas del Resto
        public void BarajarCartas()
        {
            if (Resto != null && Resto.L_cartas_resto != null)
            {
                Revolver(Resto.L_cartas_resto);
            }
        }
    }
}