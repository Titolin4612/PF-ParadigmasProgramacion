using Microsoft.AspNetCore.Mvc;
using MVC_ProyectoFinalPOO.Services;
using System.Text.Json;
using System.Diagnostics;
using CL_ProyectoFinalPOO.Interfaces;
namespace MVC_ProyectoFinalPOO.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IJuegoService _juegoService;
        public HomeController(IHomeService homeService, IJuegoService juegoService)
        {
            _homeService = homeService;
            _juegoService = juegoService;
        }

        public IActionResult Index()
        {
            try
            {
                var usuarioSesion = HttpContext.Session.GetString("UsuarioSesion");
                if (!string.IsNullOrEmpty(usuarioSesion) && _homeService.BuscarUsuario(usuarioSesion))
                {
                    ViewBag.Players = _homeService.ObtenerJugadoresConfigurados();
                }
                else
                {
                    ViewBag.Players = null;
                }

                if (TempData.ContainsKey("ErrorGlobal"))
                {
                    ViewBag.Error = TempData["ErrorGlobal"] as string;
                }
                if (TempData.ContainsKey("SuccessMessage")) // Para mensajes de éxito generales
                {
                    ViewBag.Success = TempData["SuccessMessage"] as string;
                }


                return View();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeController.Index: Error al cargar datos - {ex.Message}");
                ViewBag.Error = "Ocurrió un error al cargar la página.";
                return View();
            }
        }

        // ... (Otros métodos como AgregarJugador, EliminarJugador, Play se mantienen igual) ...
        [HttpPost]
        public IActionResult AgregarJugador(string nickname, int apuesta)
        {
            var usuarioSesion = HttpContext.Session.GetString("UsuarioSesion");
            if (string.IsNullOrEmpty(usuarioSesion) || !_homeService.BuscarUsuario(usuarioSesion))
            {
                TempData["LoginError"] = "Debes iniciar sesión para agregar un jugador.";
                return RedirectToAction("Login", "Home");
            }

            try
            {
                _homeService.AgregarJugadorConfigurado(nickname, apuesta);
                TempData["SuccessMessage"] = $"Jugador '{nickname}' agregado correctamente.";
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorGlobal"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorGlobal"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeController.AgregarJugador: Error inesperado - {ex.Message}");
                TempData["ErrorGlobal"] = "Ocurrió un error al agregar el jugador.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult EliminarJugador()
        {
            var usuarioSesion = HttpContext.Session.GetString("UsuarioSesion");
            if (string.IsNullOrEmpty(usuarioSesion) || !_homeService.BuscarUsuario(usuarioSesion))
            {
                TempData["LoginError"] = "Debes iniciar sesión para eliminar un jugador.";
                return RedirectToAction("Login", "Home");
            }

            try
            {
                _homeService.EliminarUltimoJugadorConfigurado();
                TempData["SuccessMessage"] = "Último jugador eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorGlobal"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeController.EliminarJugador: Error inesperado - {ex.Message}");
                TempData["ErrorGlobal"] = "Ocurrió un error al eliminar el jugador.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Play()
        {
            var usuarioSesion = HttpContext.Session.GetString("UsuarioSesion");
            if (string.IsNullOrEmpty(usuarioSesion) || !_homeService.BuscarUsuario(usuarioSesion))
            {
                TempData["LoginError"] = "Debes iniciar sesión para iniciar el juego.";
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var jugadoresConfigurados = _homeService.ValidarConfiguracionJugadoresParaJuego();
                HttpContext.Session.SetString("ListaJugadoresConfig", JsonSerializer.Serialize(jugadoresConfigurados));
                _juegoService.IniciarJuego(jugadoresConfigurados);
                return RedirectToAction("Index", "Juego");
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorGlobal"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeController.Play: Error al iniciar juego - {ex.Message}");
                TempData["ErrorGlobal"] = "Error al intentar iniciar el juego.";
                return RedirectToAction("Index");
            }
        }


        public IActionResult Login()
        {
            // Si viene un error específico de login (ej. necesitas loguearte para X acción)
            if (TempData.ContainsKey("LoginError"))
            {
                ViewBag.Error = TempData["LoginError"] as string;
            }
            // Si viene un error general de un intento de login fallido que NO redirige a signup
            if (TempData.ContainsKey("LoginFailedError"))
            {
                ViewBag.Error = TempData["LoginFailedError"] as string;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Buena práctica para POST
        public IActionResult Login(string nickname, string contraseña)
        {
            // 1. Validaciones básicas de input
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 4)
            {
                ViewBag.Error = "El nickname debe tener al menos 4 caracteres.";
                return View(); // Devuelve a la vista Login con el error
            }

            if (string.IsNullOrWhiteSpace(contraseña) || contraseña.Length < 6)
            {
                ViewBag.Error = "La contraseña debe tener al menos 6 caracteres.";
                return View(); // Devuelve a la vista Login con el error
            }

            // 2. Verificar si el usuario existe
            // Pon un breakpoint AQUÍ para ver qué devuelve _homeService.BuscarUsuario(nickname)
            Debug.WriteLine($"Intentando buscar usuario: {nickname}");
            if (!_homeService.BuscarUsuario(nickname))
            {
                Debug.WriteLine($"Usuario '{nickname}' NO encontrado. Redirigiendo a Signup.");
                // Usuario no existe, redirigir a Signup con un mensaje
                TempData["SignupPrompt"] = $"El usuario '{nickname}' no está registrado. Por favor, crea una cuenta.";
                TempData["PrefillNickname"] = nickname; // Opcional: para pre-rellenar el nickname en el form de signup
                return RedirectToAction("Signup");
            }
            Debug.WriteLine($"Usuario '{nickname}' encontrado. Verificando contraseña.");

            // 3. Usuario existe, verificar contraseña
            if (!_homeService.BuscarUsuario(nickname, contraseña))
            {
                Debug.WriteLine($"Contraseña INCORRECTA para usuario '{nickname}'.");
                // Contraseña incorrecta
                // Usamos TempData si quisiéramos redirigir, pero como es return View(), ViewBag es suficiente.
                // Sin embargo, para consistencia con otros errores que puedan venir de TempData:
                ViewBag.Error = "Nickname o contraseña incorrectos. Por favor, inténtalo de nuevo.";
                return View(); // Devuelve a la vista Login con el error
            }

            // 4. Login exitoso
            Debug.WriteLine($"Login EXITOSO para usuario '{nickname}'.");
            HttpContext.Session.SetString("UsuarioSesion", nickname);
            TempData["SuccessMessage"] = $"¡Bienvenido de nuevo, {nickname}!";
            return RedirectToAction("Index");
        }

        public IActionResult Signup()
        {
            if (TempData.ContainsKey("SignupError")) // Error de un intento previo de signup
            {
                ViewBag.Error = TempData["SignupError"] as string;
            }
            if (TempData.ContainsKey("SignupPrompt")) // Mensaje si se redirigió desde Login
            {
                ViewBag.Prompt = TempData["SignupPrompt"] as string;
            }
            if (TempData.ContainsKey("PrefillNickname")) // Para pre-rellenar
            {
                ViewBag.PrefillNickname = TempData["PrefillNickname"] as string;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Buena práctica para POST
        public IActionResult Signup(string nickname, string contraseña)
        {
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 4)
            {
                TempData["SignupError"] = "El nickname debe tener al menos 4 caracteres.";
                TempData["PrefillNickname"] = nickname; // Mantener el nickname para rellenar
                return RedirectToAction("Signup"); // Usar RedirectToAction para que TempData funcione bien y la URL sea limpia
            }

            if (string.IsNullOrWhiteSpace(contraseña) || contraseña.Length < 6)
            {
                TempData["SignupError"] = "La contraseña debe tener al menos 6 caracteres.";
                TempData["PrefillNickname"] = nickname;
                return RedirectToAction("Signup");
            }

            if (_homeService.BuscarUsuario(nickname))
            {
                TempData["SignupError"] = $"El nickname '{nickname}' ya está registrado. Intenta iniciar sesión.";
                // No pre-rellenamos contraseña aquí por seguridad y porque el usuario debería ir a Login.
                return RedirectToAction("Signup");
            }

            try
            {
                _homeService.RegistrarUsuario(nickname, contraseña);
                HttpContext.Session.SetString("UsuarioSesion", nickname);
                TempData["SuccessMessage"] = $"¡Cuenta creada exitosamente! Bienvenido, {nickname}.";
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex) // Específico de validaciones en RegistrarUsuario
            {
                Debug.WriteLine($"HomeController.Signup: Error de argumento al registrar - {ex.Message}");
                TempData["SignupError"] = ex.Message; // Usar mensaje de la excepción si es amigable
                TempData["PrefillNickname"] = nickname;
                return RedirectToAction("Signup");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeController.Signup: Error al registrar usuario - {ex.Message}");
                TempData["SignupError"] = "Ocurrió un error inesperado al registrar el usuario. Inténtalo más tarde.";
                TempData["PrefillNickname"] = nickname;
                return RedirectToAction("Signup");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Has cerrado sesión correctamente.";
            return RedirectToAction("Login");
        }

        public IActionResult Privacy() => View();

        public IActionResult Reglas()
        {
            var usuarioSesion = HttpContext.Session.GetString("UsuarioSesion");
            if (string.IsNullOrEmpty(usuarioSesion) || !_homeService.BuscarUsuario(usuarioSesion))
            {
                TempData["LoginError"] = "Debes iniciar sesión para ver las reglas.";
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
    }

}