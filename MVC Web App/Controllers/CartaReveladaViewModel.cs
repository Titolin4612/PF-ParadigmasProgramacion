namespace MVC_ProyectoFinalPOO.Controllers
{
    public class CartaReveladaViewModel
    {
        public string TipoCarta { get; set; }
        public string Nombre { get; set; }
        public string Mitologia { get; set; }
        public string Descripcion { get; set; }
        public string ImagenArteUrl { get; set; }
        public int Puntos { get; set; }
        public string Rareza { get; set; }    // Será null/vacío si no aplica
        public string Bendicion { get; set; } // Será null/vacío si no aplica
        public string Maleficio { get; set; } // Será null/vacío si no aplica
    }

}