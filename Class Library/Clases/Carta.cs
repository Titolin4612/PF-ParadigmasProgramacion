using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO.Clases
{
    public abstract class Carta
    {
        // Atributos
        public string _nombre;
        private string _mitologia;
        private string _descripcion;



        // Accesores
        public string Nombre
        {
            get => _nombre;
            set => _nombre = value = !(string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value)) ? value
                : throw new Exception("Error, el nombre es inválido.");

        }
        public string Mitologia
        {
            get => _mitologia;
            set => _mitologia = value = !(string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value)) ? value
                : throw new Exception("Error, la mitología es inválida.");
        }
        public string Descripcion
        {
            get => _descripcion;
            set => _descripcion = value = !(string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value)) && !(value.Length <= 2) ? value
                : throw new Exception("Error, la descripcion es inválida.");
        }


        // Constructor
        public Carta(string nombre, string descripcion, string mitologia)
        {
            Nombre = nombre;
            Mitologia = mitologia;
            Descripcion = descripcion;
        }
    }
}
