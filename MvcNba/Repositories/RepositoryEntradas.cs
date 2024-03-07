using Microsoft.EntityFrameworkCore;
using MvcNba.Data;
using MvcNba.Models;

namespace MvcNba.Repositories
{
    public class RepositoryEntradas
    {
        private NbaContext context;

        public RepositoryEntradas(NbaContext context)
        {
            this.context = context;
        }
        public List<ModelVistaProximosPartidos> GetProximosPartidos()
        {
            return this.context.VistaProximosPartidos.ToList();
        }
    }
}
