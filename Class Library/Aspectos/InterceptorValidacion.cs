using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using CL_ProyectoFinalPOO.Clases;

namespace CL_ProyectoFinalPOO.Aspectos
{
    public class InterceptorValidacion : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "ActualizarPuntos")
            {
                var jugador = (Jugador)invocation.Arguments[0];
                int puntosNuevos = (int)invocation.Arguments[1];

                if (string.IsNullOrEmpty(jugador.Nickname))
                    throw new InvalidOperationException("El nickname del jugador es obligatorio.");
                if (puntosNuevos < 0)
                    throw new InvalidOperationException("Los puntos no pueden ser negativos.");
                if (puntosNuevos > 1000)
                    throw new InvalidOperationException("Los puntos exceden el máximo permitido (1000).");

                System.Threading.Thread.Sleep(100); // Simula latencia de DB
                invocation.Proceed();
                Console.WriteLine($"Éxito: Los puntos de {jugador.Nickname} se actualizaron a {puntosNuevos}.");
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
