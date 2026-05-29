namespace MVC_ProyectoFinalPOO.Entities
{
    public class Estadistica
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int PartidasJugadas { get; set; }
        public int PartidasGanadas { get; set; }
        public int PartidasPerdidas { get; set; }
        public int PuntosTotales { get; set; }
        public int MejorPuntuacion { get; set; }
        public double PromedioPuntos { get; set; }

        public DateTime UltimaPartida { get; set; } = DateTime.UtcNow;
    }
}
