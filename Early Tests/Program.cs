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


            //try
            //{
            //    // Crear instancia de Juego
            //    Juego juego = new Juego();

            //    // Crear y agregar jugadores (los puntos se asignan en el constructor de Jugador)
            //    Jugador jugador1 = new Jugador("Alice", 100, juego);
            //    Jugador jugador2 = new Jugador("Bob", 500, juego);
            //    Jugador jugador3 = new Jugador("Tito", 800, juego);
            //    juego.Jugadores.Add(jugador1);
            //    juego.Jugadores.Add(jugador2);
            //    juego.Jugadores.Add(jugador3);

            //    // Mostrar puntos iniciales
            //    Console.WriteLine("Puntos iniciales:");
            //    jugador1.MostrarPuntos();
            //    jugador2.MostrarPuntos();
            //    jugador3.MostrarPuntos();

            //    // Repartir cartas iniciales
            //    juego.RepartirCartasIniciales(juego.CartasPorJugador);

            //    // Mostrar cartas y puntos tras reparto
            //    Console.WriteLine("\nTras repartir cartas iniciales:");
            //    foreach (var jugador in juego.Jugadores)
            //    {
            //        juego.MostrarCartasJugador(jugador);
            //        jugador.MostrarPuntos();
            //        Console.WriteLine("\n");
            //    }

            //    // Probar CogerCarta
            //    Console.WriteLine($"{jugador1.Nickname} probara a coger 3 cartas:");
            //    jugador1.MostrarPuntos();
            //    jugador1.CogerCarta();
            //    jugador1.CogerCarta();
            //    jugador1.CogerCarta();
            //    jugador1.MostrarPuntos();

            //    // Probar ObtenerLider
            //    Jugador lider = juego.ObtenerLider();
            //    Console.WriteLine($"\nEl líder actual es: {lider.Nickname} con {lider.Puntos} puntos.");

            //    // Probar barajar y mostrar resto (primeras 5 cartas)
            //    //Console.WriteLine("\nPrimeras 5 cartas del resto tras barajar:");
            //    //juego.Revolver(juego.L_barajacompleta);
            //    //juego.Resto.imprimirLista(); 
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}


            Console.WriteLine("Iniciando prueba de carga de cartas...");

            Juego juego = new Juego();
            Baraja baraja = new Baraja();
            Baraja.Ruta();
            try
            {
                // Simplemente llamar al método estático.
                // El constructor estático de Baraja se ejecutará automáticamente
                // la primera vez que se acceda a la clase Baraja aquí.
                Baraja.CargarCartas();
                Baraja.ImprimirCartasCargadas();

                Console.WriteLine("\nPrueba completada exitosamente.");
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción que pueda ocurrir durante la carga en el constructor estático
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine("ERROR FATAL DURANTE LA CARGA DE CARTAS:");
                Console.WriteLine(ex.ToString()); // Imprime toda la información de la excepción
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.ResetColor();
                Console.WriteLine("\nLa carga falló. Revisa el archivo cartas.json y los mensajes de error.");
            }
            Console.WriteLine("juego.Imprimir");
            juego.imprimir();

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();

            //Console.OutputEncoding = System.Text.Encoding.UTF8;
            //Console.WriteLine("🎮 BIENVENIDO A BLESSINGS AND CURSES\n");

            //// 1. Crear instancia del juego
            //Juego juego = new Juego();

            //// 2. Crear jugadores
            //Juego.Jugadores.Add(new Jugador("Santi", 200, juego));
            //Juego.Jugadores.Add(new Jugador("Laura", 500, juego));
            //Juego.Jugadores.Add(new Jugador("Nico", 100, juego));

            //Console.WriteLine("✅ Jugadores registrados:");
            //foreach (var j in Juego.Jugadores)
            //{
            //    Console.WriteLine($"   - {j.Nickname} (Apuesta: {j.ApuestaInicial} | Puntos iniciales: {j.Puntos})");
            //}

            //// 3. Iniciar el juego
            //juego.IniciarRonda();

            //// 4. Ciclo principal del juego
            //while (juego.L_cartas_resto.Count > 0 || Juego.L_cartas_castigo.Count > 0 || Juego.L_cartas_premio.Count > 0)
            //{
            //    var jugadorActual = Juego.Jugadores[Juego.IndiceJugador];

            //    Console.WriteLine($"\n🕹️ Turno de {jugadorActual.Nickname}:");
            //    var carta = jugadorActual.CogerCarta();

            //    Console.WriteLine($"   🃏 Carta: {carta.Nombre} | Puntos obtenidos: {juego.AplicarEfectoCartas(carta)}");
            //    Console.WriteLine($"   🔢 Puntos totales de {jugadorActual.Nickname}: {jugadorActual.Puntos}");

            //    juego.PasarTurno();

            //    Console.WriteLine("Presiona ENTER para continuar...");
            //    Console.ReadLine();
            //}

            //// 5. Finalizar juego
            //var ganador = juego.FinalizarJuego();

            //Console.WriteLine($"\n🏁 FIN DEL JUEGO");
            //Console.WriteLine($"🎉 GANADOR: {ganador.Nickname} con {ganador.Puntos} puntos");
            //Console.WriteLine("\n📜 Historial de eventos:");
            //foreach (var evento in juego.Historial.ObtenerNotificaciones())
            //{
            //    Console.WriteLine("   - " + evento);
            //}

            //Console.WriteLine("\nGracias por jugar 🃏");
            //Console.ReadLine();
            //Console.ReadLine();
            //Console.ReadLine();
            //Console.ReadLine();
            //Console.ReadLine();
            //Console.ReadLine();
            //Console.ReadLine();

        }


    }
}