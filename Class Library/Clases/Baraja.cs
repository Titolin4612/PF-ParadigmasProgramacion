using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL_ProyectoFinalPOO.Interfaces;

namespace CL_ProyectoFinalPOO.Clases
{
    public class Baraja
    {
        public static List<CartaJuego> CrearCartasJuego()
        {
            try
            {
                var cartas = new List<CartaJuego>();

                // Cartas de Juego
                // COMUN (-2)
                cartas.Add(new CartaJuego("Hermes", "Mensajero de los dioses, veloz pero poco influyente", "Griega", CartaJuego.Rareza.Comun));
                cartas.Add(new CartaJuego("Pan", "Espíritu libre del bosque, más bromista que fuerte", "Griega", CartaJuego.Rareza.Comun));
                cartas.Add(new CartaJuego("Bastet", "Protectora del hogar, tierna pero discreta", "Egipcia", CartaJuego.Rareza.Comun));
                cartas.Add(new CartaJuego("Tyr", "Valiente, pero limitado tras perder su brazo", "Nórdica", CartaJuego.Rareza.Comun));
                cartas.Add(new CartaJuego("Nut", "Diosa madre del cielo, protectora pero pasiva", "Egipcia", CartaJuego.Rareza.Comun));
                cartas.Add(new CartaJuego("Chac", "Dios maya de la lluvia, algo temperamental", "Maya", CartaJuego.Rareza.Comun));
                cartas.Add(new CartaJuego("Hel", "Reina del inframundo, gélida pero inmóvil", "Nórdica", CartaJuego.Rareza.Comun));
                cartas.Add(new CartaJuego("Eros", "Dios del amor, poderoso pero emocionalmente volátil", "Griega", CartaJuego.Rareza.Comun));


                // ESPECIAL (-1)
                cartas.Add(new CartaJuego("Brigid", "Diosa celta del fuego y la poesía, inspiradora", "Celta", CartaJuego.Rareza.Especial));
                cartas.Add(new CartaJuego("Neit", "Guerrera egipcia, estratega, pero no muy influyente", "Egipcia", CartaJuego.Rareza.Especial));
                cartas.Add(new CartaJuego("Ixchel", "Diosa maya de la medicina y los partos", "Maya", CartaJuego.Rareza.Especial));
                cartas.Add(new CartaJuego("Epona", "Diosa celta de los caballos, símbolo de nobleza", "Celta", CartaJuego.Rareza.Especial));
                cartas.Add(new CartaJuego("Enki", "Sabio sumerio, menos activo en batalla", "Sumeria", CartaJuego.Rareza.Especial));
                cartas.Add(new CartaJuego("Hecate", "Diosa de la brujería y los portales, misteriosa", "Griega", CartaJuego.Rareza.Especial));


                // RARA (0)
                cartas.Add(new CartaJuego("Atenea", "Diosa de la sabiduría y la estrategia", "Griega", CartaJuego.Rareza.Rara));
                cartas.Add(new CartaJuego("Loki", "Embaucador impredecible de múltiples rostros", "Nórdica", CartaJuego.Rareza.Rara));
                cartas.Add(new CartaJuego("Anubis", "Protector de tumbas y guía de almas", "Egipcia", CartaJuego.Rareza.Rara));
                cartas.Add(new CartaJuego("Ishtar", "Diosa babilónica del amor y la guerra", "Mesopotámica", CartaJuego.Rareza.Rara));
                cartas.Add(new CartaJuego("Marduk", "Campeón babilónico contra el caos", "Mesopotámica", CartaJuego.Rareza.Rara));
                cartas.Add(new CartaJuego("Persefone", "Reina del inframundo, dulce y temida", "Griega", CartaJuego.Rareza.Rara));
                cartas.Add(new CartaJuego("Fenrir", "Lobo del fin del mundo, temido pero incontrolable", "Nórdica", CartaJuego.Rareza.Rara));
                cartas.Add(new CartaJuego("Horus", "Dios de la realeza y el cielo", "Egipcia", CartaJuego.Rareza.Rara));


                // EPICA (+1)
                cartas.Add(new CartaJuego("Thor", "Dios del trueno, valiente y devastador", "Nórdica", CartaJuego.Rareza.Epica));
                cartas.Add(new CartaJuego("Freya", "Diosa del amor y la guerra, feroz y hermosa", "Nórdica", CartaJuego.Rareza.Epica));
                cartas.Add(new CartaJuego("Poseidón", "Dios del mar, impredecible y poderoso", "Griega", CartaJuego.Rareza.Epica));
                cartas.Add(new CartaJuego("Cronos", "Titán del tiempo, inmutable e inevitable", "Griega", CartaJuego.Rareza.Epica));
                cartas.Add(new CartaJuego("Apolo", "Dios del sol, la medicina y la música", "Griega", CartaJuego.Rareza.Epica));

                // LEGENDARIA (+2)
                cartas.Add(new CartaJuego("Zeus", "Rey del Olimpo, maestro del rayo", "Griega", CartaJuego.Rareza.Legendaria));
                cartas.Add(new CartaJuego("Ra", "Dios solar egipcio, símbolo de poder supremo", "Egipcia", CartaJuego.Rareza.Legendaria));
                cartas.Add(new CartaJuego("Odín", "Padre sabio, busca poder y conocimiento", "Nórdica", CartaJuego.Rareza.Legendaria));

                return cartas;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error en el metodo CrearCartasJuego" + ex);
            }

            
        }

        public static List<CartaCastigo> CrearCartasCastigo()
        {
            try
            {
                var cartas = new List<CartaCastigo>();

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

                return cartas;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error en el metodo CrearCartasCastigo" + ex);
            }

        }

        public static List<CartaPremio> CrearCartasPremio() 
        {
            try
            {
                var cartas = new List<CartaPremio>();

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
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error en el metodo CrearCartasPremio" + ex);
            }

            
        }

        public static List<Carta> ObtenerBarajaCompleta()
        {
            try
            {
                var baraja = new List<Carta>();

                baraja.AddRange(CrearCartasJuego());

                baraja.AddRange(CrearCartasCastigo());

                baraja.AddRange(CrearCartasPremio());

                return baraja;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error en el metodo ObtenerBarajaCompleta" + ex);
            }
            
        }
    }
}