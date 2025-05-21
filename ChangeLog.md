
# 📝 ChangeLog
## Juego V 1.30 (Mayo 7 2025) SANTIAGO H

- Cambie en la clase Baraja para manejar 3 listas diferentes dependiendo el tipo de carta (Juego, Castigo, premio)
	
- Ahora la clase Resto solo incluye las cartas de tipo Juego, las de castigo y premio se manejarán con listas aparte en la clase juego
	
- Cambie el método AplicarEfectoCartas() para que imprima mejor los puntos obtenidos en esa carta y los puntos totales del jugador
	
- Cree las listas l_cartas_premio y l_cartas_castigo en la Clase juego, estas se rellenan en el constructor con los nuevos métodos de la clase Baraja ( CrearCartasCastigo() y CrearCartasPremio() )
	
- Cambie el método BarajarCartas() para que ahora no solo revuelva las cartas de resto si no también las de premio y castigo
	
- Cambie el constructor de Resto para evitar quemar valores
	
- Cambie durísimo la clase baraja y le agregue otro método que obtiene una lista con todas las cartas, algo similar a como antes funcionaba el Resto
	
- Añadí nuevos y modifiqué métodos y listas en la clase Juego, también cambié el constructor
	
- El método ObtenerCarta() pasó de estar en resto a estar en juego ya que al cambiar el funcionamiento de Resto quedaba obsoleto el método
	
- Modifiqué el funcionamiento del método BarajarCartas()
	
- Cambie los accesores de Carta por unos “Menos redundantes y mejor estructurados” similares a los que se hicieron en el proyecto del Multiplex
	
- Eliminé la interfaz ICartaEfecto ya que no estaba programada y por el momento sobraba, el método ActualizarPuntos(); que estaba en cada carta lo eliminé ya que no tenía estructura ni funcionalidad, luego se estructura mejor cada interfaz y métodos que llevaran
	
- Transferí el método de AplicarEfectoCartas de Jugador a Juego para intentar tener todos los métodos que regulen el funcionamiento del juego en la clase Juego, para eso modifique el funcionamiento para que devuelva un Int con el efecto de la carta
	
- También cree nuevos atributos estáticos en cada carta para dejar ahí el valor que van a sumar o restar y evitar quemar valores en los métodos que usen esos valores
	
- Añadí el método para repartir las cartas iniciales en cada juego, el método simplemente saca las 3 primeras cartas de la baraja actual y las entrega a un jugador y así repite hasta terminar con los jugadores
	
- Añadí también un par de atributos de reglas de negocio en el juego como numero max de jugadores y cartas por jugador
	
- Cambie el accesor de puntos ya que estaba haciendo que nadie pudiera pasar de 80 puntos, esta mal, hay que hacer una validación diferente si queremos controlar que al INICIAR los puntos estén en ese rango
	
- Hice mucha corrección de errores, falta revisar mucho pero creo que se logro avanzar bastante hoy, faltaron cosas por mencionar aquí ya que cada que me iba acordando iba poniendo, algunas se me pasaron
--- 
##  Juego V 1.34 (Mayo 9 2025) SANTIAGO H

- Cree un par de interfaces con los métodos ya creados para ir definiendo mejor la biblioteca

- A TODOS los métodos le cree bloque Try Catch para mejor control de errores y mejorar el cumplimiento de requisitos

- Cree la carpeta de Eventos para encaminar el creado de los eventos 
--- 
## Juego V 1.40 (Mayo 17 2025) SANTIAGO H
* Empecé a implementar en el MVC, Fueron avances cortos pero significativos en la parte del front de el Home, Ya se ve bonito al menos para la pantalla 2k y la 1080p. quizas en resoluciones mayores se pueda distorsionar un poco

* Los botones ya están bien pero aun no son funcionales, falta hacer el controller

* Corregí la NavBar que estaba terriblemente fea  
