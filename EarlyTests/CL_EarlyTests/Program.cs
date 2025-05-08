using System;
using CL_ProyectoFinalPOO.Clases;

namespace CL_EarlyTests
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //    try
            //    {
            //        Console.OutputEncoding = System.Text.Encoding.UTF8;

            //        // Crear el juego
            //        Juego juego = new Juego();

            //        // Imprimir baraja original (opcional)
            //        Console.WriteLine("\n🔥 Baraja antes de repartir:");
            //        juego.Resto.imprimirLista();

            //        // Crear jugador
            //        Console.WriteLine("\nCreando jugador...");
            //        Jugador jugador = new Jugador("ApoloKid", 250, juego);

            //        // Coger cartas del resto
            //        Console.WriteLine($"\n👤 {jugador.Nickname} va a coger 3 cartas...\n");
            //        for (int i = 0; i < 3; i++)
            //        {
            //            Carta carta = jugador.CogerCarta();
            //        }

            //        // Mostrar cartas del jugador
            //        Console.WriteLine("\n📜 Mostrando cartas del jugador:");
            //        jugador.MostrarCartasJugador();

            //        // Mostrar la baraja restante
            //        Console.WriteLine("\n📉 Cartas restantes en el Resto:");
            //        Console.WriteLine($"Cantidad: {juego.Resto.L_cartas_resto.Count}");

            //        jugador.MostrarPuntos();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.ForegroundColor = ConsoleColor.Red;
            //        Console.WriteLine($"💥 Error: {ex.Message}");
            //        Console.ResetColor();
            //    }

            //    Console.WriteLine("\nPresiona una tecla para salir...");
            //    Console.ReadKey();


            try
            {
                // Crear instancia de Juego
                Juego juego = new Juego();

                // Crear y agregar jugadores (los puntos se asignan en el constructor de Jugador)
                Jugador jugador1 = new Jugador("Alice", 100, juego);
                Jugador jugador2 = new Jugador("Bob", 500, juego);
                Jugador jugador3 = new Jugador("Tito", 800, juego);
                juego.Jugadores.Add(jugador1);
                juego.Jugadores.Add(jugador2);
                juego.Jugadores.Add(jugador3);

                // Mostrar puntos iniciales
                Console.WriteLine("Puntos iniciales:");
                jugador1.MostrarPuntos();
                jugador2.MostrarPuntos();
                jugador3.MostrarPuntos();

                // Repartir cartas iniciales
                juego.RepartirCartasIniciales(juego.CartasPorJugador);

                // Mostrar cartas y puntos tras reparto
                Console.WriteLine("\nTras repartir cartas iniciales:");
                foreach (var jugador in juego.Jugadores)
                {
                    juego.MostrarCartasJugador(jugador);
                    jugador.MostrarPuntos();
                    Console.WriteLine("\n");
                }

                // Probar CogerCarta
                Console.WriteLine($"{jugador1.Nickname} probara a coger 3 cartas:");
                jugador1.MostrarPuntos();
                jugador1.CogerCarta();
                jugador1.CogerCarta();
                jugador1.CogerCarta();
                jugador1.MostrarPuntos();

                // Probar ObtenerLider
                Jugador lider = juego.ObtenerLider();
                Console.WriteLine($"\nEl líder actual es: {lider.Nickname} con {lider.Puntos} puntos.");

                // Probar barajar y mostrar resto (primeras 5 cartas)
                //Console.WriteLine("\nPrimeras 5 cartas del resto tras barajar:");
                //juego.Revolver(juego.L_barajacompleta);
                //juego.Resto.imprimirLista(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
