using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcNba.Models;
using MvcNba.Repositories;

namespace MvcNba.Controllers
{
    public class JugadoresController : Controller
    {
        private RepositoryJugadores repo;
        private RepositoryNba repoEq;


        public JugadoresController(RepositoryJugadores repo, RepositoryNba repoEq)
        {
            this.repo = repo;
            this.repoEq = repoEq;
        }

        public async Task<IActionResult> Jugadores(int pagina = 1, string nombre = null, string posicion = null, string equipo = null)
        {
            const int tamanoPagina = 5;
            int indiceInicio = (pagina - 1) * tamanoPagina + 1;

            List<Jugador> jugadores;

            if (!string.IsNullOrEmpty(nombre))
            {
                jugadores = await this.repo.GetJugadoresByNombreAsync(nombre);
            }
            else if (!string.IsNullOrEmpty(posicion))
            {
                jugadores = await this.repo.GetJugadoresByPosicionAsync(posicion);
            }
            else if (!string.IsNullOrEmpty(equipo))
            {
                jugadores = await this.repo.GetJugadoresByEquipoAsync(equipo);
            }
            else
            {
                jugadores = await this.repo.GetGrupoJugadoresAsync(indiceInicio, tamanoPagina);
            }

            var equipos = await this.repoEq.GetAllEquiposAsync();

            var totalJugadores = await this.repo.GetNumeroTotalJugadoresAsync(); // Obtener el total de jugadores
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalJugadores / tamanoPagina); // Calcular el número total de páginas

            var viewModel = new JugadoresViewModel
            {
                Jugadores = jugadores,
                Equipos = equipos
            };

            ViewBag.Pagina = pagina;

            return View(viewModel);
        }

        public async Task<IActionResult> DetalleJugador(int id)
        {
            var jugador = await this.repo.ObtenerJugadorPorId(id);
            return View(jugador);
        }
    }
}
