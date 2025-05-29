using GYMHECTORAPI.DataAccess;
using GYMHECTORAPI.Entities.Usuario;
using System.Data;
using System.Net;

namespace GYMHECTORAPI.Bussiness
{
    public class UsuarioBO : IUsuarioBO
    {
        private readonly IUsuarioDO _usuarioDO;
        private readonly ILogger<UsuarioBO> _log;
        public UsuarioBO(IUsuarioDO usuarioDO, ILogger<UsuarioBO> log)
        {
            _usuarioDO = usuarioDO;
            _log = log;
        }

        public async Task<ListarMaestrosResponse> ListarMaestros(int idUsuario)
        {
            try
            {
                var response = new ListarMaestrosResponse();

                response.data = new DataMaestros();
                response.data.datosUsuario = await _usuarioDO.ObtenerDatosUsuario(idUsuario);
                var listaHorarios = await _usuarioDO.ListarHorariosRegistrados(idUsuario);

                if (listaHorarios != null && listaHorarios.Count > 0)
                {
                    response.data.ListaHorariosRegistrados = listaHorarios.Select(x => new ListaHorariosRegistrados()
                    {
                        idHorarioRegistrado = x.idHorarioRegistrado,
                        NombreClase = x.NombreClase,
                        FechaInicio = x.FechaInicio,
                        FechaFin = x.FechaFin,
                        NombreProfesor = x.NombreProfesor
                    }).ToList();
                }

                if (response.data.datosUsuario != null)
                {
                    response.codigoRes = HttpStatusCode.OK;
                    response.mensajeRes = "Maestros obtenidos correctamente.";
                    response.data = response.data;
                }
                else
                {
                    response.codigoRes = HttpStatusCode.BadRequest;
                    response.mensajeRes = "No se pudo obtener los maestros.";
                }

                return response;
            }
            catch (Exception ex)
            {
                _log.LogError("{ListarMaestros} Error: " + ex.ToString());
                return new ListarMaestrosResponse
                {
                    codigoRes = HttpStatusCode.InternalServerError,
                    mensajeRes = "Error interno al obtener los maestros. " + ex.ToString()
                };
            }

        }

        public async Task<ListarHorariosGenerales> ListarHorariosGenerales(int idUsuario)
        {
            try
            {
                var response = new ListarHorariosGenerales();

                response.data = new DataHorarios();
                var listaHorarios = await _usuarioDO.ListaHorariosGenerales(idUsuario);

                if (listaHorarios != null && listaHorarios.Count > 0)
                {
                    var dataOrdenada = listaHorarios
                            .GroupBy(x => x.nombreClase)
                            .Select(g => new ListaHorariosGeneral
                            {
                                nombreClase = g.Key,
                                horariosClase = g.Select(h => new ListaHorariosClase
                                {
                                    idHorario = h.idHorario,
                                    FechaInicio = h.FechaInicio,
                                    FechaFin = h.FechaFin,
                                    NombreProfesor = h.NombreProfesor,
                                    FlagDiponible = h.FlagDiponible,
                                    Descripcion = h.Descripcion
                                }).ToList()
                            })
                            .OrderBy(x => x.nombreClase)
                            .ToList();

                    response.codigoRes = HttpStatusCode.OK;
                    response.mensajeRes = "Horarios obtenidos correctamente.";
                    response.data.ListaHorarios = dataOrdenada;
                }
                else
                {
                    response.codigoRes = HttpStatusCode.BadRequest;
                    response.mensajeRes = "No se pudo obtener los horarios.";
                }

                return response;
            }
            catch (Exception ex)
            {
                _log.LogError("{ListarHorariosGenerales} Error: " + ex.ToString());
                return new ListarHorariosGenerales
                {
                    codigoRes = HttpStatusCode.InternalServerError,
                    mensajeRes = "Error interno al obtener los horarios. " + ex.ToString()
                };
            }

        }

        public async Task<RegistrarReservaResponse> registrarReserva(int idUsuarioEdita, RegistraReservaRequest req)
        {
            var idUsuario = idUsuarioEdita;
            if (idUsuario > 0 && req.intIdHorario > 0)
            {
                return await _usuarioDO.registrarReserva(idUsuario, req.intIdHorario, req.intFlagImpedimento);
            }
            else
            {
                return new RegistrarReservaResponse()
                {
                    codigoRes = HttpStatusCode.BadRequest,
                    mensajeRes = "El usuario o el horario no es válido."
                };
            }

        }


    }
}
