[![Typing SVG](https://readme-typing-svg.demolab.com?font=Alfa+Slab+One&size=30&pause=1000&color=98F7D0&width=435&lines=Proyecto+Final+Paradigmas)](https://git.io/typing-svg)
## 👤 **Integrantes**

* `Santiago Hernández Morantes`
* `Juan José Mesa Cardona`

---
## 📔 Enunciado **Ejercicio 7**

Diseñe un diagrama UML e Implemente las siguientes clases en C# que se tienen en un juego de cartas: cartas, jugadores, resto.

Las **Carta** son de 3 tipos: **juego**, **premio** y **castigo**. Las cartas premio y castigo modifican puntos al jugador, pero esta modificación es distinta para cada carta. Una carta de premio da 5 puntos y una de castigo quita cinco puntos.

Usted le pone los atributos a la carta (Nombre, Descripción, Mitología) de acuerdo con su juego favorito. Las cartas de juego son las que se le entregan al jugador, las cartas de premio y castigo (usted pone un atributo distintivo a cada una): (Juego: Rareza , Premio: Bendición , Castigo: Maldición) **tienen dos listas aparte**.

La clase **jugador** tiene los siguientes atributos:  un Nickname, los puntos y la lista de cartas que le tocan en cada juego. Cada que inicia un juego al jugador se le refresca esta lista con cartas nuevas.  Cuando comienzan el juego cada jugador debe iniciar con un número de puntos entre 50 y 80, que equivale a su apuesta inicial.

La clase **resto**, contiene la lista de cartas que sobraron y que se usa para que los jugadores recojan durante el juego. 
El **juego** tiene todas las listas de cartas.  Cada jugador que gana un juego acumula 20 puntos.

**Eventos:**
- Un evento informará cuando se agote las cartas de premio y de castigo
- Un evento informa cuando se agotan las cartas del resto
- Finalmente, un evento informará el cambio en el usuario que va ganando el juego

---
## ♠️ **Distribución de la baraja mitológica** 

- **30 cartas de Juego           |             Probabilidad: 0.6             60%**

- **12 cartas de Castigo         |             Probabilidad: 0.24           24%**

 - **8   cartas de Premio        |             Probabilidad: 0.16            16%**

### 🃏🎮 **Cartas de Juego (30)**

| **Nombre** | **Descripción**                              | **Mitología** | **Rareza** |
| ---------- | -------------------------------------------- | ------------- | ---------- |
| Atenea     | Diosa de la sabiduría y la estrategia        | Griega        | Especial   |
| Zeus       | Rey de los dioses, domina el trueno          | Griega        | Legendario |
| Thor       | Guerrero del trueno, hijo de Odín            | Nórdica       | Épico      |
| Anubis     | Protector del inframundo, guía de almas      | Egipcia       | Raro       |
| Loki       | Embaucador impredecible de múltiples rostros | Nórdica       | Especial   |
| Hades      | Señor del inframundo, silencioso y justo     | Griega        | Raro       |
| Freya      | Diosa del amor y la guerra, feroz y bella    | Nórdica       | Épico      |
| Ra         | Dios del Sol, fuente de vida y poder         | Egipcia       | Legendario |
| Ares       | Dios de la guerra, sediento de combate       | Griega        | Especial   |
| Apolo      | Dios de la música, medicina y luz            | Griega        | Especial   |
| Bastet     | Diosa felina, protectora del hogar           | Egipcia       | Común      |
| Hermes     | Mensajero veloz, guía de viajeros            | Griega        | Común      |
| Fenrir     | Lobo del Ragnarok, bestia indomable          | Nórdica       | Raro       |
| Poseidón   | Dios del mar, impredecible y fuerte          | Griega        | Épico      |
| Ishtar     | Diosa del amor y guerra, bella y mortal      | Mesopotámica  | Especial   |
| Osiris     | Señor de la resurrección, justo juez         | Egipcia       | Raro       |
| Tyr        | Dios de la guerra y el sacrificio            | Nórdica       | Común      |
| Hel        | Reina de los muertos, gélida presencia       | Nórdica       | Común      |
| Artemis    | Cazadora infalible, guardiana del bosque     | Griega        | Especial   |
| Horus      | Dios halcón, símbolo de protección real      | Egipcia       | Especial   |
| Cronos     | Titán del tiempo, implacable y eterno        | Griega        | Épico      |
| Persefone  | Reina del inframundo, entre dos mundos       | Griega        | Especial   |
| Marduk     | Guerrero de los dioses, mata al caos         | Mesopotámica  | Raro       |
| Nut        | Diosa del cielo estrellado, madre protectora | Egipcia       | Común      |
| Pan        | Espíritu libre del bosque y la música        | Griega        | Común      |
| Eros       | Dios del deseo y el amor irresistible        | Griega        | Común      |
| Set        | Caótico dios del desierto y la destrucción   | Egipcia       | Especial   |
| Odín       | Padre sabio, busca poder y conocimiento      | Nórdica       | Legendario |
| Enki       | Señor del agua dulce y la sabiduría          | Sumeria       | Raro       |
| Chac       | Dios de la lluvia, impredecible y fuerte     | Maya          | Común      |

### 🃏🥊 **Cartas de Castigo (12)**

| **Nombre**     | **Descripción**                              | **Mitología** | **Maldición**                        |
| -------------- | -------------------------------------------- | ------------- | ------------------------------------ |
| Medusa         | Su mirada convierte en piedra                | Griega        | Piedra eterna para quien la enfrente |
| Quimera        | Monstruo con fuego y ferocidad incontrolable | Griega        | Quema todo a su paso                 |
| Cerbero        | Guardián infernal de tres cabezas            | Griega        | No permite el regreso del inframundo |
| Hidra de Lerna | Cada cabeza cortada da lugar a dos más       | Griega        | Multiplica el sufrimiento            |
| Tifón          | Criatura colosal de fuego y tormenta         | Griega        | Trae caos donde pisa                 |
| Apofis         | Serpiente del caos, opuesto al orden         | Egipcia       | Engulle el sol y el equilibrio       |
| Jörmungandr    | Serpiente marina que rodea el mundo          | Nórdica       | Inicia el fin con su mordida         |
| Lamia          | Monstruo devorador de niños                  | Griega        | Acecha en sueños inocentes           |
| Echidna        | Madre de monstruos, salvaje y astuta         | Griega        | Engendra pesadillas vivas            |
| Hécate         | Hechicera oscura, guía entre sombras         | Griega        | Oscurece todo destino                |
| Garm           | Perro infernal del fin del mundo             | Nórdica       | Aúlla anunciando la perdición        |
| Lilith         | Espíritu nocturno y destructor               | Mesopotámica  | Robará tu aliento en la noche        |

### 🃏🕊️ **Cartas de Premio (8)**

| Nombre  | Descripción                                        | Mitología | Bendición                        |
| ------- | -------------------------------------------------- | --------- | -------------------------------- |
| Iris    | Mensajera del arcoíris, portadora de buenas nuevas | Griega    | Trae alegría entre nubes oscuras |
| Brigid  | Diosa de la sanación, inspiración y fertilidad     | Celta     | Florecen los campos a su paso    |
| Hathor  | Diosa del amor, la alegría y la música             | Egipcia   | Su canto sana el alma            |
| Eir     | Sanadora divina de los dioses                      | Nórdica   | Sus manos tejen la salud         |
| Fortuna | Dadora del destino y la buena suerte               | Romana    | Todo gira a tu favor             |
| Deméter | Diosa de la cosecha y la abundancia                | Griega    | Todo lo que tocas florece        |
| Inanna  | Diosa del amor, justicia y poder                   | Sumeria   | Hace que reine la armonía        |
| Gaia    | Madre Tierra, fuente de toda vida                  | Griega    | Su abrazo nutre la existencia    |

---
## 📝 Changelog

  [**📝 Ver ChangeLog**](./ChangeLog.md)
