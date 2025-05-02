using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO.Clases
{
    public class CartaCastigo : Carta
    {
        // Atributo
        private string _maleficio;
        

        // Accesor
        public string Maleficio { get => _maleficio; set => _maleficio = value; }

        // Constructor
        public CartaCastigo(string nombre, string descripcion, string mitologia, string maleficio) : base (nombre, descripcion, mitologia)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Mitologia = mitologia;
            Maleficio = maleficio;
        }
    }
}
