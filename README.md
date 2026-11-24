# BallGame

Patrones utilizados:

# Patron State

se usa para el menu prncipal. Asegura que solo una pantalla se muestre a la vez

# Service Locator

Se usa en el bootsrapper para mantener una sola referencia e instancia de servicios.
por ejemplo el event bus, el SceneLoader y el logger (plugin). este patron hace mucho mas facil tener singletons que sean a su vez monoibehaviours

# Singleton

como mencione antes, el servicelocator guarda una sola instancia de la clase proporcionada, lo cual funciona como un singleton. Un buen ejemplo es el eventBus, el cual debe ser unico y debe mantenerse a traves de la carga de escenas.

# Event Bus (y patron observer)

El event bus permite el desacoplamiento de partes separadas del codigo que necesitan comunicarse. por ejemplo, al recolectar una moneda puedo publicar un evento para que lo puedan leer las clases de player, de shop, etc, sin la necesidad de que coin conozca a ninguna.
Agrupo el patron observer aqui tambien ya que de manera interna ek eventBus funciona como un observer de los metodos subscritos

# Data Drivemn / Flyweight

Los scriptable objects permiten cargar ciertos tipos de objetos deuna sola manera, sin tener que añadirlos a codigo. Un ejemplo en el juego son las mejoras de la tienda. Son un scriptable object base que define los atributos de la mejora, y esta parte es la que se integra a la tienda. Luego se crean las mejoras a partir de esta base. Permite añadir objetos de una manera estandarizada y sin tener que escribir codigo.

# Startegy

El PlayerMovementHandler cambia entre tres modos de input exclusivos, y expone solo una variable a partir de el utilizado. Est me permite tener la opcion de varios metodos de entrada a la vez pero siempre conectarme a la misma salida de datos.

# Memento

Al momento de construir los cuartos con los scriptable objects, se les pueden realizar modificaciones y guardar nuevamente en el scriptable object. Esto es a traves del patron memento, haciendo una copia total del estado que se quiere guardar.