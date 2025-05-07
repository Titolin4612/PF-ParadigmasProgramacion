using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Jugador
    {
        // Atributos
        private string _nickname;
        private int _puntos;
        private int _apuestaInicial;
        private List<Carta> l_cartas_jugador;
        public Juego Juego { get; set; }

        // Accesores

        public string Nickname
        {
            get => _nickname;
            set => _nickname = value = !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value)) || !(value.Length < 4) ? value
                : throw new Exception("Error, el nickname es invalido.");
        }

        public int Puntos

        {
            get => _puntos;
            set => _puntos = value >= 50 && value <= 80 ? value
                : throw new Exception("Los puntos para iniciar no se encuentran dentro del rango esperado.");
        }

        public List<Carta> L_cartas_jugador { get => l_cartas_jugador; set => l_cartas_jugador = value; }

        public int ApuestaInicial
        {
            get => _apuestaInicial;
            set
            {
                if (value < 10 || value > 1000)
                    throw new Exception("La apuesta inicial debe estar entre 10 y 1000.");
                _apuestaInicial = value;
                AsignarPuntosSegunApuesta(); // Cuando se asigna la apuesta, se asignan los puntos
            }
        }

        // Constructor

        public Jugador(string nickname, int apuestaInicial, Juego juego)
        {
            Nickname = nickname;
            L_cartas_jugador = new List<Carta>();
            ApuestaInicial = apuestaInicial;
            Juego = juego;
        }

        // Metodos

        private void AsignarPuntosSegunApuesta()
        {
            if (_apuestaInicial < 100)
                Puntos = 50;
            else if (_apuestaInicial <= 300)
                Puntos = 60;
            else if (_apuestaInicial <= 600)
                Puntos = 70;
            else
                Puntos = 80;
        }

        public Carta CogerCarta()
        {
            if (Juego?.Resto != null)
            {
                var carta = Juego.Resto.ObtenerCarta(); // Llama al método de Resto para obtener la primera carta
                Juego.Resto.L_cartas_resto.Remove(carta); // Elimina la carta de la baraja
                L_cartas_jugador.Add(carta); // Agrega la carta al jugador
                AplicarEfectoCartas(carta);
                return carta; // Devuelve la carta obtenida
            }
            else
            {
                throw new InvalidOperationException("No se puede obtener la carta, el Resto no está disponible.");
            }
        }

        public void MostrarCartasJugador()
        {
            if (L_cartas_jugador.Count == 0)
            {
                Console.WriteLine($"{Nickname} aún no tiene cartas en su poder.");
                return;
            }

            Console.WriteLine($"Cartas de {Nickname}:");
            Console.WriteLine(new string('-', 60));

            int i = 1;

            // Imprimir las cartas del jugador
            foreach (var carta in L_cartas_jugador)
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

        public void AplicarEfectoCartas(Carta carta)
        {
            switch (carta)
            {
                case CartaJuego juego:
                    switch (juego.RarezaCarta)
                    {
                        case CartaJuego.Rareza.Comun:
                            Puntos += -2;
                            break;
                        case CartaJuego.Rareza.Especial:
                            Puntos += -1;
                            break;
                        case CartaJuego.Rareza.Rara:
                            Puntos += 0;
                            break;
                        case CartaJuego.Rareza.Epica:
                            Puntos += 1;
                            break;
                        case CartaJuego.Rareza.Legendaria:
                            Puntos += 2;
                            break;
                    }
                    Console.WriteLine($"¡Carta de juego obtenida! {juego.Nombre} ({juego.RarezaCarta}) → {Puntos:+#;-#;0} puntos. Total: {Puntos}");
                    break;

                case CartaPremio premio:
                    Puntos += 5; 
                    Console.WriteLine($"🎁 Premio recibido: {premio.Bendicion}. ¡+5 puntos! Total: {Puntos}");
                    break;

                case CartaCastigo castigo:
                    Puntos -= 5;
                    Console.WriteLine($"💀 Castigo sufrido: {castigo.Maleficio}. ¡-5 puntos! Total: {Puntos}");
                    break;

                default:
                    Console.WriteLine("⚠️ Carta sin efecto definido.");
                    break;
            }
        }


        // Método para mostrar los puntos del jugador
        public void MostrarPuntos()
        {
            Console.WriteLine($"{Nickname} tiene {Puntos} puntos.");
        }
    }
}
