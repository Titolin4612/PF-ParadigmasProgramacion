using System;
using System.Collections.Generic;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Eventos;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Historial
    {
        private readonly List<string> _notificaciones;
        private readonly Publisher_Eventos_Juego _publisherJuego;
        private readonly Publisher_Eventos_Jugador _publisherJugador;
        private readonly Publisher_Eventos_Cartas _publisherCartas;

        public Historial(Publisher_Eventos_Juego publisherJuego, Publisher_Eventos_Jugador publisherJugador, Publisher_Eventos_Cartas publisherCartas)
        {
            _publisherJuego = publisherJuego ?? throw new Exception(nameof(publisherJuego));
            _publisherJugador = publisherJugador ?? throw new Exception(nameof(publisherJugador));
            _publisherCartas = publisherCartas ?? throw new Exception(nameof(publisherCartas));
            _notificaciones = new List<string>();

            // Suscripción a los eventos de PublisherEventosJuego
            _publisherJuego.InicioPartida += InicioPartidaHandler;
            _publisherJuego.FinPartida += FinPartidaHandler;

            // Suscripción a los eventos de PublisherEventosJugador
            _publisherJugador.SinPuntos += SinPuntosHandler;
            _publisherJugador.CambioLider += CambioLiderHandler;

            // Suscripción a los eventos de PublisherEventosCartas
            _publisherCartas.AgotadasPremio += AgotadasPremioHandler;
            _publisherCartas.AgotadasCastigo += AgotadasCastigoHandler;
            _publisherCartas.AgotadasResto += AgotadasRestoHandler;
            _publisherCartas.CartasIniciales += CartasObtenidasHandler;
        }

        // Métodos Handler para cada evento
        private void AgotadasPremioHandler()
        {
            _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡Las cartas de premio se han agotado!");
        }

        private void AgotadasCastigoHandler()
        {
            _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡Las cartas de castigo se han agotado!");
        }

        private void AgotadasRestoHandler()
        {
            _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡Las cartas del mazo de juego se han agotado!");
        }

        private void CambioLiderHandler(Jugador nuevoLider)
        {
            if (nuevoLider != null)
            {
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡El líder del juego ha cambiado! Ahora es: {nuevoLider.Nickname} con {nuevoLider.Puntos} puntos!");
            }
        }

        private void InicioPartidaHandler()
        {
            _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡El juego ha comenzado, preparate para probar tu suerte!");
        }

        private void CartasObtenidasHandler(Jugador jugador, Carta carta)
        {
            if (carta is CartaJuego)
            {
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡{jugador.Nickname} recibio una carta de Juego: {carta.Nombre} de {carta.ObtenerPuntos()} puntos!");
            }
            else if (carta is CartaPremio)
            {
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡{jugador.Nickname} recibio una carta de Premio: {carta.Nombre} de {carta.ObtenerPuntos()} puntos!");
            }
            else if (carta is CartaCastigo)
            {
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡{jugador.Nickname} recibio una carta de Castigo: {carta.Nombre} de {carta.ObtenerPuntos()} puntos!");
            }
        }

        public void SinPuntosHandler(Jugador jugador)
        {
            _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡El jugador {jugador.Nickname} se ha quedado sin puntos, pierde el juego.!");
        }

        private void FinPartidaHandler(Jugador ganador)
        {
            if (ganador != null)
            {
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡EL JUEGO HA FINALIZADO!\nEl ganador fue {ganador.Nickname}, Se le dan 20 puntos adicionales, para un total de {ganador.Puntos} puntos.!");
            }
            else
            {
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡EL JUEGO HA FINALIZADO! No hubo un ganador claro o la partida terminó en empate!");
            }
        }

        public IReadOnlyList<string> ObtenerNotificaciones()
        {
            return _notificaciones.AsReadOnly();
        }
    }
}