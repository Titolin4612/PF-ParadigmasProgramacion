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

// CL_ProyectoFinalPOO/Clases/Baraja.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Baraja
    {
        public List<CartaJuego> CartasJuego { get; private set; } = null!;
        public List<CartaPremio> CartasPremio { get; private set; } = null!;
        public List<CartaCastigo> CartasCastigo { get; private set; } = null!;

        public static readonly string _rutaArchivoCartas = Path.Combine(AppContext.BaseDirectory, "cartas.json");
        public static readonly string _rutaBaseImagenesCartas = Path.Combine(AppContext.BaseDirectory, "cartas\\");
        public const string _rutaBaseImagenes = "images/cartas/";

        public Baraja()
        {
            CartasJuego = new List<CartaJuego>();
            CartasPremio = new List<CartaPremio>();
            CartasCastigo = new List<CartaCastigo>();
        }

        public virtual void CargarCartas(string rutaArchivo = null)
        {
            CargarCartasAsync(rutaArchivo).GetAwaiter().GetResult();
        }

        public virtual async Task CargarCartasAsync(string rutaArchivo = null)
        {
            rutaArchivo = rutaArchivo ?? _rutaArchivoCartas;

            CartasJuego.Clear();
            CartasPremio.Clear();
            CartasCastigo.Clear();

            try
            {
                string json = await File.ReadAllTextAsync(rutaArchivo);
                var cartas = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

                if (cartas == null || cartas.Count == 0)
                {
                    throw new InvalidOperationException("El archivo de cartas no contiene cartas para cargar.");
                }

                foreach (var carta in cartas)
                {
                    string tipo = carta.ContainsKey("Tipo") ? carta["Tipo"].ToLower() : "";
                    string nombre = carta.ContainsKey("Nombre") ? carta["Nombre"] : "";
                    string descripcion = carta.ContainsKey("Descripcion") ? carta["Descripcion"] : "";
                    string mitologia = carta.ContainsKey("Mitologia") ? carta["Mitologia"] : "";
                    string nombreArchivoImagen = carta.ContainsKey("ArchivoImagen") ? carta["ArchivoImagen"] : "";
                    string imagenUrl = "";

                    if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(tipo))
                    {
                        Console.WriteLine($"Advertencia: Carta sin Nombre o Tipo, se ignora durante la carga. (Esto debería ser prevenido por el interceptor)");
                        continue;
                    }

                    if (!string.IsNullOrEmpty(nombreArchivoImagen))
                    {
                        imagenUrl = _rutaBaseImagenes + nombreArchivoImagen;
                    }

                    if (tipo == "juego")
                    {
                        string rarezaStr = carta.ContainsKey("Rareza") ? carta["Rareza"] : "";
                        if (string.IsNullOrEmpty(rarezaStr) || !Enum.TryParse<CartaJuego.Rareza>(rarezaStr, true, out var rarezaEnum))
                        {
                            Console.WriteLine($"Advertencia: Carta '{nombre}' no tiene Rareza válida o es nula/vacía, se ignora.");
                            continue;
                        }
                        CartasJuego.Add(new CartaJuego(nombre, descripcion, mitologia, rarezaEnum, imagenUrl));
                    }
                    else if (tipo == "premio")
                    {
                        string bendicion = carta.ContainsKey("Bendicion") ? carta["Bendicion"] : "";
                        if (string.IsNullOrEmpty(bendicion))
                        {
                            Console.WriteLine($"Advertencia: Carta '{nombre}' no tiene Bendición o es nula/vacía, se ignora.");
                            continue;
                        }
                        CartasPremio.Add(new CartaPremio(nombre, descripcion, mitologia, bendicion, imagenUrl));
                    }
                    else if (tipo == "castigo")
                    {
                        string maleficio = carta.ContainsKey("Maleficio") ? carta["Maleficio"] : "";
                        if (string.IsNullOrEmpty(maleficio))
                        {
                            Console.WriteLine($"Advertencia: Carta '{nombre}' no tiene Maleficio o es nula/vacía, se ignora.");
                            continue;
                        }
                        CartasCastigo.Add(new CartaCastigo(nombre, descripcion, mitologia, maleficio, imagenUrl));
                    }
                    else
                    {
                        Console.WriteLine($"Advertencia: Tipo de carta '{tipo}' desconocido para '{nombre}', se ignora.");
                    }
                }

                if (CartasJuego.Count + CartasPremio.Count + CartasCastigo.Count == 0)
                {
                    throw new InvalidOperationException("El archivo de cartas no contiene cartas válidas.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error interno al procesar el contenido de las cartas: {ex.Message}", ex);
            }
        }
    }
}
