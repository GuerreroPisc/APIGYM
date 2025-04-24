using GYMHECTORAPI.Entities.Usuario;
using GYMHECTORAPI.Models.GTMHECTOR.DB;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GYMHECTORAPI.DataAccess
{
    public class UsuarioDO : IUsuarioDO
    {
        private readonly GymHectorContext _context;
        private readonly ILogger<UsuarioDO> _log;

        public UsuarioDO(GymHectorContext context, ILogger<UsuarioDO> log)
        {
            _context = context;
            _log = log;
        }

        public async Task<DatosUsuario> ObtenerDatosUsuario(int idUsuario)
        {

            var idUsuarioParam = new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@pintIdUsuario", Value = idUsuario };

            var respuesta = _context.MpSp_MaestroDatosUsuario
                .FromSqlRaw("EXEC MpSp_ObtenerDetalleUsuario " +
                " @pintIdUsuario ",
                idUsuarioParam
            ).AsEnumerable().Select(x => new DatosUsuario()
            {
                idRol = x.idRol,
                NombreCompleto = x.NombreCompleto,
                TipoSuscripcion = x.TipoSuscripcion,
                FechaInicio = x.FechaInicio,
                FechaFin = x.FechaFin,
                DiasRestantes = x.DiasRestantes,
                FlgSuscripcionActiva = x.FlgSuscripcionActiva
            }).FirstOrDefault();

            return respuesta;
        }
        public async Task<List<HorarioRegistrosUsuario_Result>> ListarHorariosRegistrados(int idUsuario)
        {
            var idUsuarioParam = new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@pintIdUsuario", Value = idUsuario };

            var response = _context.MpSp_HorarioRegistrosUsuario
                .FromSqlRaw("EXEC MpSp_ListarHorariosReservados " +
                " @pintIdUsuario ",
                idUsuarioParam
            ).ToList();

            if (response == null)
            {
                return new List<HorarioRegistrosUsuario_Result>();
            }
            return response;
        }

        public async Task<List<ListaHorariosGeneral_Result>> ListaHorariosGenerales(int idUsuario)
        {
            var idUsuarioParam = new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@pintIdUsuario", Value = idUsuario };

            var response = _context.MpSp_HorariosGenerales
                .FromSqlRaw("EXEC MpSp_ListarHorariosGenerales " +
                " @pintIdUsuario ",
                idUsuarioParam
            ).ToList();

            if (response == null)
            {
                return new List<ListaHorariosGeneral_Result>();
            }
            return response;
        }
    }
}
