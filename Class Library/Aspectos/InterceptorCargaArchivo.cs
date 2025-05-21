using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace CL_ProyectoFinalPOO.Aspectos
{
    public class InterceptorCargaArchivo : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //if (invocation.Method.Name == "CargarCartas")
            //{
            //    string rutaArchivo = (string)invocation.Arguments[0];

            //    // Validación 1: Existencia del archivo
            //    if (!File.Exists(rutaArchivo))
            //    {
            //        throw new FileNotFoundException($"El archivo {rutaArchivo} no se encontró.");
            //    }

            //    // Validación 2: Extensión correcta
            //    if (Path.GetExtension(rutaArchivo).ToLower() != ".json")
            //    {
            //        throw new ArgumentException("El archivo debe ser un JSON válido (.json).");
            //    }

            //    // Validación 3: Archivo no vacío
            //    if (new FileInfo(rutaArchivo).Length == 0)
            //    {
            //        throw new InvalidOperationException("El archivo está vacío.");
            //    }

            //    // Validación 4: Contenido JSON válido
            //    try
            //    {
            //        string contenido = File.ReadAllText(rutaArchivo);
            //        var jsonDoc = JsonDocument.Parse(contenido);
            //        if (!jsonDoc.RootElement.EnumerateArray().Any())
            //        {
            //            throw new InvalidOperationException("El archivo JSON no contiene cartas.");
            //        }

            //        // Validación 5: Campos obligatorios
            //        foreach (var elemento in jsonDoc.RootElement.EnumerateArray())
            //        {
            //            if (!elemento.TryGetProperty("Nombre", out _) || !elemento.TryGetProperty("Tipo", out _))
            //            {
            //                throw new InvalidOperationException("Una carta en el JSON carece de 'Nombre' o 'Tipo'.");
            //            }
            //        }
            //    }
            //    catch (JsonException ex)
            //    {
            //        throw new InvalidOperationException("El archivo JSON tiene un formato inválido.", ex);
            //    }

            //    // Ejecutar el método original
            //    invocation.Proceed();
            //    Console.WriteLine($"Éxito: Archivo {rutaArchivo} cargado correctamente.");
            //}
            //else
            //{
            //    invocation.Proceed(); // Métodos no relacionados pasan sin validación
            //}
        }
    }
}
