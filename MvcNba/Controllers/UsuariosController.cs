using Microsoft.AspNetCore.Mvc;
using MvcNba.Extensions;
using MvcNba.Helpers;
using MvcNba.Models;
using MvcNba.Repositories;
using System.Text;
using System.Threading.Tasks;

namespace MvcNba.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly RepositoryUsuarios repo;

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
        public async Task<IActionResult> Registro(string nombre, string password, string confirmPassword, string email, string nombreCompleto, string direccion)
        {
            if (repo.UsuarioExists(nombre))
            {
                ViewData["Mensaje"] = "Username ya en uso.";
                return View();
            }
            if (!string.IsNullOrEmpty(email) && repo.EmailExists(email))
            {
                ViewData["Mensaje"] = "Correo electronico ya en uso.";
                return View();
            }
            if (password != confirmPassword)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden.";
                return View();
            }

            await repo.RegisterUserAsync(nombre, password, email, nombreCompleto, direccion);

            Usuario user = repo.GetUser(nombre);
            HttpContext.Session.SetObject("CurrentUser", user);

            return RedirectToAction("PartidosJugados", "Partidos");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CurrentUser");
            return RedirectToAction("Login");
        }
        public IActionResult EditarPerfil()
        {
            var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");
            if (currentUser == null)
            {
                // Manejar la situación en la que no hay un usuario actual en la sesión
                return RedirectToAction("Login");
            }

            return View(currentUser); // Pasar el usuario actual como modelo a la vista
        }
        [HttpPost]
        public async Task<IActionResult> EditarPerfil(int id, string nombre, string email, string nombreCompleto, string direccion)
        {
            this.repo.UpdateUserAsync(id, nombre, email, nombreCompleto, direccion);

            // Actualizar la sesión con los nuevos datos del usuario
            var updatedUser = await this.repo.GetUserByIdAsync(id);

            if (updatedUser != null)
            {
                HttpContext.Session.SetObject("CurrentUser", updatedUser);
            }

            return RedirectToAction("PartidosJugados", "Partidos");
        }

        public async Task<IActionResult> VerPerfil(int id)
        {
            var perfilUsuario = await this.repo.GetUserByIdAsync(id);

            return View(perfilUsuario);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarPerfil(int id)
        {
            await repo.DeleteUserAsync(id);

            // Eliminar la sesión del usuario actual
            HttpContext.Session.Remove("CurrentUser");

            // Redireccionar al login después de eliminar el perfil
            return RedirectToAction("Login", "Usuarios");
        }
    }
}
