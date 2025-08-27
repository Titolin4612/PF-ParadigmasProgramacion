// CL_ProyectoFinalPOO/Clases/Juego.cs
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
        private List<Jugador> jugadores;
        private List<CartaPremio> l_cartas_premio;
        private List<CartaCastigo> l_cartas_castigo;
        private List<CartaJuego> l_cartas_resto;

        // Atributos
        private int indiceJugadorActual;
        private string nicknameLiderAnterior = null;

        // Random
        private static Random rng = new Random();

        // Propiedad baraja
        public Baraja baraja { get; set; }

        // Instancia de clases publicadoras
        private Publisher_Eventos_Juego publicadorJuego;
        private Publisher_Eventos_Jugador publicadorJugador;
        private Publisher_Eventos_Cartas publicadorCartas;
        private Historial historial;

        // Atributos de reglas de negocio
        private int cartasPorJugador = 3;
        private int jugadoresMin = 2;
        private int jugadoresMax = 4;
        private bool agotadasResto = false;
        private bool agotadasCastigo = false;
        private bool agotadasPremio = false;

        // Accesores
        public List<Jugador> Jugadores { get => jugadores; set { if (value == null) throw new ArgumentException("La lista de jugadores no puede ser nula."); jugadores = value; } }
        public List<CartaPremio> L_cartas_premio { get => l_cartas_premio; set { if (value == null) throw new ArgumentException("La lista de cartas premio no puede ser nula."); l_cartas_premio = value; } }
        public List<CartaCastigo> L_cartas_castigo { get => l_cartas_castigo; set { if (value == null) throw new ArgumentException("La lista de cartas castigo no puede ser nula."); l_cartas_castigo = value; } }
        public List<CartaJuego> L_cartas_resto { get => l_cartas_resto; set { if (value == null) throw new ArgumentException("La lista de cartas resto no puede ser nula."); l_cartas_resto = value; } }
        public int CartasPorJugador { get => cartasPorJugador; }
        public int JugadoresMin { get => jugadoresMin; }
        public int JugadoresMax { get => jugadoresMax; }
        public int IndiceJugador { get => indiceJugadorActual; set => indiceJugadorActual = value; }
        public Historial Historial { get => historial; set { if (value == null) throw new ArgumentException("El historial no puede ser nulo."); historial = value; } }
        public Publisher_Eventos_Juego PublicadorJuego { get => publicadorJuego; set { if (value == null) throw new ArgumentException("El publicador de eventos de juego no puede ser nulo."); publicadorJuego = value; } }
        public Publisher_Eventos_Jugador PublicadorJugador { get => publicadorJugador; set { if (value == null) throw new ArgumentException("El publicador de eventos de jugador no puede ser nulo."); publicadorJugador = value; } }
        public Publisher_Eventos_Cartas PublicadorCartas { get => publicadorCartas; set { if (value == null) throw new ArgumentException("El publicador de eventos de cartas no puede ser nulo."); publicadorCartas = value; } }
        public bool AgotadasResto { get => agotadasResto; set => agotadasResto = value; }
        public bool AgotadasCastigo { get => agotadasCastigo; set => agotadasCastigo = value; }
        public bool AgotadasPremio { get => agotadasPremio; set => agotadasPremio = value; }
        public string NicknameLiderAnterior { get => nicknameLiderAnterior; set => nicknameLiderAnterior = value; }

        // Constructor
        public Juego()
        {

            baraja = new Baraja();
            baraja.CargarCartas(); 

            L_cartas_resto = new List<CartaJuego>(baraja.CartasJuego);
            L_cartas_premio = new List<CartaPremio>(baraja.CartasPremio);
            L_cartas_castigo = new List<CartaCastigo>(baraja.CartasCastigo);

            Jugadores = new List<Jugador>();
            indiceJugadorActual = 0;

            PublicadorJuego = new Publisher_Eventos_Juego();
            PublicadorJugador = new Publisher_Eventos_Jugador();
            PublicadorCartas = new Publisher_Eventos_Cartas();
            Historial = new Historial(PublicadorJuego, PublicadorJugador, PublicadorCartas);
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
                else Console.WriteLine("Error"); // Deberías manejar este error de forma más robusta, ej. lanzar una excepción.
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
                Jugador liderInicial = ObtenerLider();

                // Calcular probabilidades
                double probCartaJuego = (double)L_cartas_resto.Count / totalCartas;
                double probCartaCastigo = (double)L_cartas_castigo.Count / totalCartas;

                Carta carta = null;
                double random = rng.NextDouble();

                if (totalCartas == 0) return null; 

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
                else if (L_cartas_premio.Count > 0) // Por si las otras están vacías
                {
                    int index = rng.Next(L_cartas_premio.Count);
                    carta = L_cartas_premio[index];
                    L_cartas_premio.RemoveAt(index);
                }

                else if (L_cartas_resto.Count > 0)
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

                AplicarEfectoCartas(carta);

                return carta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el metodo ObtenerCarta", ex);
            }
        }

        // Metodo para validar y disparar eventos
        public void ValidarYDispararEventos(Jugador liderInicial)
        {
            try
            {
                if (l_cartas_resto.Count == 0 && !agotadasResto)
                {
                    PublicadorCartas.NotificarAgotadasResto();
                    agotadasResto = true;
                }

                if (l_cartas_castigo.Count == 0 && !agotadasCastigo)
                {
                    PublicadorCartas.NotificarAgotadasCastigo();
                    agotadasCastigo = true;
                }

                if (l_cartas_premio.Count == 0 && !agotadasPremio)
                {
                    PublicadorCartas.NotificarAgotadasPremio();
                    agotadasPremio = true;
                }

                Jugador liderNuevo = ObtenerLider();
                if (liderNuevo != null && liderNuevo.Nickname != nicknameLiderAnterior)
                {
                    PublicadorJugador.NotificarCambioLider(liderNuevo, this);
                    nicknameLiderAnterior = liderNuevo.Nickname;
                }

                for (int i = Jugadores.Count - 1; i >= 0; i--)
                {
                    Jugador jugador = Jugadores[i];
                    if (jugador.Puntos <= 0 && !jugador.Perdio)
                    {
                        PublicadorJugador.NotificarJugadorSinPuntos(jugador);
                        jugador.Perdio = true;
                        Jugadores.RemoveAt(i); 

                        // Ajusta el índice del jugador 
                        if (i <= IndiceJugador)
                        {
                            if (IndiceJugador > 0)
                            {
                                IndiceJugador--;
                            }
                        }
                    }
                }

                // Ajusta el índice del jugador
                if (Jugadores.Count > 0 && IndiceJugador >= Jugadores.Count)
                {
                    IndiceJugador = 0;
                }
                else if (Jugadores.Count == 0)
                {
                    IndiceJugador = 0;
                }

                if ((agotadasResto && agotadasCastigo && agotadasPremio) || Jugadores.Count < jugadoresMin)
                {
                    FinalizarJuego();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ValidarYDispararEventos: ", ex);
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
                throw new Exception("Error en AsignarPuntosSegunApuesta: ", ex);
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

                if (l_cartas_resto.Count < numeroCartasPorJugador * Jugadores.Count)
                {
                    throw new Exception("No hay suficientes cartas de juego en el mazo para repartir las cartas iniciales.");
                }

                foreach (var jugador in Jugadores)
                {
                    for (int i = 0; i < numeroCartasPorJugador; i++)
                    {
                        var carta = l_cartas_resto.First();
                        l_cartas_resto.RemoveAt(0);
                        jugador.L_cartas_jugador.Add(carta);
                        jugador.Puntos += AplicarEfectoCartas(carta);
                        PublicadorCartas.NotificarCartasObtenidas(jugador, carta);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en RepartirCartasIniciales: ", ex);
            }
        }

        // Aplicar efectos de las cartas y devolver puntos
        public int AplicarEfectoCartas(Carta carta)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Error en AplicarEfectoCartas: ", ex);
            }
        }

        // Método para finalizar el juego
        public Jugador FinalizarJuego()
        {
            try
            {
                if (Jugadores == null || Jugadores.Count == 0)
                    return null;

                Jugador ganador = ObtenerLider();
                if (ganador != null)
                {
                    ganador.Puntos += 20; // Bonificación al ganador
                    PublicadorJuego.NotificarFinPartida(ganador);
                }
                return ganador;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en FinalizarJuego: ", ex);
            }
        }

        // Método para iniciar una ronda
        public void IniciarRonda()
        {
            try
            {
                PublicadorJuego.NotificarInicioPartida();

                baraja.CargarCartas();

                L_cartas_resto = new List<CartaJuego>(baraja.CartasJuego);
                L_cartas_premio = new List<CartaPremio>(baraja.CartasPremio);
                L_cartas_castigo = new List<CartaCastigo>(baraja.CartasCastigo);
                BarajarCartas();

                if (Jugadores != null && Jugadores.Any())
                {
                    foreach (var jugador in Jugadores)
                    {
                        jugador.L_cartas_jugador.Clear(); 

                        jugador.Perdio = false; 
                    }
                }
                else
                {
                    Jugadores = new List<Jugador>(); 
                    throw new InvalidOperationException("No se pueden iniciar una ronda sin jugadores. Por favor, agregue jugadores primero.");
                }

                RepartirCartasIniciales(CartasPorJugador);
                IndiceJugador = 0;
                AgotadasResto = false;
                AgotadasCastigo = false;
                AgotadasPremio = false;
                NicknameLiderAnterior = null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en IniciarRonda: ", ex);
            }
        }

        // Método para pasar el turno
        public void PasarTurno()
        {
            try
            {
                if (Jugadores.Count > 0)
                {
                    IndiceJugador = (IndiceJugador + 1) % Jugadores.Count;
                }
                else
                {
                    throw new Exception("No hay jugadores en el juego.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en PasarTurno: ", ex);
            }
        }
    }
}