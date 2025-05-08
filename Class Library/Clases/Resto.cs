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
        private byte cantidadCartas = 50;

        // Constructor
        public Resto()
        {
            CantidadResto = cantidadCartas;
            l_cartas_resto = Baraja.CrearCartasJuego();
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

        

    }
}