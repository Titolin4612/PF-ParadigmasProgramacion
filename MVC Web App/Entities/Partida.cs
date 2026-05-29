namespace MVC_ProyectoFinalPOO.Entities
{
    public class Partida
    {
        public int Id { get; set; }
        public required string NombreGanador { get; set; }
        public int PuntosGanador { get; set; }
        public int CantidadJugadores { get; set; }
        public int CartasJugadas { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
