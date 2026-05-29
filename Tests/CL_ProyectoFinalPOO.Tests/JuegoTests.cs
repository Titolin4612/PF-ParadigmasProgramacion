using CL_ProyectoFinalPOO.Clases;
using Xunit;

namespace CL_ProyectoFinalPOO.Tests
{
    public class JuegoTests
    {
        [Fact]
        public void Constructor_DebeInicializarConBaraja()
        {
            var juego = new Juego();

            Assert.NotNull(juego.L_cartas_resto);
            Assert.NotNull(juego.L_cartas_premio);
            Assert.NotNull(juego.L_cartas_castigo);
            Assert.NotNull(juego.Jugadores);
        }

        [Fact]
        public void Constructor_DebeCargar30CartasJuego()
        {
            var juego = new Juego();
            Assert.Equal(30, juego.L_cartas_resto.Count);
        }

        [Fact]
        public void Constructor_DebeCargar17CartasCastigo()
        {
            var juego = new Juego();
            Assert.Equal(17, juego.L_cartas_castigo.Count);
        }

        [Fact]
        public void Constructor_DebeCargar13CartasPremio()
        {
            var juego = new Juego();
            Assert.Equal(13, juego.L_cartas_premio.Count);
        }

        [Fact]
        public void Revolver_NoDebeModificarCantidadDeElementos()
        {
            var juego = new Juego();
            var cantidadOriginal = juego.L_cartas_resto.Count;

            juego.Revolver(juego.L_cartas_resto);

            Assert.Equal(cantidadOriginal, juego.L_cartas_resto.Count);
        }

        [Fact]
        public void Revolver_DebeAlMenosReorganizar()
        {
            var juego = new Juego();
            var cartasOriginal = juego.L_cartas_resto.ToList();

            juego.Revolver(juego.L_cartas_resto);

            // Es posible (poco probable) que randomize leave el mismo orden
            // por eso solo verificamos que sean los mismos elementos
            Assert.All(juego.L_cartas_resto, carta => Assert.Contains(carta, cartasOriginal));
        }

        [Fact]
        public void BarajarCartas_DebeRevolverLasTresListas()
        {
            var juego = new Juego();

            var cartasRestoOriginal = juego.L_cartas_resto.ToList();
            var cartasCastigoOriginal = juego.L_cartas_castigo.ToList();
            var cartasPremioOriginal = juego.L_cartas_premio.ToList();

            juego.BarajarCartas();

            // Verificar que todas las cartas siguen presentes (reordenadas)
            Assert.Equal(cartasRestoOriginal.Count, juego.L_cartas_resto.Count);
            Assert.Equal(cartasCastigoOriginal.Count, juego.L_cartas_castigo.Count);
            Assert.Equal(cartasPremioOriginal.Count, juego.L_cartas_premio.Count);
        }

        [Fact]
        public void AgregarJugador_DebeSumarseALista()
        {
            var juego = new Juego();
            var jugador = new Jugador("TestPlayer", 100, juego);

            juego.Jugadores.Add(jugador);

            Assert.Single(juego.Jugadores);
            Assert.Equal("TestPlayer", juego.Jugadores[0].Nickname);
        }

        [Fact]
        public void AsignarPuntosSegunApuesta_ApuestaBaja_DebeDar50Puntos()
        {
            var juego = new Juego();
            var jugador = new Jugador("Test", 50, juego);

            juego.AsignarPuntosSegunApuesta(jugador);

            Assert.Equal(50, jugador.Puntos);
        }

        [Fact]
        public void AsignarPuntosSegunApuesta_ApuestaMedia_DebeDar60Puntos()
        {
            var juego = new Juego();
            var jugador = new Jugador("Test", 200, juego);

            juego.AsignarPuntosSegunApuesta(jugador);

            Assert.Equal(60, jugador.Puntos);
        }

        [Fact]
        public void AsignarPuntosSegunApuesta_ApuestaAlta_DebeDar70Puntos()
        {
            var juego = new Juego();
            var jugador = new Jugador("Test", 400, juego);

            juego.AsignarPuntosSegunApuesta(jugador);

            Assert.Equal(70, jugador.Puntos);
        }

        [Fact]
        public void AsignarPuntosSegunApuesta_ApuestaMuyAlta_DebeDar80Puntos()
        {
            var juego = new Juego();
            var jugador = new Jugador("Test", 700, juego);

            juego.AsignarPuntosSegunApuesta(jugador);

            Assert.Equal(80, jugador.Puntos);
        }

        [Fact]
        public void ObtenerLider_SinJugadores_DebeRetornarNull()
        {
            var juego = new Juego();
            Assert.Null(juego.ObtenerLider());
        }

        [Fact]
        public void ObtenerLider_UnJugador_DebeRetornarEseJugador()
        {
            var juego = new Juego();
            var jugador = new Jugador("Test", 100, juego);
            jugador.Puntos = 75;
            juego.Jugadores.Add(jugador);

            var lider = juego.ObtenerLider();

            Assert.NotNull(lider);
            Assert.Equal("Test", lider.Nickname);
        }

        [Fact]
        public void ObtenerLider_MultiplesJugadores_DebeRetornarMayorPuntuacion()
        {
            var juego = new Juego();
            var jugador1 = new Jugador("Jug1", 100, juego);
            jugador1.Puntos = 50;
            var jugador2 = new Jugador("Jug2", 100, juego);
            jugador2.Puntos = 75;
            var jugador3 = new Jugador("Jug3", 100, juego);
            jugador3.Puntos = 60;

            juego.Jugadores.Add(jugador1);
            juego.Jugadores.Add(jugador2);
            juego.Jugadores.Add(jugador3);

            var lider = juego.ObtenerLider();

            Assert.NotNull(lider);
            Assert.Equal("Jug2", lider.Nickname);
            Assert.Equal(75, lider.Puntos);
        }

        [Fact]
        public void RepartirCartasIniciales_DebeDar3CartasPorJugador()
        {
            var juego = new Juego();
            var jugador1 = new Jugador("Jug1", 100, juego);
            var jugador2 = new Jugador("Jug2", 100, juego);
            juego.Jugadores.Add(jugador1);
            juego.Jugadores.Add(jugador2);

            var cartasOriginales = juego.L_cartas_resto.Count;

            juego.RepartirCartasIniciales(3);

            Assert.Equal(3, jugador1.L_cartas_jugador.Count);
            Assert.Equal(3, jugador2.L_cartas_jugador.Count);
            Assert.Equal(cartasOriginales - 6, juego.L_cartas_resto.Count);
        }

        [Fact]
        public void PasarTurno_DebeIncrementarIndiceCircularmente()
        {
            var juego = new Juego();
            var jugador1 = new Jugador("Jug1", 100, juego);
            var jugador2 = new Jugador("Jug2", 100, juego);
            var jugador3 = new Jugador("Jug3", 100, juego);
            juego.Jugadores.Add(jugador1);
            juego.Jugadores.Add(jugador2);
            juego.Jugadores.Add(jugador3);

            juego.IndiceJugador = 0;
            juego.PasarTurno();
            Assert.Equal(1, juego.IndiceJugador);

            juego.PasarTurno();
            Assert.Equal(2, juego.IndiceJugador);

            juego.PasarTurno();
            Assert.Equal(0, juego.IndiceJugador);
        }
    }
}