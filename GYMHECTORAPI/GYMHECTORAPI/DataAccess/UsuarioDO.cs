using GYMHECTORAPI.Entities.Usuario;
using GYMHECTORAPI.Models.GTMHECTOR.DB;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

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

        public async Task<RegistrarReservaResponse> registrarReserva(int idUsuarioEdita, int idHorario, int flagImpedimentos)
        {
            try
            {
                var idUsuarioParam = new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@pintIdUsuario", Value = idUsuarioEdita };
                var idhorarioParam = new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@pintIdHorario", Value = idHorario };
                var flagImpedimentosParam = new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@pintFlagImpedimentos", Value = flagImpedimentos };
                var idUsuarioRegistraParam = new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@pintIdUsuarioRegistro", Value = idUsuarioEdita };

                var respuesta = _context.MpSp_RegistrarReserva
                    .FromSqlRaw("EXEC MpSp_RegistrarReservaUsuario " +
                    " @pintIdUsuario, " +
                    " @pintIdHorario, " +
                    " @pintFlagImpedimentos, " +
                    " @pintIdUsuarioRegistro ",
                    idUsuarioParam,
                    idhorarioParam,
                    flagImpedimentosParam,
                    idUsuarioRegistraParam
                ).AsEnumerable().Select(x => new RegistrarReservaResponse()
                {
                    codigoRes = (HttpStatusCode)x.codigo.GetValueOrDefault(),
                    mensajeRes = x.descripcion
                }).FirstOrDefault();

                if (respuesta != null)
                {
                    return respuesta;
                }
                return new RegistrarReservaResponse()
                {
                    codigoRes = HttpStatusCode.InternalServerError,
                    mensajeRes = "No se obtuvo respuesta al editar los datos del usuario."
                };
            }
            catch (Exception ex)
            {
                _log.LogError("{EditarUsuario} Error: " + ex.ToString());
                return new RegistrarReservaResponse()
                {
                    codigoRes = HttpStatusCode.InternalServerError,
                    mensajeRes = "No se pudo editar los datos del usuario. " + ex.ToString()
                };
            }
        }

        public async Task<List<ListarPosiblesAsistencias_Result>> CapacidadHorariosIA(int idUsuario)
        {
            var idUsuarioParam = new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@pintIdUsuario", Value = idUsuario };

            var response = _context.MpSp_PosiblesAsistencias
                .FromSqlRaw("EXEC MpSp_ListarPosiblesAsistencias " +
                " @pintIdUsuario ",
                idUsuarioParam
            ).ToList();

            if (response == null)
            {
                return new List<ListarPosiblesAsistencias_Result>();
            }
            return response;
        }

        public async Task<List<ListarAsistencias_Result>> AsistenciasIA(int idUsuario)
        {
            var idUsuarioParam = new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@pintIdUsuario", Value = idUsuario };

            var response = _context.MpSp_Asistencias
                .FromSqlRaw("EXEC MpSp_ListarAsistencias " +
                " @pintIdUsuario ",
                idUsuarioParam
            ).ToList();

            if (response == null)
            {
                return new List<ListarAsistencias_Result>();
            }
            return response;
        }

        public async Task<List<ListarReservas_Result>> ReservasIA(int idUsuario)
        {
            var idUsuarioParam = new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@pintIdUsuario", Value = idUsuario };

            var response = _context.MpSp_ReservasIA
                .FromSqlRaw("EXEC MpSp_ListarReservasIA " +
                " @pintIdUsuario ",
                idUsuarioParam
            ).ToList();

            if (response == null)
            {
                return new List<ListarReservas_Result>();
            }
            return response;
        }
    }
}
