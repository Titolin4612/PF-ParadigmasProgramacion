using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Eventos;
using CL_ProyectoFinalPOO.Interfaces;
using Microsoft.VisualBasic;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Juego : IJuego
    {
        // Listas
        private static List<Jugador> jugadores;
        private static List<CartaPremio> l_cartas_premio;
        private static List<CartaCastigo> l_cartas_castigo;
        private static List<CartaJuego> l_cartas_resto;

        // Atributos
        private static uint indiceJugador;

        // Ramdom 
        private static Random rng = new Random();

        // Instancia de clase publicadora
        private Publisher_Eventos_Juego publicadorJuego;
        private Historial historial;

        // Metodo para manejar los eventos 
        // TODO: Implementar mejor el EventHandler para que tenga estructura y maneje bien los eventos
        internal void EventHandler() { }

        // Atributos de reglas de negocio
        private static uint cartasPorJugador = 3;
        private static uint jugadoresMin = 2;
        private static uint jugadoresMax = 4;

        // TODO: Implementa 4 eventos, que notifican al usuario hechos importantes | •	No impresión desde los métodos |

        // Cargue de información desde archivos planos. El aspecto revisa que los archivos planos
        // Validación de información: acá se debe simular que se va a guardar en una base de datos, el aspecto debe simplemente validar la información
        // y si es correcta presentar un mensaje correspondiente, de la misma forma si no es correcta
        // Autenticación a la aplicación



        // Accesores
        public List<Jugador> Jugadores { get => jugadores; set => jugadores = value; }
        public List<CartaPremio> L_cartas_premio { get => l_cartas_premio; set => l_cartas_premio = value; }
        public List<CartaCastigo> L_cartas_castigo { get => l_cartas_castigo; set => l_cartas_castigo = value; }
        public uint CartasPorJugador { get => cartasPorJugador; }
        public uint JugadoresMin { get => jugadoresMin; }
        public uint JugadoresMax { get => jugadoresMax; }
        internal Publisher_Eventos_Juego PublicadorJuego { get => publicadorJuego; }
        public List<CartaJuego> L_cartas_resto { get => l_cartas_resto; set => l_cartas_resto = value; }
        public static uint IndiceJugador { get => indiceJugador; set => indiceJugador = value; }
        public Historial Historial { get => historial; set => historial = value; }

        // Constructor
        public Juego()
        {
            Jugadores = new List<Jugador>();
            L_cartas_resto = new List<CartaJuego>(Baraja.CartasJuego);
            L_cartas_premio = new List<CartaPremio>(Baraja.CartasPremio);
            L_cartas_castigo = new List<CartaCastigo>(Baraja.CartasCastigo);
            publicadorJuego = new Publisher_Eventos_Juego();
            Historial = new Historial(publicadorJuego);

        }

        // Metodo para obtener nuevo lider
        public Jugador ObtenerLider()
        {
            try
            {
                Jugador lider = Jugadores.OrderByDescending(j => j.Puntos).FirstOrDefault();
                return lider;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ObtenerLider: ", ex);
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
                throw new Exception("Error del metodo Revolver: ", ex);
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
                throw new Exception("Error del metodo BarajarCartas: ", ex);
            }

        }

        public Carta ObtenerCarta()
        {
            try
            {
                // Calcular numero actual de cartas totales
                int totalCartas = L_cartas_resto.Count + L_cartas_premio.Count + L_cartas_castigo.Count;
                // Ver el lider actual
                Jugador liderInicial = ObtenerLider();

                // Calcular probabilidades
                double probCartaJuego = (double)L_cartas_resto.Count / totalCartas;
                double probCartaCastigo = (double)L_cartas_castigo.Count / totalCartas;

                // Seleccionar carta al azar
                Carta carta = null;
                double random = rng.NextDouble();

                if (random < probCartaJuego && L_cartas_resto.Count > 0)
                {
                    int index = rng.Next(L_cartas_resto.Count);
                    carta = L_cartas_resto[index];
                    L_cartas_resto.RemoveAt(index);
                }
                else if (random < probCartaJuego + probCartaCastigo && L_cartas_castigo.Count > 0)
                {
                    int index = rng.Next(L_cartas_castigo.Count);
                    carta = L_cartas_castigo[index];
                    L_cartas_castigo.RemoveAt(index);
                }
                else if (L_cartas_premio.Count > 0)
                {
                    int index = rng.Next(L_cartas_premio.Count);
                    carta = L_cartas_premio[index];
                    L_cartas_premio.RemoveAt(index);
                }
                else
                {
                    if (L_cartas_resto.Count > 0)
                    {
                        int index = rng.Next(L_cartas_resto.Count);
                        carta = L_cartas_resto[index];
                        L_cartas_resto.RemoveAt(index);
                    }
                    else if (L_cartas_castigo.Count > 0)
                    {
                        int index = rng.Next(L_cartas_castigo.Count);
                        carta = L_cartas_castigo[index];
                        L_cartas_castigo.RemoveAt(index);
                    }
                    else if (L_cartas_premio.Count > 0)
                    {
                        int index = rng.Next(L_cartas_premio.Count);
                        carta = L_cartas_premio[index];
                        L_cartas_premio.RemoveAt(index);
                    }
                }

                AplicarEfectoCartas(carta);

                ValidarYDispararEventos(liderInicial);

                return carta; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el metodo ObtenerCarta", ex);
            }
        }

        // Metodo para validar y disparar eventos
        private void ValidarYDispararEventos(Jugador liderInicial)
        {
            if (l_cartas_resto.Count == 0)
            {
                publicadorJuego.NotificarAgotadasResto();
            }
            if (l_cartas_castigo.Count == 0)
            {
                publicadorJuego.NotificarAgotadasCastigo();
            }
            if (l_cartas_premio.Count == 0)
            {
                publicadorJuego.NotificarAgotadasPremio();
            }

            Jugador liderNuevo = ObtenerLider();
            if (liderNuevo != liderInicial)
            {
                publicadorJuego.NotificarCambioLider(liderNuevo);
            }
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
                throw new Exception("Error en el método AsignarPuntosSegunApuesta: ", ex);
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
                throw new Exception("Error del metodo RepartirCartasIniciales: ", ex);
            }


        }

        // Aplicar efectos de las cartas y devolver puntos
        public int AplicarEfectoCartas(Carta carta)
        {
            if (carta == null)
                throw new Exception(nameof(carta));

            int puntos;

            switch (carta)
            {
                case CartaJuego juego:
                    puntos = juego.ObtenerPuntos();
                    break;
                case CartaPremio premio:
                    puntos = premio.ObtenerPuntos();
                    break;
                case CartaCastigo castigo:
                    puntos = castigo.ObtenerPuntos();
                    break;
                default:
                    return 0;
            }

            return puntos;
        }

        public void imprimir()
        {
            foreach (CartaJuego c in L_cartas_resto)
                Console.WriteLine($"{c.Nombre}");
        }

    }
}