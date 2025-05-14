using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace CL_ProyectoFinalPOO.Aspectos
{
    public class InterceptorAutenticacion : IInterceptor
    {
        private bool IsAuthenticated()
        {
            // Simulación: Cambia a true para pruebas positivas
            return true;
        }

        public void Intercept(IInvocation invocation)
        {
            if (!IsAuthenticated())
                throw new UnauthorizedAccessException(
                    $"Acceso denegado al método {invocation.Method.Name}. Inicie sesión primero.");
            invocation.Proceed();
            Console.WriteLine($"Acceso autorizado al método {invocation.Method.Name}.");
        }
    }
}
