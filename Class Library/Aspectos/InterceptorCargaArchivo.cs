/*
 * Copyright (C) 2026 Santiago Hernandez M
 *
 * This file is part of Blessings & Curses.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * See the LICENSE file for details.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace CL_ProyectoFinalPOO.Aspectos
{
    public class InterceptorCargaArchivo : IInterceptor
    {
        private readonly ILogger<InterceptorCargaArchivo> _logger;

        public InterceptorCargaArchivo(ILogger<InterceptorCargaArchivo> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "CargarCartas" && invocation.Arguments.Length > 0 && invocation.Arguments[0] is string rutaArchivo)
            {
                if (!File.Exists(rutaArchivo))
                {
                    throw new FileNotFoundException($"Error de validación: El archivo '{rutaArchivo}' no se encontró.");
                }

                if (Path.GetExtension(rutaArchivo).ToLower() != ".json")
                {
                    throw new ArgumentException($"Error de validación: El archivo '{rutaArchivo}' debe ser un JSON válido (.json).");
                }

                if (new FileInfo(rutaArchivo).Length == 0)
                {
                    throw new InvalidOperationException($"Error de validación: El archivo '{rutaArchivo}' está vacío.");
                }

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

                    throw new InvalidOperationException($"Error de validación inesperado al procesar el contenido de '{rutaArchivo}'. Detalles: {ex.Message}", ex);
                }

                invocation.Proceed();
                _logger.LogInformation("Exito: Archivo {RutaArchivo} validado y cargado correctamente.", rutaArchivo);
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
