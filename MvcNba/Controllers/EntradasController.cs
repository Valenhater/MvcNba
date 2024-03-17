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
            ViewData["MENSAJE"] = "No hay partidos almacenados";
            return View();
        }
        public async Task<IActionResult> ReservarEntrada(int partidoid)
        {
            // Obtener el modelo de ReservaEntrada esperando el resultado de la tarea
            ModelVistaProximosPartidos partido = await this.repo.FindPartidoAsync(partidoid);
            ViewData["PARTIDOID"] = partidoid;
            // Pasar el modelo a la vista
            return View(partido);
        }

        [HttpPost]
        public async Task<IActionResult> ReservarEntrada(int usuarioid, int partidoid, int asiento, int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                await this.repo.ReservarEntradaAsync(usuarioid, partidoid, asiento + i);
            }

            return RedirectToAction("EntradasReservadas", "Entradas");
        }
        public async Task<IActionResult> EntradasReservadas()
        {
            var reservas = await repo.GetReservasEntradasAsync();
            return View(reservas);
        }
        public async Task<IActionResult> EliminarReserva(int reservaId)
        {
            var exito = await repo.EliminarReservaEntradaAsync(reservaId);
            if (exito)
            {
                TempData["Mensaje"] = "Reserva eliminada correctamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo eliminar la reserva.";
            }
            return RedirectToAction("EntradasReservadas");
        }

    }
}
