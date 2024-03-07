using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Jugadores(string nombre, string posicion, string equipo)
        {
            List<Jugador> jugadores;

            if (!string.IsNullOrEmpty(nombre))
            {
                jugadores = await repo.GetJugadoresByNombreAsync(nombre);
            }
            else if (!string.IsNullOrEmpty(posicion))
            {
                jugadores = await repo.GetJugadoresByPosicionAsync(posicion);
            }
            else if (!string.IsNullOrEmpty(equipo))
            {
                jugadores = await repo.GetJugadoresByEquipoAsync(equipo);
            }
            else
            {
                jugadores = await repo.GetAllJugadoresAsync();
            }

            var equipos = await repoEq.GetAllEquiposAsync();

            var viewModel = new JugadoresViewModel
            {
                Jugadores = jugadores,
                Equipos = equipos
            };

            return View(viewModel);
        }


        public async Task<IActionResult> DetalleJugador(int id)
        {
            var jugador = await this.repo.ObtenerJugadorPorId(id);
            return View(jugador);
        }
    }
}
