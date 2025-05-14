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
            else
            {
                _notificaciones.Add($"{DateTime.Now:HH:mm:ss} - Cambio de líder, pero no hay un líder claro (posiblemente no hay jugadores o empate sin resolver).");
            }
        }

        public IReadOnlyList<string> ObtenerNotificaciones()
        {
            return _notificaciones.AsReadOnly();
        }

        // Opcional: Método para desuscribirse si es necesario (ej. en un Dispose)
        public void DesuscribirEventos()
        {
            _publisher.AgotadasPremio -= AgotadasPremioHandler;
            _publisher.AgotadasCastigo -= AgotadasCastigoHandler;
            _publisher.AgotadasResto -= AgotadasRestoHandler;
            _publisher.CambioLider -= CambioLiderHandler;
        }
    }
}