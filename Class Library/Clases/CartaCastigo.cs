using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Interfaces;

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
            Maleficio = maleficio;
        }

        // Metodos
        public override int ActualizarPuntos() => -5;
    }
}
