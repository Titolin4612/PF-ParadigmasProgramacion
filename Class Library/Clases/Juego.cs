using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Eventos;
using CL_ProyectoFinalPOO.Interfaces;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Juego : IJuego
    {
        // Listas
        private List<Jugador> jugadores;
        private List<CartaPremio> l_cartas_premio;
        private List<CartaCastigo> l_cartas_castigo;
        private List<CartaJuego> l_cartas_resto;

        // Atributos
        

        // Ramdom 
        private static Random rng = new Random();



        // Instancia de clase publicadora
        private Publisher_Eventos_Juego publicadorJuego;
        // Metodo para manejar los eventos
        internal void EventHandler() { }

        // Probabilidades para la selección de cartas
        private const double ProbJuego = 0.66; // 66%
        private const double ProbCastigo = 0.2424; // 24.24%
        private const double ProbPremio = 0.1616; // 16.16%


        // Atributos de reglas de juego
        private static byte cartasPorJugador = 3,jugadoresMin = 2, jugadoresMax = 4;



        // Accesores
        public List<Jugador> Jugadores { get => jugadores; set => jugadores = value; }
        public List<CartaPremio> L_cartas_premio { get => l_cartas_premio; set => l_cartas_premio = value; }
        public List<CartaCastigo> L_cartas_castigo { get => l_cartas_castigo; set => l_cartas_castigo = value; }
        public byte CartasPorJugador { get => cartasPorJugador; set => cartasPorJugador = value; }
        public byte JugadoresMin { get => jugadoresMin; set => jugadoresMin = value; }
        public byte JugadoresMax { get => jugadoresMax; set => jugadoresMax = value; }
        internal Publisher_Eventos_Juego PublicadorJuego { get => publicadorJuego; }
        public List<CartaJuego> L_cartas_resto { get => l_cartas_resto; set => l_cartas_resto = value; }
        public static double ProbJuego1 => ProbJuego;
        public static double ProbCastigo1 => ProbCastigo;
        public static double ProbPremio1 => ProbPremio;

        // Constructor
        public Juego()
        {
            Jugadores = new List<Jugador>();
            L_cartas_resto = Baraja.CrearCartasJuego();
            L_cartas_premio = Baraja.CrearCartasPremio();
            L_cartas_castigo = Baraja.CrearCartasCastigo();
            publicadorJuego = new Publisher_Eventos_Juego();
            BarajarCartas();
        }

        // Metodo para obtener nuevo lider
        public Jugador ObtenerLider()
        {
            try
            {
                publicadorJuego.CambioLider += EventHandler;
                Jugador lider = Jugadores.OrderByDescending(j => j.Puntos).FirstOrDefault();
                return lider;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ObtenerLider: " + ex.Message, ex);
            }
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
                if (L_cartas_premio != null && L_cartas_castigo != null && L_cartas_resto != null)
                {
                    Revolver(L_cartas_resto);
                    Revolver(L_cartas_premio);
                    Revolver(L_cartas_castigo);
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
            int totalElementos = l_cartas_resto.Count + l_cartas_castigo.Count + l_cartas_premio.Count;
            var liderActual = ObtenerLider();

            double rand = rng.NextDouble();
            Carta carta;

            if (rand < ProbJuego && l_cartas_resto.Count > 0)
            {
                carta = l_cartas_resto[0];
                l_cartas_resto.RemoveAt(0);
                if (l_cartas_resto.Count == 0)
                {
                    publicadorJuego.AgotadasResto += EventHandler;
                    publicadorJuego.NotificarAgotadasResto();
                }
                    
            }
            else if (rand < ProbJuego + ProbCastigo && l_cartas_castigo.Count > 0)
            {
                carta = l_cartas_castigo[0];
                l_cartas_castigo.RemoveAt(0);
                if (l_cartas_castigo.Count == 0)
                {
                    publicadorJuego.AgotadasCastigo += EventHandler;
                    publicadorJuego.NotificarAgotadasCastigo();
                }
                    
            }
            else if (l_cartas_premio.Count > 0)
            {
                carta = l_cartas_premio[0];
                l_cartas_premio.RemoveAt(0);
                if (l_cartas_premio.Count == 0)
                {
                    publicadorJuego.AgotadasPremio += EventHandler;
                    publicadorJuego.NotificarAgotadasPremio();
                }
                    
            }
            else
            {
                return null;
            }
            var nuevoLider = ObtenerLider();
            if (liderActual != nuevoLider)
            {
                publicadorJuego.CambioLider += EventHandler;
                publicadorJuego.NotificarCambioLider(nuevoLider);
            }
            return carta;
        }

        // Método para asignar puntos según la apuesta inicial del jugador
        public void AsignarPuntosSegunApuesta(Jugador jugador)
        {
            try
            {
                int apuesta = jugador.ApuestaInicial;
                if (apuesta < 100)
                    jugador.Puntos = 50;
                else if (apuesta <= 300)
                    jugador.Puntos = 60;
                else if (apuesta <= 600)
                    jugador.Puntos = 70;
                else
                    jugador.Puntos = 80;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el método AsignarPuntosSegunApuesta: " + ex.Message);
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

                if (L_cartas_resto.Count < numeroCartasPorJugador * Jugadores.Count)
                {
                    throw new Exception("No hay suficientes cartas en el resto para repartir.");
                }

                foreach (var jugador in Jugadores)
                {
                    Console.WriteLine($"\nCartas repartidas para {jugador.Nickname}");
                    for (int i = 0; i < numeroCartasPorJugador; i++)
                    {
                        var carta = L_cartas_resto.First();
                        L_cartas_resto.RemoveAt(0);
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

        // Aplicar efectos de las cartas y devolver puntos
        public int AplicarEfectoCartas2(Carta carta)
        {
            if (carta == null)
                throw new ArgumentNullException(nameof(carta));

            int puntos = carta.ObtenerPuntos();
            // Nota: Las salidas en consola deben moverse a la capa de presentación en un contexto MVC
            switch (carta)
            {
                case CartaJuego juego:
                    // Registrar: $"Carta de juego obtenida: {juego.Nombre} ({juego.RarezaCarta}), Puntos: {puntos}";
                    break;
                case CartaPremio premio:
                    // Registrar: $"Carta de Premio obtenida: {premio.Nombre}, Bendición: {premio.Bendicion}, Puntos: {puntos}";
                    break;
                case CartaCastigo castigo:
                    // Registrar: $"Carta de Castigo obtenida: {castigo.Nombre}, Maleficio: {castigo.Maleficio}, Puntos: {puntos}";
                    break;
            }

            return puntos;
        }

        
    }
}