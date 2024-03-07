using Microsoft.AspNetCore.Mvc;
using MvcNba.Extensions;
using MvcNba.Helpers;
using MvcNba.Models;
using MvcNba.Repositories;


namespace MvcNba.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryUsuarios repo;
        private HelperCryptography helper;

        public UsuariosController(RepositoryUsuarios repo)
        {
            this.repo = repo;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            bool loginSuccess = await repo.LogInUserAsync(username, password);
            if (loginSuccess)
            {
                Usuario user = repo.GetUser(username);
                HttpContext.Session.SetObject("CurrentUser", user);
                return RedirectToAction("PartidosJugados", "Partidos");
            }
            else
            {
                ViewData["Mensaje"] = "Credenciales incorrectas. Por favor, inténtalo de nuevo.";
                return View();
            }
        }
        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registro(string nombre, string password, string confirmPassword, string email)
        {
            if (repo.UsuarioExists(nombre))
            {
                ViewData["Mensaje"] = "El nombre de usuario ya está en uso. Por favor, elige otro.";
                return View();
            }
            if (!string.IsNullOrEmpty(email) && repo.EmailExists(email))
            {
                ViewData["Mensaje"] = "El correo electrónico ya está en uso. Por favor, utiliza otro.";
                return View();
            }
            if (password != confirmPassword)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden. Por favor, inténtalo de nuevo.";
                return View();
            }
            // Llamar al método RegisterUserAsync con la contraseña tal como está
            await repo.RegisterUserAsync(nombre, password, email);

            Usuario user = repo.GetUser(nombre);
            HttpContext.Session.SetObject("CurrentUser", user);

            return RedirectToAction("PartidosJugados", "Partidos");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CurrentUser");
            return RedirectToAction("Login");
        }
    }
}
