using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Interfaces;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Juego : IJuego
    {

        private List<Jugador> jugadores;
        private List<CartaPremio> l_cartas_premio;
        private List<CartaCastigo> l_cartas_castigo;
        private List<Carta> l_barajacompleta;

        private static Random rng = new Random();

        // Atributos de reglas de juego
        private byte cartasPorJugador = 3, jugadoresMax = 4;
      
        // Suponiendo que esta propiedad sea inicializada desde afuera o internamente
        public Resto Resto { get; set; }

        // Accesores
        public List<Jugador> Jugadores { get => jugadores; set => jugadores = value; }
        public List<CartaPremio> L_cartas_premio { get => l_cartas_premio; set => l_cartas_premio = value; }
        public List<CartaCastigo> L_cartas_castigo { get => l_cartas_castigo; set => l_cartas_castigo = value; }
        public List<Carta> L_barajacompleta { get => l_barajacompleta; set => l_barajacompleta = value; }
        public byte CartasPorJugador { get => cartasPorJugador; set => cartasPorJugador = value; }
        public byte JugadoresMax { get => jugadoresMax; set => jugadoresMax = value; }

        // Constructor
        public Juego()
        {
            Jugadores = new List<Jugador>();
            Resto = new Resto(); // Esto inicializa el l_cartas_resto del constructor de resto
            L_barajacompleta = Baraja.ObtenerBarajaCompleta();
            L_cartas_premio = Baraja.CrearCartasPremio();
            L_cartas_castigo = Baraja.CrearCartasCastigo();
            BarajarCartas();
        }

        // Método para revolver una lista (GENERICO)
        public void Revolver<T>(List<T> lista)
        {
            try
            {
                int n = lista.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    (lista[n], lista[k]) = (lista[k], lista[n]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error del metodo Revolver: " + ex);
            }

        }

        // Método para revolver las cartas de la baraja completa
        public void BarajarCartas()
        {
            try
            {
                if (Resto != null && Resto.L_cartas_resto != null && L_cartas_premio != null && L_cartas_castigo != null)
                {
                    Revolver(L_barajacompleta);
                }
                else Console.WriteLine("Error");
            }
            catch (Exception ex)
            {
                throw new Exception("Error del metodo BarajarCartas: " + ex);
            }


            
        }

        // Metodo para obtener una carta aleatoria de la baraja completa
        public Carta ObtenerCarta()
        {
            try
            {
                if (L_barajacompleta != null && L_barajacompleta.Count > 0)
                {
                    var carta = L_barajacompleta.First();
                    L_barajacompleta.RemoveAt(0);
                    return carta;
                }
                else
                {
                    throw new Exception("La baraja está vacía o no está inicializada.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error del metodo ObtenerCarta: " + ex);
            }


            
        }

        // Metodo para entregar las cartas iniciales a cada jugador
        public void RepartirCartasIniciales(int numeroCartasPorJugador)
        {
            try
            {

                if (Jugadores == null || Jugadores.Count == 0)
                {
                    throw new Exception("No hay jugadores en el juego.");
                }

                if (Resto == null || Resto.L_cartas_resto.Count < numeroCartasPorJugador * Jugadores.Count)
                {
                    throw new Exception("No hay suficientes cartas en el resto para repartir.");
                }

                Revolver(Resto.L_cartas_resto); // Barajar antes de repartir

                foreach (var jugador in Jugadores)
                {
                    Console.WriteLine($"\nCartas repartidas para {jugador.Nickname}");
                    for (int i = 0; i < numeroCartasPorJugador; i++)
                    {
                        var carta = Resto.L_cartas_resto.First();
                        Resto.L_cartas_resto.RemoveAt(0);
                        jugador.L_cartas_jugador.Add(carta);
                        jugador.Puntos += AplicarEfectoCartas(carta); // Sumar puntos

                    }
                    Console.WriteLine($"{jugador.Nickname} empieza el juego con {jugador.Puntos} puntos");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error del metodo RepartirCartasIniciales: " + ex);
            }


        }

        // Metodo para aplicar el efecto de las cartas
        public int AplicarEfectoCartas(Carta carta)
        {
            try
            {
                int puntosCarta = 0;
                switch (carta)
                {
                    case CartaJuego juego:
                        switch (juego.RarezaCarta)
                        {
                            case CartaJuego.Rareza.Comun:
                                puntosCarta = CartaJuego.RaComun;

                                break;
                            case CartaJuego.Rareza.Especial:
                                puntosCarta = CartaJuego.RaEspecial;

                                break;
                            case CartaJuego.Rareza.Rara:
                                puntosCarta = CartaJuego.RaRara;

                                break;
                            case CartaJuego.Rareza.Epica:
                                puntosCarta = CartaJuego.RaEpica;

                                break;
                            case CartaJuego.Rareza.Legendaria:
                                puntosCarta = CartaJuego.RaLegendaria;

                                break;
                        }
                        Console.WriteLine($"¡Carta de juego obtenida! {juego.Nombre} ({juego.RarezaCarta}) Valor de la carta: {puntosCarta}");
                        break;

                    case CartaPremio premio:
                        puntosCarta = CartaPremio.VPremio;
                        Console.WriteLine($"¡Carta de Premio obtenida! {premio.Nombre} Premio recibido: {premio.Bendicion}. ¡+5 puntos!");
                        break;

                    case CartaCastigo castigo:
                        puntosCarta = CartaCastigo.VCastigo;
                        Console.WriteLine($"¡Carta de Castigo obtenida! {castigo.Nombre} Castigo sufrido: {castigo.Maleficio}. ¡-5 puntos!");
                        break;

                    default:
                        Console.WriteLine("Carta sin efecto definido.");
                        break;
                }

                return puntosCarta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error del metodo AplicarEfectoCartas: " + ex);
            }

        }


        // Metodo para mostrar las cartas del jugador
        public void MostrarCartasJugador(Jugador jugador)
        {

            try
            {
                if (jugador.L_cartas_jugador.Count == 0)
                {
                    Console.WriteLine($"{jugador.Nickname} aún no tiene cartas en su poder.");
                    return;
                }

                Console.WriteLine($"Cartas de {jugador.Nickname}:");
                Console.WriteLine(new string('-', 60));

                int i = 1;

                // Imprimir las cartas del jugador
                foreach (var carta in jugador.L_cartas_jugador)
                {
                    Console.WriteLine($"Carta {i++}: {carta.Nombre} | Miología: {carta.Mitologia}");
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
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error del metodo MostrarCartasJugador: " + ex);
            }

            
        }

        public Jugador ObtenerLider() => Jugadores.OrderByDescending(j => j.Puntos).First();
    }
}