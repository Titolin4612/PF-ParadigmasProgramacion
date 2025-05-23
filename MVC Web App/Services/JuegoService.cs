// MVC_ProyectoFinalPOO/Services/JuegoService.cs
using CL_ProyectoFinalPOO.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace MVC_ProyectoFinalPOO.Services
{
    public class JuegoService
    {
        private Juego _juegoActual; // Instancia del juego en curso. Será gestionada por este servicio Singleton.
        private readonly HomeService _homeService; // Para limpiar la configuración de jugadores

        // Constructor para Inyección de Dependencias
        public JuegoService(HomeService homeService)
        {
            _homeService = homeService;
            // _juegoActual se inicializará en IniciarJuego.
            // No se instancia un 'Juego' aquí porque podría no haber uno activo siempre.
            Debug.WriteLine("JuegoService: Instancia Singleton creada por DI.");
        }

        public bool EstaJuegoActivo()
        {
            // Un juego está activo si _juegoActual existe y tiene jugadores.
            // Asumimos que CL_ProyectoFinalPOO.Clases.Juego.Jugadores es la lista autoritativa
            // si la clase Juego sigue usando estáticos para ello.
            // Si CL_ProyectoFinalPOO.Clases.Juego fuera completamente instanciable (sin estáticos para jugadores),
            // sería: return _juegoActual != null && _juegoActual.Jugadores != null && _juegoActual.Jugadores.Any();
            return _juegoActual != null && CL_ProyectoFinalPOO.Clases.Juego.Jugadores != null && CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Any();
        }

        public void IniciarJuego(List<Jugador> jugadoresConfigurados)
        {
            if (jugadoresConfigurados == null || !jugadoresConfigurados.Any())
            {
                Debug.WriteLine("JuegoService.IniciarJuego: Intento de iniciar juego sin jugadores configurados.");
                throw new ArgumentException("Se requiere al menos un jugador configurado para iniciar el juego.");
            }

            try
            {
                Debug.WriteLine($"JuegoService.IniciarJuego: Iniciando juego con {jugadoresConfigurados.Count} jugadores.");
                _juegoActual = new Juego(); // Crea una NUEVA instancia de la lógica del juego.

                // IMPORTANTE: La clase CL_ProyectoFinalPOO.Clases.Juego parece usar listas ESTÁTICAS
                // (Juego.Jugadores, Juego.IndiceJugador). Esto significa que aunque _juegoActual es una instancia,
                // el estado subyacente de jugadores, etc., es compartido globalmente por la clase Juego.
                // Para un Singleton JuegoService, esto es consistente (un único juego global).
                CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Clear();
                CL_ProyectoFinalPOO.Clases.Juego.IndiceJugador = 0;

                foreach (var jConf in jugadoresConfigurados)
                {
                    // El constructor de Jugador toma la instancia de _juegoActual.
                    var nuevoJugador = new Jugador(jConf.Nickname, jConf.ApuestaInicial, _juegoActual);
                    // AsignarPuntosSegunApuesta está en la clase Juego y opera sobre el jugador.
                    _juegoActual.AsignarPuntosSegunApuesta(nuevoJugador);
                    // Se añade a la lista estática de la clase Juego.
                    CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Add(nuevoJugador);
                    Debug.WriteLine($"JuegoService.IniciarJuego: Jugador '{nuevoJugador.Nickname}' añadido con {nuevoJugador.Puntos} puntos.");
                }

                _juegoActual.IniciarRonda(); // Prepara mazos, reparte cartas iniciales, etc.
                _homeService.LimpiarConfiguracionJugadores(); // Limpia la lista de configuración de HomeService
                Debug.WriteLine("JuegoService.IniciarJuego: Juego iniciado y configuración de HomeService limpiada.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.IniciarJuego: Error crítico - {ex.Message}");
                _juegoActual = null; // Asegura que el juego no se considere activo si falla la inicialización.
                // Re-lanzar para que el controlador lo maneje
                throw new Exception("Error crítico en JuegoService al intentar iniciar el juego.", ex);
            }
        }

        public Jugador ObtenerJugadorActual()
        {
            if (!EstaJuegoActivo())
            {
                Debug.WriteLine("JuegoService.ObtenerJugadorActual: No hay juego activo.");
                return null;
            }
            // Accede a los miembros estáticos de la clase Juego para el jugador actual
            return CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Count == 0 ? null : CL_ProyectoFinalPOO.Clases.Juego.Jugadores[CL_ProyectoFinalPOO.Clases.Juego.IndiceJugador];
        }

        public (Carta carta, int puntos, object cartaViewModel) CogerCarta()
        {
            if (!EstaJuegoActivo())
            {
                Debug.WriteLine("JuegoService.CogerCarta: Intento de coger carta sin juego activo.");
                throw new InvalidOperationException("El juego no ha sido iniciado o no hay jugadores.");
            }

            var jugadorActual = ObtenerJugadorActual();
            if (jugadorActual == null)
            {
                Debug.WriteLine("JuegoService.CogerCarta: No se pudo determinar el jugador actual.");
                throw new InvalidOperationException("No se pudo determinar el jugador actual.");
            }

            try
            {
                // _juegoActual.ObtenerCarta() debe manejar la lógica de sacar una carta del mazo correcto.
                var carta = _juegoActual.ObtenerCarta();
                int puntosObtenidos = 0;
                object cartaViewModel = null;

                if (carta != null)
                {
                    // _juegoActual.AplicarEfectoCartas() calcula los puntos de la carta.
                    puntosObtenidos = _juegoActual.AplicarEfectoCartas(carta);

                    jugadorActual.Puntos += puntosObtenidos;
                    jugadorActual.L_cartas_jugador.Add(carta); // Agrega la carta a la mano del jugador.

                    // Preparar el ViewModel para la carta revelada
                    string tipoCartaStr = "desconocido";
                    string rareza = null, bendicion = null, maleficio = null;

                    if (carta is CartaJuego cj) { tipoCartaStr = "juego"; rareza = cj.RarezaCarta.ToString(); }
                    else if (carta is CartaPremio cp) { tipoCartaStr = "premio"; bendicion = cp.Bendicion; }
                    else if (carta is CartaCastigo cc) { tipoCartaStr = "castigo"; maleficio = cc.Maleficio; }

                    cartaViewModel = new
                    {
                        TipoCarta = tipoCartaStr,
                        carta.Nombre,
                        carta.Mitologia,
                        carta.Descripcion,
                        carta.ImagenUrl,
                        Puntos = puntosObtenidos,
                        Rareza = rareza,
                        Bendicion = bendicion,
                        Maleficio = maleficio
                    };
                    Debug.WriteLine($"JuegoService.CogerCarta: Jugador '{jugadorActual.Nickname}' cogió '{carta.Nombre}'. Puntos efecto: {puntosObtenidos}. Puntos totales: {jugadorActual.Puntos}.");
                }
                else
                {
                    Debug.WriteLine("JuegoService.CogerCarta: No se pudo obtener una carta (mazo posiblemente vacío).");
                }

                // Validar eventos DESPUÉS de aplicar efectos y actualizar jugador.
                // El líder podría haber cambiado.
                _juegoActual.ValidarYDispararEventos(null); // O pasar el líder si es necesario para la lógica de eventos.

                return (carta, puntosObtenidos, cartaViewModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.CogerCarta: Error - {ex.Message}");
                throw new Exception("Error en JuegoService al procesar CogerCarta.", ex);
            }
        }

        public void PasarTurno()
        {
            if (!EstaJuegoActivo())
            {
                Debug.WriteLine("JuegoService.PasarTurno: Intento de pasar turno sin juego activo.");
                throw new InvalidOperationException("El juego no ha sido iniciado.");
            }
            try
            {
                Jugador liderInicial = _juegoActual.ObtenerLider(); // Obtener líder ANTES de cualquier cambio de turno o estado.
                _juegoActual.ValidarYDispararEventos(liderInicial);

                // Solo pasar turno si el juego no ha terminado y hay jugadores.
                if (!JuegoTerminado() && CL_ProyectoFinalPOO.Clases.Juego.Jugadores != null && CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Any())
                {
                    _juegoActual.PasarTurno();
                    Debug.WriteLine($"JuegoService.PasarTurno: Turno pasado. Nuevo jugador actual: {ObtenerJugadorActual()?.Nickname ?? "Ninguno"}.");
                }
                else
                {
                    Debug.WriteLine("JuegoService.PasarTurno: No se pasó el turno, el juego terminó o no hay jugadores.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.PasarTurno: Error - {ex.Message}");
                throw new Exception("Error en JuegoService al intentar pasar el turno.", ex);
            }
        }

        public Jugador FinalizarJuego()
        {
            if (!EstaJuegoActivo())
            {
                Debug.WriteLine("JuegoService.FinalizarJuego: No hay juego activo para finalizar.");
                // Si se llama múltiples veces o no hay juego, podría devolver null o el estado ya finalizado.
                return _juegoActual?.ObtenerLider(); // Intenta obtener el líder si _juegoActual existe.
            }
            try
            {
                // El método FinalizarJuego en la clase Juego debe manejar la lógica de determinar el ganador
                // y actualizar su estado (ej. puntos adicionales).
                var ganador = _juegoActual.FinalizarJuego();
                Debug.WriteLine($"JuegoService.FinalizarJuego: Juego finalizado. Ganador: {ganador?.Nickname ?? "Ninguno"}.");
                // No anulamos _juegoActual aquí para permitir "NuevaRonda" o "VerResumen" con el estado final.
                // Se anulará en ReiniciarJuego().
                return ganador;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.FinalizarJuego: Error - {ex.Message}");
                throw new Exception("Error en JuegoService al finalizar el juego.", ex);
            }
        }

        public List<Jugador> ObtenerJugadores()
        {
            // Devuelve la lista estática de la clase Juego si el juego está activo o si tiene jugadores (partida finalizada).
            if (_juegoActual != null)
            {
                return CL_ProyectoFinalPOO.Clases.Juego.Jugadores ?? new List<Jugador>();
            }
            return new List<Jugador>(); // Si no hay _juegoActual, no hay jugadores.
        }

        public List<string> ObtenerHistorial()
        {
            if (_juegoActual == null || _juegoActual.Historial == null)
            {
                return new List<string>();
            }
            try
            {
                return _juegoActual.Historial.ObtenerNotificaciones().ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.ObtenerHistorial: Error - {ex.Message}");
                return new List<string> { $"Error al obtener historial: {ex.Message}" };
            }
        }

        public bool JuegoTerminado()
        {
            if (!EstaJuegoActivo())
            {
                // Si el juego no está activo (ej. _juegoActual es null), se considera terminado para la UI.
                Debug.WriteLine("JuegoService.JuegoTerminado: No hay juego activo, se considera terminado.");
                return true;
            }
            try
            {
                // La lógica de terminación se basa en el estado de la instancia _juegoActual.
                bool mazosAgotados = _juegoActual.AgotadasResto && _juegoActual.AgotadasCastigo && _juegoActual.AgotadasPremio;
                bool pocosJugadores = CL_ProyectoFinalPOO.Clases.Juego.Jugadores == null || CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Count < _juegoActual.JugadoresMin;

                bool terminado = mazosAgotados || pocosJugadores;
                Debug.WriteLineIf(terminado, "JuegoService.JuegoTerminado: El juego ha terminado.");
                Debug.WriteLineIf(!terminado, "JuegoService.JuegoTerminado: El juego NO ha terminado.");
                return terminado;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.JuegoTerminado: Error - {ex.Message}");
                // En caso de error, es más seguro asumir que terminó para evitar bucles o estados inconsistentes.
                return true;
            }
        }

        public int TotalCartasEnMazo()
        {
            if (!EstaJuegoActivo())
            {
                return 0;
            }
            try
            {
                // Accede a las listas de cartas estáticas de la clase Juego.
                return CL_ProyectoFinalPOO.Clases.Juego.L_cartas_resto.Count +
                       CL_ProyectoFinalPOO.Clases.Juego.L_cartas_castigo.Count +
                       CL_ProyectoFinalPOO.Clases.Juego.L_cartas_premio.Count;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.TotalCartasEnMazo: Error - {ex.Message}");
                return 0;
            }
        }

        public void ReiniciarJuego() // Para volver a la pantalla de Home (configuración).
        {
            try
            {
                Debug.WriteLine("JuegoService.ReiniciarJuego: Reiniciando estado del juego.");
                _juegoActual = null; // Anula la instancia del juego actual.

                // Limpia las listas estáticas de la clase Juego.
                CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Clear();
                CL_ProyectoFinalPOO.Clases.Juego.IndiceJugador = 0;
                // Asegurarse de que las barajas (L_cartas_resto, etc.) también se limpien o se recarguen
                // si son estáticas y no se manejan automáticamente por `new Juego()`.
                // Baraja.CargarCartas() se llama en `new Juego()`, lo que debería resetear las fuentes
                // pero las listas L_cartas_resto etc. en Juego son estáticas, así que podrían necesitar limpieza explícita.
                // Sin embargo, `new Juego()` en `IniciarJuego` ya copia de Baraja a estas listas estáticas,
                // lo que efectivamente las "reinicia" para un nuevo juego.

                _homeService.LimpiarConfiguracionJugadores(); // Borra jugadores de la configuración de Home.
                Debug.WriteLine("JuegoService.ReiniciarJuego: Juego reiniciado y configuración de HomeService limpiada.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.ReiniciarJuego: Error - {ex.Message}");
                throw new Exception("Error en JuegoService al reiniciar el juego.", ex);
            }
        }

        public void ComenzarNuevaRondaConJugadoresActuales()
        {
            if (!EstaJuegoActivo()) // Necesita un juego existente (probablemente finalizado).
            {
                Debug.WriteLine("JuegoService.ComenzarNuevaRonda: No hay juego activo para iniciar nueva ronda.");
                throw new InvalidOperationException("No hay un juego activo para iniciar una nueva ronda. Configure un nuevo juego.");
            }
            if (CL_ProyectoFinalPOO.Clases.Juego.Jugadores == null ||
                !CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Any() ||
                CL_ProyectoFinalPOO.Clases.Juego.Jugadores.Count < _juegoActual.JugadoresMin)
            {
                Debug.WriteLine("JuegoService.ComenzarNuevaRonda: No hay suficientes jugadores para nueva ronda.");
                throw new InvalidOperationException("No hay suficientes jugadores actuales para comenzar una nueva ronda.");
            }

            try
            {
                Debug.WriteLine("JuegoService.ComenzarNuevaRonda: Iniciando nueva ronda con jugadores actuales.");
                // _juegoActual.IniciarRonda() debe resetear manos, mazos, y mantener puntos de jugadores.
                _juegoActual.IniciarRonda();
                Debug.WriteLine("JuegoService.ComenzarNuevaRonda: Nueva ronda iniciada.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JuegoService.ComenzarNuevaRonda: Error - {ex.Message}");
                throw new Exception("Error en JuegoService al comenzar una nueva ronda.", ex);
            }
        }
    }
}