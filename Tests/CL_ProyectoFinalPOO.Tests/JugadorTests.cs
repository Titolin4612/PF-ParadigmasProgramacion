using CL_ProyectoFinalPOO.Clases;
using Xunit;

namespace CL_ProyectoFinalPOO.Tests
{
    public class JugadorTests
    {
        private Juego CrearJuegoConDatos()
        {
            var juego = new Juego();
            return juego;
        }

        [Fact]
        public void Constructor_NicknameValido_DebeCrearJugador()
        {
            var juego = CrearJuegoConDatos();
            var jugador = new Jugador("Juan", 100, juego);

            Assert.Equal("Juan", jugador.Nickname);
            Assert.Equal(100, jugador.ApuestaInicial);
        }

        [Theory]
        [InlineData("abc")]     // menos de 4 caracteres
        [InlineData("ab")]      // 2 caracteres
        [InlineData("a")]       // 1 caracter
        public void Nickname_MenosDe4Caracteres_DebeLanzarExcepcion(string nicknameInvalido)
        {
            var juego = CrearJuegoConDatos();
            Assert.Throws<Exception>(() => new Jugador(nicknameInvalido, 100, juego));
        }

        [Fact]
        public void Nickname_Nulo_DebeLanzarExcepcion()
        {
            var juego = CrearJuegoConDatos();
            Assert.Throws<Exception>(() => new Jugador(null!, 100, juego));
        }

        [Fact]
        public void Nickname_Vacio_DebeLanzarExcepcion()
        {
            var juego = CrearJuegoConDatos();
            Assert.Throws<Exception>(() => new Jugador("", 100, juego));
        }

        [Fact]
        public void Nickname_SoloEspacios_DebeLanzarExcepcion()
        {
            var juego = CrearJuegoConDatos();
            Assert.Throws<Exception>(() => new Jugador("   ", 100, juego));
        }

        [Theory]
        [InlineData("Juan")]
        [InlineData("Juan1")]
        [InlineData("Julian1234567890")] // muy largo pero valido
        public void Nickname_4OMasCaracteres_DebeSerValido(string nicknameValido)
        {
            var juego = CrearJuegoConDatos();
            var jugador = new Jugador(nicknameValido, 100, juego);
            Assert.Equal(nicknameValido, jugador.Nickname);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        public void ApuestaInicial_ValorValido_DebeAceptar(int apuestaValida)
        {
            var juego = CrearJuegoConDatos();
            var jugador = new Jugador("TestUser", apuestaValida, juego);
            Assert.Equal(apuestaValida, jugador.ApuestaInicial);
        }

        [Theory]
        [InlineData(9)]    // menos de min
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1001)] // mayor que max
        public void ApuestaInicial_ValorInvalido_DebeLanzarExcepcion(int apuestaInvalida)
        {
            var juego = CrearJuegoConDatos();
            Assert.Throws<Exception>(() => new Jugador("TestUser", apuestaInvalida, juego));
        }

        [Fact]
        public void Puntos_InicialSegunApuesta_DebeEstarEntre50y80()
        {
            var juego = CrearJuegoConDatos();

            var jugador1 = new Jugador("Jug1", 50, juego);
            Assert.InRange(jugador1.Puntos, 50, 80);

            var jugador2 = new Jugador("Jug2", 150, juego);
            Assert.InRange(jugador2.Puntos, 50, 80);

            var jugador3 = new Jugador("Jug3", 400, juego);
            Assert.InRange(jugador3.Puntos, 50, 80);
        }

        [Fact]
        public void L_cartas_jugador_Inicial_DebeEstarVacia()
        {
            var juego = CrearJuegoConDatos();
            var jugador = new Jugador("TestUser", 100, juego);

            Assert.NotNull(jugador.L_cartas_jugador);
            Assert.Empty(jugador.L_cartas_jugador);
        }

        [Fact]
        public void Perdio_Inicial_DebeSerFalse()
        {
            var juego = CrearJuegoConDatos();
            var jugador = new Jugador("TestUser", 100, juego);
            Assert.False(jugador.Perdio);
        }

        [Fact]
        public void CogerCarta_SinJuegoActivo_DebeLanzarExcepcion()
        {
            var juegoVacio = new Juego();
            var jugador = new Jugador("TestUser", 100, juegoVacio);

            // El juego no tiene jugadores configurados
            Assert.Throws<Exception>(() => jugador.CogerCarta());
        }
    }
}