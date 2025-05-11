using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Clases;

namespace CL_ProyectoFinalPOO.Eventos
{
    internal class Publisher_Eventos_Juego
    {
        // Delegado para eventos sin parámetros
        public delegate void DelegadoEvento();

        // Eventos
        public event DelegadoEvento AgotadasPremio;
        public event DelegadoEvento AgotadasCastigo;
        public event DelegadoEvento AgotadasResto;
        public event DelegadoEvento CambioLider;

        // Métodos para disparar eventos
        public string NotificarAgotadasPremio()
        {
            if (AgotadasPremio != null)
            {
                AgotadasPremio();
                return "¡Las cartas de premio se han agotado!";
            }
            else
            {
                return "No hay suscriptores para el evento AgotadasPremio.";
            }
        }

        public string NotificarAgotadasCastigo()
        {
            if (AgotadasCastigo != null)
            {
                AgotadasCastigo();
                return "¡Las cartas de castigo se han agotado!";
            }
            else
            {
                return "No hay suscriptores para el evento AgotadasCastigo.";
            }
        }

        public string NotificarAgotadasResto()
        {
            if (AgotadasResto != null)
            {
                AgotadasResto();
                return "¡Las cartas del resto se han agotado!";
            }
            else
            {
                return "No hay suscriptores para el evento AgotadasResto.";
            }
        }

        public string NotificarCambioLider(Jugador jugador)
        {
            if (CambioLider != null)
            {
                CambioLider();
                return $"¡El líder del juego ha cambiado! Ahora es: {jugador}";
            }
            else
            {
                return "No hay suscriptores para el evento CambioLider.";
            }
        }
    }
}