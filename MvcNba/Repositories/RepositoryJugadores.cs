using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNba.Data;
using MvcNba.Models;
using System.Data;

namespace MvcNba.Repositories
{

    #region PROCEDURES
    //    CREATE PROCEDURE SP_NEQUIPO_JUGADORES
    //    @NombreEquipo NVARCHAR(100)
    //AS
    //BEGIN
    //    SELECT j.*
    //    FROM Jugadores j
    //    INNER JOIN Equipos e ON j.EquipoId = e.EquipoId
    //    WHERE e.Nombre = @NombreEquipo
    //END
    //    CREATE VIEW VistaProximosPartidos AS
    //SELECT PP.PARTIDOID, EL.Nombre AS EquipoLocal, EV.Nombre AS EquipoVisitante, EL.Ciudad AS CiudadLocal, EV.Ciudad AS CiudadVisitante, PP.Fecha, PP.PrecioEntrada, PP.PLAZASDISPONIBLES, PP.IMAGENPARTIDO
    //FROM PROXIMOSPARTIDOS PP
    //JOIN EQUIPOS EL ON PP.EquipoLocalId = EL.EquipoId
    //JOIN EQUIPOS EV ON PP.EquipoVisitanteId = EV.EquipoId;
    #endregion
    public class RepositoryJugadores
    {
        private NbaContext context;
        public RepositoryJugadores(NbaContext context)
        {
            this.context = context;
        }
        public async Task<List<Jugador>> GetAllJugadoresAsync()
        {
            return await this.context.Jugadores.ToListAsync();
        }

        public async Task<List<Jugador>> GetJugadoresByNombreAsync(string nombre)
        {
            var consulta = await this.context.Jugadores.Where(j => j.Nombre.Contains(nombre)).ToListAsync();
            return consulta;
        }

        public async Task<List<Jugador>> GetJugadoresByPosicionAsync(string posicion)
        {
            var consulta = await this.context.Jugadores.Where(j => j.Posicion == posicion).ToListAsync();
            return consulta;
        }
        public async Task<List<Jugador>> GetJugadoresByEquipoAsync(string nombreEquipo)
        {
            // El procedimiento almacenado espera un parámetro de tipo NVARCHAR(100)
            var nombreEquipoParam = new SqlParameter("@NombreEquipo", SqlDbType.NVarChar, 100);
            nombreEquipoParam.Value = nombreEquipo;

            // Ejecutar el procedimiento almacenado y obtener los resultados
            var jugadores = await context.Jugadores
                .FromSqlRaw("EXECUTE SP_NEQUIPO_JUGADORES @NombreEquipo", nombreEquipoParam)
                .ToListAsync();

            return jugadores;
        }


        public async Task<Jugador> ObtenerJugadorPorId(int id)
        {
            return await this.context.Jugadores.FindAsync(id);
        }

    }
}
