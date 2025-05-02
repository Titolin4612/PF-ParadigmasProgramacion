using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL_ProyectoFinalPOO
{
    internal class Baraja
    {
        public static List<Carta> CrearCartas()
        {
            var cartas = new List<Carta>();

            // Cartas de Juego
            cartas.Add(new CartaJuego("Atenea", "Diosa de la sabiduría y la estrategia", "Griega", "Especial"));
            cartas.Add(new CartaJuego("Zeus", "Rey de los dioses, domina el trueno", "Griega", "Legendario"));
            cartas.Add(new CartaJuego("Thor", "Guerrero del trueno, hijo de Odín", "Nórdica", "Epico"));
            cartas.Add(new CartaJuego("Anubis", "Protector del inframundo, guía de almas", "Egipcia", "Raro"));
            cartas.Add(new CartaJuego("Loki", "Embaucador impredecible de múltiples rostros", "Nórdica", "Especial"));
            cartas.Add(new CartaJuego("Hades", "Señor del inframundo, silencioso y justo", "Griega", "Raro"));
            cartas.Add(new CartaJuego("Freya", "Diosa del amor y la guerra, feroz y bella", "Nórdica", "Epico"));
            cartas.Add(new CartaJuego("Ra", "Dios del Sol, fuente de vida y poder", "Egipcia", "Legendario"));
            cartas.Add(new CartaJuego("Ares", "Dios de la guerra, sediento de combate", "Griega", "Especial"));
            cartas.Add(new CartaJuego("Apolo", "Dios de la música, medicina y luz", "Griega", "Especial"));
            cartas.Add(new CartaJuego("Bastet", "Diosa felina, protectora del hogar", "Egipcia", "Comun"));
            cartas.Add(new CartaJuego("Hermes", "Mensajero veloz, guía de viajeros", "Griega", "Comun"));
            cartas.Add(new CartaJuego("Fenrir", "Lobo del Ragnarok, bestia indomable", "Nórdica", "Raro"));
            cartas.Add(new CartaJuego("Poseidón", "Dios del mar, impredecible y fuerte", "Griega", "Epico"));
            cartas.Add(new CartaJuego("Ishtar", "Diosa del amor y guerra, bella y mortal", "Mesopotámica", "Especial"));
            cartas.Add(new CartaJuego("Osiris", "Señor de la resurrección, justo juez", "Egipcia", "Raro"));
            cartas.Add(new CartaJuego("Tyr", "Dios de la guerra y el sacrificio", "Nórdica", "Comun"));
            cartas.Add(new CartaJuego("Hel", "Reina de los muertos, gélida presencia", "Nórdica", "Comun"));
            cartas.Add(new CartaJuego("Artemis", "Cazadora infalible, guardiana del bosque", "Griega", "Especial"));
            cartas.Add(new CartaJuego("Horus", "Dios halcón, símbolo de protección real", "Egipcia", "Especial"));
            cartas.Add(new CartaJuego("Cronos", "Titán del tiempo, implacable y eterno", "Griega", "Epico"));
            cartas.Add(new CartaJuego("Persefone", "Reina del inframundo, entre dos mundos", "Griega", "Especial"));
            cartas.Add(new CartaJuego("Marduk", "Guerrero de los dioses, mata al caos", "Mesopotámica", "Raro"));
            cartas.Add(new CartaJuego("Nut", "Diosa del cielo estrellado, madre protectora", "Egipcia", "Comun"));
            cartas.Add(new CartaJuego("Pan", "Espíritu libre del bosque y la música", "Griega", "Comun"));
            cartas.Add(new CartaJuego("Eros", "Dios del deseo y el amor irresistible", "Griega", "Comun"));
            cartas.Add(new CartaJuego("Set", "Caótico dios del desierto y la destrucción", "Egipcia", "Especial"));
            cartas.Add(new CartaJuego("Odín", "Padre sabio, busca poder y conocimiento", "Nórdica", "Legendario"));
            cartas.Add(new CartaJuego("Enki", "Señor del agua dulce y la sabiduría", "Sumeria", "Raro"));
            cartas.Add(new CartaJuego("Chac", "Dios de la lluvia, impredecible y fuerte", "Maya", "Comun"));

            // Cartas de Castigo
            cartas.Add(new CartaCastigo("Medusa", "Su mirada convierte en piedra", "Griega", "Piedra eterna para quien la enfrente"));
            cartas.Add(new CartaCastigo("Quimera", "Monstruo con fuego y ferocidad incontrolable", "Griega", "Quema todo a su paso"));
            cartas.Add(new CartaCastigo("Cerbero", "Guardián infernal de tres cabezas", "Griega", "No permite el regreso del inframundo"));
            cartas.Add(new CartaCastigo("Hidra de Lerna", "Cada cabeza cortada da lugar a dos más", "Griega", "Multiplica el sufrimiento"));
            cartas.Add(new CartaCastigo("Tifón", "Criatura colosal de fuego y tormenta", "Griega", "Trae caos donde pisa"));
            cartas.Add(new CartaCastigo("Apofis", "Serpiente del caos, opuesto al orden", "Egipcia", "Engulle el sol y el equilibrio"));
            cartas.Add(new CartaCastigo("Jörmungandr", "Serpiente marina que rodea el mundo", "Nórdica", "Inicia el fin con su mordida"));
            cartas.Add(new CartaCastigo("Lamia", "Monstruo devorador de niños", "Griega", "Acecha en sueños inocentes"));
            cartas.Add(new CartaCastigo("Echidna", "Madre de monstruos, salvaje y astuta", "Griega", "Engendra pesadillas vivas"));
            cartas.Add(new CartaCastigo("Hécate", "Hechicera oscura, guía entre sombras", "Griega", "Oscurece todo destino"));
            cartas.Add(new CartaCastigo("Garm", "Perro infernal del fin del mundo", "Nórdica", "Aúlla anunciando la perdición"));
            cartas.Add(new CartaCastigo("Lilith", "Espíritu nocturno y destructor", "Mesopotámica", "Robará tu aliento en la noche"));

            // Cartas de Premio
            cartas.Add(new CartaPremio("Iris", "Mensajera del arcoíris, portadora de buenas nuevas", "Griega", "Trae alegría entre nubes oscuras"));
            cartas.Add(new CartaPremio("Brigid", "Diosa de la sanación, inspiración y fertilidad", "Celta", "Florecen los campos a su paso"));
            cartas.Add(new CartaPremio("Hathor", "Diosa del amor, la alegría y la música", "Egipcia", "Su canto sana el alma"));
            cartas.Add(new CartaPremio("Eir", "Sanadora divina de los dioses", "Nórdica", "Sus manos tejen la salud"));
            cartas.Add(new CartaPremio("Fortuna", "Dadora del destino y la buena suerte", "Romana", "Todo gira a tu favor"));
            cartas.Add(new CartaPremio("Deméter", "Diosa de la cosecha y la abundancia", "Griega", "Todo lo que tocas florece"));
            cartas.Add(new CartaPremio("Inanna", "Diosa del amor, justicia y poder", "Sumeria", "Hace que reine la armonía"));
            cartas.Add(new CartaPremio("Gaia", "Madre Tierra, fuente de toda vida", "Griega", "Su abrazo nutre la existencia"));

            return cartas;
        }
    }
}
