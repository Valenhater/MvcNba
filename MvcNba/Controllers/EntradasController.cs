using Microsoft.AspNetCore.Mvc;
using MvcNba.Extensions;
using MvcNba.Models;
using MvcNba.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcNba.Controllers
{
    public class EntradasController : Controller
    {
        private readonly RepositoryEntradas repo;

        public EntradasController(RepositoryEntradas repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> VistaReservaEntradas(int? idPartido)
        {
            if (idPartido != null)
            {
                List<int> partidosList = HttpContext.Session.GetObject<List<int>>("PARTIDOS") ?? new List<int>();
                partidosList.Add(idPartido.Value);
                HttpContext.Session.SetObject("PARTIDOS", partidosList);
                ViewData["MENSAJE"] = "Partido almacenado correctamente";
            }

            List<ModelVistaProximosPartidos> partidos = await repo.GetProximosPartidosAsync();
            return View(partidos);
        }

        public async Task<IActionResult> PartidosFavoritos(int? ideliminar)
        {
            List<int> idsPartidos = HttpContext.Session.GetObject<List<int>>("PARTIDOS");
            if (idsPartidos != null)
            {
                if (ideliminar != null)
                {
                    idsPartidos.Remove(ideliminar.Value);
                    HttpContext.Session.SetObject("PARTIDOS", idsPartidos);
                }
                List<ModelVistaProximosPartidos> partidos = await repo.GetFavoritosAsync(idsPartidos);
                return View(partidos);
            }
            return View();
        }
    }
}
