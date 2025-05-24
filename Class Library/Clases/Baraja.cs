// CL_ProyectoFinalPOO/Clases/Baraja.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Baraja
    {
        // Lists to store the cards - NOW INSTANCE PROPERTIES
        public List<CartaJuego> CartasJuego { get; private set; }
        public List<CartaPremio> CartasPremio { get; private set; }
        public List<CartaCastigo> CartasCastigo { get; private set; }

        // Path to the JSON file (in the same folder as the program) - These can remain static readonly
        public static readonly string _rutaArchivoCartas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cartas.json");
        public static readonly string _rutaBaseImagenesCartas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cartas\\");
        public const string _rutaBaseImagenes = "images/cartas/";

        // Constructor for the Baraja instance (replaces the static constructor for initialization)
        public Baraja()
        {
            CartasJuego = new List<CartaJuego>();
            CartasPremio = new List<CartaPremio>();
            CartasCastigo = new List<CartaCastigo>();
        }

        // Load cards from JSON file - NOW A VIRTUAL INSTANCE METHOD
        // Mark it as 'virtual' so Castle.DynamicProxy can override it.
        public virtual void CargarCartas(string rutaArchivo = null)
        {
            rutaArchivo = rutaArchivo ?? _rutaArchivoCartas;

            // Clear lists via instance references
            CartasJuego.Clear();
            CartasPremio.Clear();
            CartasCastigo.Clear();

            try
            {
                string json = File.ReadAllText(rutaArchivo);
                var cartas = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

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
            }
            catch (Exception ex)
            {
                throw new Exception($"Error interno al procesar el contenido de las cartas: {ex.Message}", ex);
            }
        }
    }
}