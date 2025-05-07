using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Resto
    {
        // Atributos
        private List<Carta> l_cartas_resto;
        private byte _cantidadResto;

        public Resto()
        {
            CantidadResto = 50;
            l_cartas_resto = Baraja.CrearCartas();
        }

        // Accesores
        public List<Carta> L_cartas_resto { get => l_cartas_resto; set => l_cartas_resto = value; }
        public byte CantidadResto { get => _cantidadResto; set => _cantidadResto = value; }

        // Imprimir las cartas en la baraja

        int consecutivo;
        public void imprimirLista()
        {
            consecutivo = 1;
            Console.WriteLine("Baraja de cartas:");
            Console.WriteLine(new string('-', 60));

            foreach (var carta in l_cartas_resto)
            {
                Console.WriteLine($"Nombre: {carta.Nombre} | Mitología: {carta.Mitologia} | Numero: {consecutivo}");
                Console.WriteLine($"Descripción: {carta.Descripcion}");

                switch (carta)
                {
                    case CartaJuego juego:
                        Console.WriteLine($"Tipo: Juego | Rareza: {juego.RarezaCarta}");
                        break;
                    case CartaCastigo castigo:
                        Console.WriteLine($"Tipo: Castigo | Maleficio: {castigo.Maleficio}");
                        break;
                    case CartaPremio premio:
                        Console.WriteLine($"Tipo: Premio | Bendición: {premio.Bendicion}");
                        break;
                    default:
                        Console.WriteLine("Tipo desconocido.");
                        break;
                }

                Console.WriteLine(new string('-', 60));
                consecutivo++;
            }
        }

        public Carta ObtenerCarta() // primera carta de la lista
        {
            if (l_cartas_resto != null && l_cartas_resto.Count > 0)
            {
                return l_cartas_resto.First();
            }
            else
            {
                throw new InvalidOperationException("La baraja está vacía o no está inicializada.");
            }
        }

    }
}