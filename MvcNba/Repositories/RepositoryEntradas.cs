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
        public async Task<List<ModelVistaProximosPartidos>> GetProximosPartidosAsync()
        {
            return  this.context.VistaProximosPartidos.ToList();
        }
        public async Task<ModelVistaProximosPartidos> FindPartidoAsync(int idPartido)
        {
            return await this.context.VistaProximosPartidos.FirstOrDefaultAsync(z => z.IdPartido == idPartido);
        }

        public async Task<List<ModelVistaProximosPartidos>> GetFavoritosAsync(List<int> ids)
        {
            var consulta = from datos in this.context.VistaProximosPartidos where ids.Contains(datos.IdPartido) select datos;
            if(consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                return await consulta.ToListAsync();
            }
        }
    }
}
