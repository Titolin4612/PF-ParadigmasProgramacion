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
                if (TempData.ContainsKey("SuccessMessage")) // Para mensajes de �xito generales
                {
                    ViewBag.Success = TempData["SuccessMessage"] as string;
                }


                return View();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeController.Index: Error al cargar datos - {ex.Message}");
                ViewBag.Error = "Ocurri� un error al cargar la p�gina.";
                return View();
            }
        }

        // ... (Otros m�todos como AgregarJugador, EliminarJugador, Play se mantienen igual) ...
        [HttpPost]
        public IActionResult AgregarJugador(string nickname, int apuesta)
        {
            var usuarioSesion = HttpContext.Session.GetString("UsuarioSesion");
            if (string.IsNullOrEmpty(usuarioSesion) || !_homeService.BuscarUsuario(usuarioSesion))
            {
                TempData["LoginError"] = "Debes iniciar sesi�n para agregar un jugador.";
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
                TempData["ErrorGlobal"] = "Ocurri� un error al agregar el jugador.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult EliminarJugador()
        {
            var usuarioSesion = HttpContext.Session.GetString("UsuarioSesion");
            if (string.IsNullOrEmpty(usuarioSesion) || !_homeService.BuscarUsuario(usuarioSesion))
            {
                TempData["LoginError"] = "Debes iniciar sesi�n para eliminar un jugador.";
                return RedirectToAction("Login", "Home");
            }

            try
            {
                _homeService.EliminarUltimoJugadorConfigurado();
                TempData["SuccessMessage"] = "�ltimo jugador eliminado correctamente.";
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
                TempData["ErrorGlobal"] = "Ocurri� un error al eliminar el jugador.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Play()
        {
            var usuarioSesion = HttpContext.Session.GetString("UsuarioSesion");
            if (string.IsNullOrEmpty(usuarioSesion) || !_homeService.BuscarUsuario(usuarioSesion))
            {
                TempData["LoginError"] = "Debes iniciar sesi�n para iniciar el juego.";
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
            // Si viene un error espec�fico de login (ej. necesitas loguearte para X acci�n)
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
        [ValidateAntiForgeryToken] // Buena pr�ctica para POST
        public IActionResult Login(string nickname, string contrase�a)
        {
            // 1. Validaciones b�sicas de input
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 4)
            {
                ViewBag.Error = "El nickname debe tener al menos 4 caracteres.";
                return View(); // Devuelve a la vista Login con el error
            }

            if (string.IsNullOrWhiteSpace(contrase�a) || contrase�a.Length < 6)
            {
                ViewBag.Error = "La contrase�a debe tener al menos 6 caracteres.";
                return View(); // Devuelve a la vista Login con el error
            }

            // 2. Verificar si el usuario existe
            // Pon un breakpoint AQU� para ver qu� devuelve _homeService.BuscarUsuario(nickname)
            Debug.WriteLine($"Intentando buscar usuario: {nickname}");
            if (!_homeService.BuscarUsuario(nickname))
            {
                Debug.WriteLine($"Usuario '{nickname}' NO encontrado. Redirigiendo a Signup.");
                // Usuario no existe, redirigir a Signup con un mensaje
                TempData["SignupPrompt"] = $"El usuario '{nickname}' no est� registrado. Por favor, crea una cuenta.";
                TempData["PrefillNickname"] = nickname; // Opcional: para pre-rellenar el nickname en el form de signup
                return RedirectToAction("Signup");
            }
            Debug.WriteLine($"Usuario '{nickname}' encontrado. Verificando contrase�a.");

            // 3. Usuario existe, verificar contrase�a
            if (!_homeService.BuscarUsuario(nickname, contrase�a))
            {
                Debug.WriteLine($"Contrase�a INCORRECTA para usuario '{nickname}'.");
                // Contrase�a incorrecta
                // Usamos TempData si quisi�ramos redirigir, pero como es return View(), ViewBag es suficiente.
                // Sin embargo, para consistencia con otros errores que puedan venir de TempData:
                ViewBag.Error = "Nickname o contrase�a incorrectos. Por favor, int�ntalo de nuevo.";
                return View(); // Devuelve a la vista Login con el error
            }

            // 4. Login exitoso
            Debug.WriteLine($"Login EXITOSO para usuario '{nickname}'.");
            HttpContext.Session.SetString("UsuarioSesion", nickname);
            TempData["SuccessMessage"] = $"�Bienvenido de nuevo, {nickname}!";
            return RedirectToAction("Index");
        }

        public IActionResult Signup()
        {
            if (TempData.ContainsKey("SignupError")) // Error de un intento previo de signup
            {
                ViewBag.Error = TempData["SignupError"] as string;
            }
            if (TempData.ContainsKey("SignupPrompt")) // Mensaje si se redirigi� desde Login
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
        [ValidateAntiForgeryToken] // Buena pr�ctica para POST
        public IActionResult Signup(string nickname, string contrase�a)
        {
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 4)
            {
                TempData["SignupError"] = "El nickname debe tener al menos 4 caracteres.";
                TempData["PrefillNickname"] = nickname; // Mantener el nickname para rellenar
                return RedirectToAction("Signup"); // Usar RedirectToAction para que TempData funcione bien y la URL sea limpia
            }

            if (string.IsNullOrWhiteSpace(contrase�a) || contrase�a.Length < 6)
            {
                TempData["SignupError"] = "La contrase�a debe tener al menos 6 caracteres.";
                TempData["PrefillNickname"] = nickname;
                return RedirectToAction("Signup");
            }

            if (_homeService.BuscarUsuario(nickname))
            {
                TempData["SignupError"] = $"El nickname '{nickname}' ya est� registrado. Intenta iniciar sesi�n.";
                // No pre-rellenamos contrase�a aqu� por seguridad y porque el usuario deber�a ir a Login.
                return RedirectToAction("Signup");
            }

            try
            {
                _homeService.RegistrarUsuario(nickname, contrase�a);
                HttpContext.Session.SetString("UsuarioSesion", nickname);
                TempData["SuccessMessage"] = $"�Cuenta creada exitosamente! Bienvenido, {nickname}.";
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex) // Espec�fico de validaciones en RegistrarUsuario
            {
                Debug.WriteLine($"HomeController.Signup: Error de argumento al registrar - {ex.Message}");
                TempData["SignupError"] = ex.Message; // Usar mensaje de la excepci�n si es amigable
                TempData["PrefillNickname"] = nickname;
                return RedirectToAction("Signup");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeController.Signup: Error al registrar usuario - {ex.Message}");
                TempData["SignupError"] = "Ocurri� un error inesperado al registrar el usuario. Int�ntalo m�s tarde.";
                TempData["PrefillNickname"] = nickname;
                return RedirectToAction("Signup");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Has cerrado sesi�n correctamente.";
            return RedirectToAction("Login");
        }

        public IActionResult Privacy() => View();

        public IActionResult Reglas()
        {
            var usuarioSesion = HttpContext.Session.GetString("UsuarioSesion");
            if (string.IsNullOrEmpty(usuarioSesion) || !_homeService.BuscarUsuario(usuarioSesion))
            {
                TempData["LoginError"] = "Debes iniciar sesi�n para ver las reglas.";
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
    }

}