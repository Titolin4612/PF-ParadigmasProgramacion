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
            // We only want to intercept the "CargarCartas" method.
            // Note: In a real-world scenario, you might use custom attributes
            // or more sophisticated checks to decide when to apply the interceptor.
            if (invocation.Method.Name == "CargarCartas" && invocation.Arguments.Length > 0 && invocation.Arguments[0] is string rutaArchivo)
            {
                // Validation 1: Archivo existe
                if (!File.Exists(rutaArchivo))
                {
                    throw new FileNotFoundException($"Error de validación: El archivo '{rutaArchivo}' no se encontró.");
                }

                // Validation 2: Extensión correcta
                if (Path.GetExtension(rutaArchivo).ToLower() != ".json")
                {
                    throw new ArgumentException($"Error de validación: El archivo '{rutaArchivo}' debe ser un JSON válido (.json).");
                }

                // Validation 3: Archivo no vacío
                if (new FileInfo(rutaArchivo).Length == 0)
                {
                    throw new InvalidOperationException($"Error de validación: El archivo '{rutaArchivo}' está vacío.");
                }

                // Validation 4 & 5: Contenido JSON válido y campos obligatorios
                try
                {
                    string contenido = File.ReadAllText(rutaArchivo);
                    var jsonDoc = JsonDocument.Parse(contenido);

                    if (jsonDoc.RootElement.ValueKind != JsonValueKind.Array)
                    {
                        throw new InvalidOperationException($"Error de validación: El contenido del archivo '{rutaArchivo}' no es un array JSON válido.");
                    }

                    if (!jsonDoc.RootElement.EnumerateArray().Any())
                    {
                        throw new InvalidOperationException($"Error de validación: El archivo JSON '{rutaArchivo}' no contiene ninguna carta.");
                    }

                    foreach (var elemento in jsonDoc.RootElement.EnumerateArray())
                    {
                        if (elemento.ValueKind != JsonValueKind.Object)
                        {
                            throw new InvalidOperationException($"Error de validación: Un elemento en el archivo JSON '{rutaArchivo}' no es un objeto JSON.");
                        }

                        if (!elemento.TryGetProperty("Nombre", out JsonElement nombreElement) || string.IsNullOrWhiteSpace(nombreElement.GetString()))
                        {
                            throw new InvalidOperationException($"Error de validación: Una carta en el JSON '{rutaArchivo}' carece de un 'Nombre' válido.");
                        }
                        if (!elemento.TryGetProperty("Tipo", out JsonElement tipoElement) || string.IsNullOrWhiteSpace(tipoElement.GetString()))
                        {
                            throw new InvalidOperationException($"Error de validación: Una carta en el JSON '{rutaArchivo}' carece de un 'Tipo' válido.");
                        }
                    }
                }
                catch (JsonException ex)
                {
                    throw new InvalidOperationException($"Error de validación: El archivo JSON '{rutaArchivo}' tiene un formato inválido. Detalles: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    // Catch any other unexpected errors during content validation
                    throw new InvalidOperationException($"Error de validación inesperado al procesar el contenido de '{rutaArchivo}'. Detalles: {ex.Message}", ex);
                }

                // If all validations pass, proceed with the original method execution
                invocation.Proceed();
                Console.WriteLine($"Éxito: Archivo {rutaArchivo} validado y cargado correctamente.");
            }
            else
            {
                // If it's not the CargarCartas method, just proceed normally.
                invocation.Proceed();
            }
        }
    }
}