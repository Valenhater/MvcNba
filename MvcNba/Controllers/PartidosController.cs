using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MvcNba.Helpers;
using MvcNba.Repositories;

namespace MvcNba.Controllers
{
    public class PartidosController : Controller
    {
        private RepositoryNba repo;
        public PartidosController(RepositoryNba repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> PartidosJugados()
        {
            var partidosJugados = await this.repo.GetPartidosJugadosAsync();
            return View(partidosJugados);
        }
        public async Task<IActionResult> Index()
        {
            var equipos = await this.repo.GetAllEquiposAsync();
            return View(equipos);
        }
       
       
        public IActionResult Detalle(int id)
        {
            // Lógica para recuperar los detalles del equipo basados en el ID
            var equipo = this.repo.ObtenerEquipoPorId(id);

            // Pasa los detalles del equipo a la vista
            return View(equipo);
        }
    }
}
