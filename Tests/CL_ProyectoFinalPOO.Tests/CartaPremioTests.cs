using CL_ProyectoFinalPOO.Clases;
using Xunit;

namespace CL_ProyectoFinalPOO.Tests
{
    public class CartaPremioTests
    {
        [Fact]
        public void ObtenerPuntos_DebeRetornar5()
        {
            var carta = new CartaPremio("Iris", "Mensajera del arcoíris", "Griega", "Trae alegría", "img.png");
            Assert.Equal(5, carta.ObtenerPuntos());
        }

        [Fact]
        public void VPremio_ValorEstatico_DebeSer5()
        {
            Assert.Equal(5, CartaPremio.VPremio);
        }

        [Fact]
        public void Constructor_DebeEstablecerBendicion()
        {
            var carta = new CartaPremio("Fortuna", "Dadora de suerte", "Romana", "Todo gira a tu favor", "fortuna.png");
            Assert.Equal("Todo gira a tu favor", carta.Bendicion);
        }

        [Fact]
        public void Bendicion_ValorNulo_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaPremio("Test", "Desc", "Mitologia", null!, "img.png"));
        }

        [Fact]
        public void Bendicion_ValorVacio_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaPremio("Test", "Desc", "Mitologia", "", "img.png"));
        }

        [Fact]
        public void Bendicion_SoloEspacios_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaPremio("Test", "Desc", "Mitologia", "   ", "img.png"));
        }

        [Fact]
        public void Nombre_ValorNulo_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaPremio(null!, "Desc", "Mitologia", "Bless", "img.png"));
        }

        [Fact]
        public void Mitologia_ValorNulo_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaPremio("Test", "Desc", null!, "Bless", "img.png"));
        }

        [Fact]
        public void Descripcion_ValorNulo_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaPremio("Test", null!, "Mitologia", "Bless", "img.png"));
        }
    }
}