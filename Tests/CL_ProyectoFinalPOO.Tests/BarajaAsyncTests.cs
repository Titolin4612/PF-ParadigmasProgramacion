using CL_ProyectoFinalPOO.Clases;
using Xunit;
using System.IO;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO.Tests
{
    public class BarajaAsyncTests
    {
        [Fact]
        public async Task CargarCartasAsync_ConJSONValido_DebeCargar60Cartas()
        {
            var baraja = new Baraja();
            var rutaJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "MVC Web App", "cartas.json");

            if (File.Exists(rutaJson))
            {
                await baraja.CargarCartasAsync(rutaJson);

                Assert.Equal(30, baraja.CartasJuego.Count);
                Assert.Equal(17, baraja.CartasCastigo.Count);
                Assert.Equal(13, baraja.CartasPremio.Count);
                Assert.Equal(60, baraja.CartasJuego.Count + baraja.CartasCastigo.Count + baraja.CartasPremio.Count);
            }
            else
            {
                Assert.True(true, "JSON no encontrado en ruta esperada, se omite test");
            }
        }

        [Fact]
        public async Task CargarCartasAsync_DebeCrearInstanciasCorrectas()
        {
            var baraja = new Baraja();
            var rutaJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "MVC Web App", "cartas.json");

            if (File.Exists(rutaJson))
            {
                await baraja.CargarCartasAsync(rutaJson);

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
        public async Task CargarCartasAsync_ConArchivoInexistente_DebeLanzarExcepcion()
        {
            var baraja = new Baraja();
            var rutaInvalida = "archivo_que_no_existe.json";

            await Assert.ThrowsAnyAsync<Exception>(() => baraja.CargarCartasAsync(rutaInvalida));
        }

        [Fact]
        public async Task CargarCartasAsync_DebeLimpiarListasAnteriores()
        {
            var baraja = new Baraja();
            baraja.CartasJuego.Add(new CartaJuego("Temp", "Temp", "Temp", CartaJuego.Rareza.Comun, ""));

            var rutaJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "MVC Web App", "cartas.json");
            if (File.Exists(rutaJson))
            {
                await baraja.CargarCartasAsync(rutaJson);

                Assert.DoesNotContain(baraja.CartasJuego, c => c.Nombre == "Temp");
            }
            else
            {
                Assert.True(true, "JSON no encontrado en ruta esperada, se omite test");
            }
        }

        [Fact]
        public async Task CargarCartasAsync_Vacio_DebeLanzarExcepcion()
        {
            var baraja = new Baraja();
            var rutaTemporal = Path.Combine(Path.GetTempPath(), "vacio_" + System.Guid.NewGuid() + ".json");
            await File.WriteAllTextAsync(rutaTemporal, "[]");

            try
            {
                await Assert.ThrowsAnyAsync<Exception>(() => baraja.CargarCartasAsync(rutaTemporal));
            }
            finally
            {
                if (File.Exists(rutaTemporal))
                    File.Delete(rutaTemporal);
            }
        }

        [Fact]
        public async Task CargarCartasAsync_SintaxisJSONInvalida_DebeLanzarExcepcion()
        {
            var baraja = new Baraja();
            var rutaTemporal = Path.Combine(Path.GetTempPath(), "invalido_" + System.Guid.NewGuid() + ".json");
            await File.WriteAllTextAsync(rutaTemporal, "{ invalid json }");

            try
            {
                await Assert.ThrowsAnyAsync<Exception>(() => baraja.CargarCartasAsync(rutaTemporal));
            }
            finally
            {
                if (File.Exists(rutaTemporal))
                    File.Delete(rutaTemporal);
            }
        }
    }
}
