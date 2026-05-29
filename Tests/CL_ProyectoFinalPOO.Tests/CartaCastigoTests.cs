using CL_ProyectoFinalPOO.Clases;
using Xunit;

namespace CL_ProyectoFinalPOO.Tests
{
    public class CartaCastigoTests
    {
        [Fact]
        public void ObtenerPuntos_DebeRetornarMenos5()
        {
            var carta = new CartaCastigo("Medusa", "Su mirada convierte en piedra", "Griega", "Piedra eterna", "img.png");
            Assert.Equal(-5, carta.ObtenerPuntos());
        }

        [Fact]
        public void VCastigo_ValorEstatico_DebeSerMenos5()
        {
            Assert.Equal(-5, CartaCastigo.VCastigo);
        }

        [Fact]
        public void Constructor_DebeEstablecerMaleficio()
        {
            var carta = new CartaCastigo("Cerbero", "Guardián de tres cabezas", "Griega", "No permite el regreso", "cerbero.png");
            Assert.Equal("No permite el regreso", carta.Maleficio);
        }

        [Fact]
        public void Maleficio_ValorNulo_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaCastigo("Test", "Desc", "Mitologia", null!, "img.png"));
        }

        [Fact]
        public void Maleficio_ValorVacio_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaCastigo("Test", "Desc", "Mitologia", "", "img.png"));
        }

        [Fact]
        public void Maleficio_SoloEspacios_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaCastigo("Test", "Desc", "Mitologia", "   ", "img.png"));
        }

        [Fact]
        public void Nombre_ValorNulo_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaCastigo(null!, "Desc", "Mitologia", "Curse", "img.png"));
        }

        [Fact]
        public void Mitologia_ValorNulo_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaCastigo("Test", "Desc", null!, "Curse", "img.png"));
        }

        [Fact]
        public void Descripcion_ValorNulo_DebeLanzarExcepcion()
        {
            Assert.Throws<Exception>(() => new CartaCastigo("Test", null!, "Mitologia", "Curse", "img.png"));
        }
    }
}