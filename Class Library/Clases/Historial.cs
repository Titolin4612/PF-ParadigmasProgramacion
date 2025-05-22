using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Clases;
using CL_ProyectoFinalPOO.Eventos;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Historial // No necesita implementar ninguna interfaz específica para el enunciado
    {
        private readonly List<string> _notificaciones;
        private readonly Publisher_Eventos_Juego _publisher; // Guardar referencia si se necesita desuscribir

        public Historial(Publisher_Eventos_Juego publisher)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _notificaciones = new List<string>();

            // Suscripción a los eventos
            _publisher.AgotadasPremio += AgotadasPremioHandler;
            _publisher.AgotadasCastigo += AgotadasCastigoHandler;
            _publisher.AgotadasResto += AgotadasRestoHandler;
            _publisher.CambioLider += CambioLiderHandler;
            _publisher.InicioPartida += InicioPartidaHandler;
            _publisher.CartasIniciales += CartasObtenidasHandler;
            _publisher.FinPartida += FinPartidaHandler;
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
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡El líder del juego ha cambiado! Ahora es: {nuevoLider.Nickname} con {nuevoLider.Puntos} puntos.");
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
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡{jugador.Nickname} recibio una carta de Juego: {carta.Nombre} de {carta.ObtenerPuntos()} puntos");
            }
            else if (carta is CartaPremio)
            {
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡{jugador.Nickname} recibio una carta de Premio: {carta.Nombre} de {carta.ObtenerPuntos()} puntos");
            }
            else if (carta is CartaCastigo)
            {
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡{jugador.Nickname} recibio una carta de Castigo: {carta.Nombre} de {carta.ObtenerPuntos()} puntos");
            }

        }

        private void FinPartidaHandler(Jugador ganador)
        {
            if (ganador != null)
            {
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡El juego ha concluido!\nEl ganador fue {ganador.Nickname}, Se le dan 20 puntos adicionales, para un total de {ganador.Puntos} puntos.");
            }
            else
            {
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - ¡El juego ha concluido! No hubo un ganador claro o la partida terminó en empate.");
            }
        }

        public IReadOnlyList<string> ObtenerNotificaciones()
        {
            return _notificaciones.AsReadOnly();
        }

    }
}