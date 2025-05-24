using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Baraja
    {
        // Lists to store the cards
        public static List<CartaJuego> CartasJuego { get; private set; }
        public static List<CartaPremio> CartasPremio { get; private set; }
        public static List<CartaCastigo> CartasCastigo { get; private set; }

        // Path to the JSON file (in the same folder as the program)
        public static readonly string _rutaArchivoCartas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cartas.json");
        public static readonly string _rutaBaseImagenesCartas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cartas\\");
        public const string _rutaBaseImagenes = "images/cartas/";

        public static void Ruta()
        {
            Console.WriteLine(_rutaArchivoCartas);
            Console.WriteLine(_rutaBaseImagenesCartas);
            Console.WriteLine("Ruta base imágenes para la web: /" + _rutaBaseImagenes);
        }

        // Runs once when the class is first used
        static Baraja()
        {

            CartasJuego = new List<CartaJuego>();
            CartasPremio = new List<CartaPremio>();
            CartasCastigo = new List<CartaCastigo>();
        }

        // Load cards from JSON file
        public static void CargarCartas()
        {
            // Check if the file exists
            if (!File.Exists(_rutaArchivoCartas))
            {
                throw new Exception("Error: No se encontró el archivo cartas.json");
            }

            CartasJuego.Clear(); CartasPremio.Clear(); CartasCastigo.Clear();

            try
            {
                // Read the JSON file
                string json = File.ReadAllText(_rutaArchivoCartas);

                // Turn JSON into a list of dictionaries
                var cartas = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

                // If no cards, show error
                if (cartas == null || cartas.Count == 0)
                {
                    throw new Exception("Error: El archivo JSON está vacío");
                }

                // Go through each card
                foreach (var carta in cartas)
                {
                    // Get basic fields (use empty string if missing)
                    string tipo = carta.ContainsKey("Tipo") ? carta["Tipo"].ToLower() : "";
                    string nombre = carta.ContainsKey("Nombre") ? carta["Nombre"] : "";
                    string descripcion = carta.ContainsKey("Descripcion") ? carta["Descripcion"] : "";
                    string mitologia = carta.ContainsKey("Mitologia") ? carta["Mitologia"] : "";
                    string nombreArchivoImagen = carta.ContainsKey("ArchivoImagen") ? carta["ArchivoImagen"] : "";
                    string imagenUrl = "";

                    // Check if Nombre or Tipo are missing
                    if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(tipo))
                    {
                        Console.WriteLine($"Advertencia: Carta sin Nombre o Tipo, se ignora");
                        continue;
                    }

                    if (!string.IsNullOrEmpty(nombreArchivoImagen))
                    {
                        // Construimos la URL que el navegador entenderá
                        //imagenUrl = _rutaBaseImagenesCartas + nombreArchivoImagen.Replace("\\", "/"); 
                        imagenUrl = _rutaBaseImagenes + nombreArchivoImagen;
                    }

                    // Handle different card types
                    if (tipo == "juego")
                    {
                        // Get Rareza
                        string rareza = carta.ContainsKey("Rareza") ? carta["Rareza"] : "";
                        if (string.IsNullOrEmpty(rareza) || !Enum.TryParse<CartaJuego.Rareza>(rareza, true, out var rarezaEnum))
                        {
                            Console.WriteLine($"Advertencia: Carta '{nombre}' no tiene Rareza válida, se ignora");
                            continue;
                        }
                        CartasJuego.Add(new CartaJuego(nombre, descripcion, mitologia, rarezaEnum, imagenUrl));
                    }
                    else if (tipo == "premio")
                    {
                        // Get Bendicion
                        string bendicion = carta.ContainsKey("Bendicion") ? carta["Bendicion"] : "";
                        if (string.IsNullOrEmpty(bendicion))
                        {
                            Console.WriteLine($"Advertencia: Carta '{nombre}' no tiene Bendición, se ignora");
                            continue;
                        }
                        CartasPremio.Add(new CartaPremio(nombre, descripcion, mitologia, bendicion, imagenUrl));
                    }
                    else if (tipo == "castigo")
                    {
                        // Get Maleficio
                        string maleficio = carta.ContainsKey("Maleficio") ? carta["Maleficio"] : "";
                        if (string.IsNullOrEmpty(maleficio))
                        {   
                            Console.WriteLine($"Advertencia: Carta '{nombre}' no tiene Maleficio, se ignora");
                            continue;
                        }
                        CartasCastigo.Add(new CartaCastigo(nombre, descripcion, mitologia, maleficio, imagenUrl));
                    }
                    else
                    {
                        Console.WriteLine($"Advertencia: Tipo de carta '{tipo}' desconocido para '{nombre}', se ignora");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al leer el archivo JSON: {ex.Message}");
            }
        }
    }
}