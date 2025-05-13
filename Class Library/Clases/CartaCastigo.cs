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
        // Valor carta
        private static int vCastigo = -5;
        

        // Accesor
        public string Maleficio
        {
            get => _maleficio;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Error, el maleficio es inválido.");
                _maleficio = value;
            }
        }
        public static int VCastigo { get => vCastigo; }


        // Constructor
        public CartaCastigo(string nombre, string descripcion, string mitologia, string maleficio) : base (nombre, descripcion, mitologia)
        { 
            Maleficio = maleficio;
        }

        public override int ObtenerPuntos()
        {
            return VCastigo;
        }

    }
}
