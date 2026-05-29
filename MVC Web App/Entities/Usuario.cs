namespace MVC_ProyectoFinalPOO.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nickname { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoLogin { get; set; }

        public ICollection<Partida> Partidas { get; set; } = new List<Partida>();
        public Estadistica? Estadistica { get; set; }
    }
}
