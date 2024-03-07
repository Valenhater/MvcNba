using Microsoft.AspNetCore.Mvc;
using MvcNba.Repositories;
using NuGet.Protocol.Core.Types;

namespace MvcNba.Controllers
{
    public class EntradasController : Controller
    {
        private RepositoryEntradas repo;

        public EntradasController(RepositoryEntradas repo)
        {
            this.repo = repo;
        }
        public IActionResult VistaReservaEntradas()
        {
            var proximosPartidos = this.repo.GetProximosPartidos();
            return View(proximosPartidos);
        }
    }
}
