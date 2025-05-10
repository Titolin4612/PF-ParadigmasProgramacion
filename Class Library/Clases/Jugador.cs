using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Interfaces;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Jugador : IJugador
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

        //public int Puntos

        //{
        //    get => _puntos;
        //    set => _puntos = value >= 50 && value <= 80 ? value
        //        : throw new Exception("Los puntos para iniciar no se encuentran dentro del rango esperado.");
        //}

        public int Puntos { get => _puntos; set => _puntos = value; }

        public List<Carta> L_cartas_jugador { get => l_cartas_jugador; set => l_cartas_jugador = value; }

        public int ApuestaInicial
        {
            get => _apuestaInicial;
            set
            {
                if (value < 10 || value > 1000)
                    throw new Exception("La apuesta inicial debe estar entre 10 y 1000.");
                _apuestaInicial = value;
            }
        }


        // Constructor

        public Jugador(string nickname, int apuestaInicial, Juego juego)
        {
            Nickname = nickname;
            L_cartas_jugador = new List<Carta>();
            ApuestaInicial = apuestaInicial;
            Juego = juego;
            AsignarPuntosSegunApuesta();
        }

        // Metodos

        public void AsignarPuntosSegunApuesta()
        {
            try
            {
                if (_apuestaInicial < 100)
                    Puntos = 50;
                else if (_apuestaInicial <= 300)
                    Puntos = 60;
                else if (_apuestaInicial <= 600)
                    Puntos = 70;
                else
                    Puntos = 80;

            } catch (Exception ex)
            {
                throw new Exception("Error en el metodo AsignarPuntosSegunApuesta" +  ex);            
            }

            
        }

        public Carta CogerCarta()
        {
            try
            {
                if (Juego?.Resto != null)
                {
                    var carta = Juego.ObtenerCarta(); // Llama al método para obtener la primera carta
                    if (carta is CartaJuego)
                    {
                        L_cartas_jugador.Add(carta); // Agrega la carta al jugador
                    }
                    Puntos += Juego.AplicarEfectoCartas(carta);
                    return carta; // Devuelve la carta obtenida
                }
                else
                {
                    throw new Exception("No se puede obtener la carta, el Resto no está disponible.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el metodo CogerCarta" + ex);
            }
        }

        //public void MostrarCartasJugador()
        //{
        //    if (L_cartas_jugador.Count == 0)
        //    {
        //        Console.WriteLine($"{Nickname} aún no tiene cartas en su poder.");
        //        return;
        //    }

        //    Console.WriteLine($"Cartas de {Nickname}:");
        //    Console.WriteLine(new string('-', 60));

        //    int i = 1;

        //    // Imprimir las cartas del jugador
        //    foreach (var carta in L_cartas_jugador)
        //    {
        //        Console.WriteLine($"Carta {i++}: {carta.Nombre} | Miología: {carta.Mitologia}");
        //        Console.WriteLine($"Descripción: {carta.Descripcion}");

        //        switch (carta)
        //        {
        //            case CartaJuego juego:
        //                Console.WriteLine($"Tipo: Juego | Rareza: {juego.RarezaCarta}");
        //                break;
        //            case CartaCastigo castigo:
        //                Console.WriteLine($"Tipo: Castigo | Maleficio: {castigo.Maleficio}");
        //                break;
        //            case CartaPremio premio:
        //                Console.WriteLine($"Tipo: Premio | Bendición: {premio.Bendicion}");
        //                break;
        //            default:
        //                Console.WriteLine("Tipo desconocido.");
        //                break;
        //        }
        //        Console.WriteLine(new string('-', 60));
        //    }
        //}

        //public void AplicarEfectoCartas(Carta carta)
        //{
        //    var puntosCarta = 0;
        //    switch (carta)
        //    {
        //        case CartaJuego juego:
        //            switch (juego.RarezaCarta)
        //            {
        //                case CartaJuego.Rareza.Comun:
        //                    puntosCarta = -2;
        //                    Puntos += -2;
        //                    break;
        //                case CartaJuego.Rareza.Especial:
        //                    puntosCarta = -1;
        //                    Puntos += -1;
        //                    break;
        //                case CartaJuego.Rareza.Rara:
        //                    puntosCarta = 0;
        //                    Puntos += 0;
        //                    break;
        //                case CartaJuego.Rareza.Epica:
        //                    puntosCarta = +1;
        //                    Puntos += 1;
        //                    break;
        //                case CartaJuego.Rareza.Legendaria:
        //                    puntosCarta = +2;
        //                    Puntos += 2;
        //                    break;
        //            }
        //            Console.WriteLine($"¡Carta de juego obtenida! {juego.Nombre} ({juego.RarezaCarta}) Puntos obtenidos: {puntosCarta} Puntos en total: {Puntos}");
        //            break;

        //        case CartaPremio premio:
        //            Puntos += 5;
        //            Console.WriteLine($"🎁 Premio recibido: {premio.Bendicion}. ¡+5 puntos! Total: {Puntos}");
        //            break;

        //        case CartaCastigo castigo:
        //            Puntos -= 5;
        //            Console.WriteLine($"💀 Castigo sufrido: {castigo.Maleficio}. ¡-5 puntos! Total: {Puntos}");
        //            break;

        //        default:
        //            Console.WriteLine("⚠️ Carta sin efecto definido.");
        //            break;
        //    }
        //}


        // Método para mostrar los puntos del jugador
        public void MostrarPuntos()
        {
            try
            {
                Console.WriteLine($"{Nickname} tiene {Puntos} puntos.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el metodo MostrarPuntos" + ex);
            }

        }
    }
}
