using CL_ProyectoFinalPOO.Clases;
using Xunit;

namespace CL_ProyectoFinalPOO.Tests
{
    public class CartaJuegoTests
    {
        [Fact]
        public void ObtenerPuntos_Comun_DebeRetornarMenos2()
        {
            var carta = new CartaJuego("Test", "Desc", "Mitologia", CartaJuego.Rareza.Comun, "img.png");
            Assert.Equal(-2, carta.ObtenerPuntos());
        }

        [Fact]
        public void ObtenerPuntos_Especial_DebeRetornarMenosUno()
        {
            var carta = new CartaJuego("Test", "Desc", "Mitologia", CartaJuego.Rareza.Especial, "img.png");
            Assert.Equal(-1, carta.ObtenerPuntos());
        }

        [Fact]
        public void ObtenerPuntos_Rara_DebeRetornarCero()
        {
            var carta = new CartaJuego("Test", "Desc", "Mitologia", CartaJuego.Rareza.Rara, "img.png");
            Assert.Equal(0, carta.ObtenerPuntos());
        }

        [Fact]
        public void ObtenerPuntos_Epica_DebeRetornarUno()
        {
            var carta = new CartaJuego("Test", "Desc", "Mitologia", CartaJuego.Rareza.Epica, "img.png");
            Assert.Equal(1, carta.ObtenerPuntos());
        }

        [Fact]
        public void ObtenerPuntos_Legendaria_DebeRetornarDos()
        {
            var carta = new CartaJuego("Test", "Desc", "Mitologia", CartaJuego.Rareza.Legendaria, "img.png");
            Assert.Equal(2, carta.ObtenerPuntos());
        }

        [Fact]
        public void Constructor_DebeEstablecerRarezaCorrectamente()
        {
            var carta = new CartaJuego("Zeus", "Rey de los dioses", "Griega", CartaJuego.Rareza.Legendaria, "zeus.png");
            Assert.Equal(CartaJuego.Rareza.Legendaria, carta.RarezaCarta);
        }

        [Theory]
        [InlineData(CartaJuego.Rareza.Comun)]
        [InlineData(CartaJuego.Rareza.Especial)]
        [InlineData(CartaJuego.Rareza.Rara)]
        [InlineData(CartaJuego.Rareza.Epica)]
        [InlineData(CartaJuego.Rareza.Legendaria)]
        public void Rareza_TodasLasVariantes_DebenSerValidas(CartaJuego.Rareza rareza)
        {
            var carta = new CartaJuego("Test", "Desc", "Mitologia", rareza, "img.png");
            Assert.Equal(rareza, carta.RarezaCarta);
        }

        [Fact]
        public void Nombre_ValorNulo_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaJuego(null!, "Desc", "Mitologia", CartaJuego.Rareza.Comun, "img.png"));
        }

        [Fact]
        public void Nombre_ValorVacio_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaJuego("", "Desc", "Mitologia", CartaJuego.Rareza.Comun, "img.png"));
        }

        [Fact]
        public void Nombre_SoloEspacios_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaJuego("   ", "Desc", "Mitologia", CartaJuego.Rareza.Comun, "img.png"));
        }
    }
}