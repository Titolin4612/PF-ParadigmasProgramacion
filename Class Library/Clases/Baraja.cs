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

        public static void Ruta()
        {
            Console.WriteLine(_rutaArchivoCartas);
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

                    // Check if Nombre or Tipo are missing
                    if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(tipo))
                    {
                        Console.WriteLine($"Advertencia: Carta sin Nombre o Tipo, se ignora");
                        continue;
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
                        CartasJuego.Add(new CartaJuego(nombre, descripcion, mitologia, rarezaEnum));
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
                        CartasPremio.Add(new CartaPremio(nombre, descripcion, mitologia, bendicion));
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
                        CartasCastigo.Add(new CartaCastigo(nombre, descripcion, mitologia, maleficio));
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

        // Unchanged ImprimirCartasCargadas method
        public static void ImprimirCartasCargadas()
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("       CARTAS CARGADAS EN BARAJA");
            Console.WriteLine("=========================================");

            // --- Imprimir Cartas de Juego ---
            Console.WriteLine("\n--- CARTAS DE JUEGO ---");
            if (!CartasJuego.Any()) // Comprueba si la lista está vacía usando Linq
            {
                Console.WriteLine("  (No hay cartas de juego cargadas)");
            }
            else
            {
                foreach (var carta in CartasJuego)
                {
                    Console.WriteLine($"  ---------------------------------");
                    Console.WriteLine($"  Nombre:      {carta.Nombre}");
                    Console.WriteLine($"  Mitología:   {carta.Mitologia}");
                    Console.WriteLine($"  Rareza:      {carta.RarezaCarta}");
                    Console.WriteLine($"  Puntos:      {carta.ObtenerPuntos()}"); // Muestra los puntos
                    Console.WriteLine($"  Descripción: {carta.Descripcion}");
                }
                Console.WriteLine($"  ---------------------------------");
            }

            // --- Imprimir Cartas de Premio ---
            Console.WriteLine("\n--- CARTAS DE PREMIO ---");
            if (!CartasPremio.Any())
            {
                Console.WriteLine("  (No hay cartas de premio cargadas)");
            }
            else
            {
                foreach (var carta in CartasPremio)
                {
                    Console.WriteLine($"  ---------------------------------");
                    Console.WriteLine($"  Nombre:      {carta.Nombre}");
                    Console.WriteLine($"  Mitología:   {carta.Mitologia}");
                    Console.WriteLine($"  Puntos:      {carta.ObtenerPuntos()}"); // Muestra los puntos (+5)
                    Console.WriteLine($"  Bendición:   {carta.Bendicion}");
                    Console.WriteLine($"  Descripción: {carta.Descripcion}");
                }
                Console.WriteLine($"  ---------------------------------");
            }

            // --- Imprimir Cartas de Castigo ---
            Console.WriteLine("\n--- CARTAS DE CASTIGO ---");
            if (!CartasCastigo.Any())
            {
                Console.WriteLine("  (No hay cartas de castigo cargadas)");
            }
            else
            {
                foreach (var carta in CartasCastigo)
                {
                    Console.WriteLine($"  ---------------------------------");
                    Console.WriteLine($"  Nombre:      {carta.Nombre}");
                    Console.WriteLine($"  Mitología:   {carta.Mitologia}");
                    Console.WriteLine($"  Puntos:      {carta.ObtenerPuntos()}"); // Muestra los puntos (-5)
                    Console.WriteLine($"  Maleficio:   {carta.Maleficio}");
                    Console.WriteLine($"  Descripción: {carta.Descripcion}");
                }
                Console.WriteLine($"  ---------------------------------");
            }

            // --- Imprimir Resumen ---
            Console.WriteLine("\n=========================================");
            Console.WriteLine("             RESUMEN CARGA");
            Console.WriteLine("=========================================");
            Console.WriteLine($"Total Cartas Juego:    {CartasJuego.Count}");
            Console.WriteLine($"Total Cartas Premio:   {CartasPremio.Count}");
            Console.WriteLine($"Total Cartas Castigo:  {CartasCastigo.Count}");
            Console.WriteLine("=========================================");
        }
    }
}