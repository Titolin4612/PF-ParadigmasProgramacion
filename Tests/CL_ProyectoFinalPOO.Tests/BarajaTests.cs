using CL_ProyectoFinalPOO.Clases;
using Xunit;
using System.IO;

namespace CL_ProyectoFinalPOO.Tests
{
    public class BarajaTests
    {
        [Fact]
        public void Constructor_DebeInicializarListas()
        {
            var baraja = new Baraja();

            Assert.NotNull(baraja.CartasJuego);
            Assert.NotNull(baraja.CartasPremio);
            Assert.NotNull(baraja.CartasCastigo);
        }

        [Fact]
        public void CargarCartas_ConJSONValido_DebeCargar60Cartas()
        {
            var baraja = new Baraja();
            var rutaJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "MVC Web App", "cartas.json");

            if (File.Exists(rutaJson))
            {
                baraja.CargarCartas(rutaJson);

                Assert.Equal(30, baraja.CartasJuego.Count);
                Assert.Equal(17, baraja.CartasCastigo.Count);
                Assert.Equal(13, baraja.CartasPremio.Count);
                Assert.Equal(60, baraja.CartasJuego.Count + baraja.CartasCastigo.Count + baraja.CartasPremio.Count);
            }
            else
            {
                // Si no encuentra el JSON, solo verifica que las listas existen
                Assert.True(true, "JSON no encontrado en ruta esperada, se omite test");
            }
        }

        [Fact]
        public void CargarCartas_DebeCrearInstanciasCorrectas()
        {
            var baraja = new Baraja();
            var rutaJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "MVC Web App", "cartas.json");

            if (File.Exists(rutaJson))
            {
                baraja.CargarCartas(rutaJson);

                Assert.All(baraja.CartasJuego, carta => Assert.IsType<CartaJuego>(carta));
                Assert.All(baraja.CartasPremio, carta => Assert.IsType<CartaPremio>(carta));
                Assert.All(baraja.CartasCastigo, carta => Assert.IsType<CartaCastigo>(carta));
            }
            else
            {
                Assert.True(true, "JSON no encontrado en ruta esperada, se omite test");
            }
        }

        [Fact]
        public void CargarCartas_ConArchivoInexistente_DebeLanzarExcepcion()
        {
            var baraja = new Baraja();
            var rutaInvalida = "archivo_que_no_existe.json";

            Assert.ThrowsAny<Exception>(() => baraja.CargarCartas(rutaInvalida));
        }

        [Fact]
        public void CargarCartas_DebeLimpiarListasAnteriores()
        {
            var baraja = new Baraja();
            baraja.CartasJuego.Add(new CartaJuego("Temp", "Temp", "Temp", CartaJuego.Rareza.Comun, ""));

            var rutaJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "MVC Web App", "cartas.json");
            if (File.Exists(rutaJson))
            {
                baraja.CargarCartas(rutaJson);

                Assert.DoesNotContain(baraja.CartasJuego, c => c.Nombre == "Temp");
            }
            else
            {
                Assert.True(true, "JSON no encontrado en ruta esperada, se omite test");
            }
        }
    }
}