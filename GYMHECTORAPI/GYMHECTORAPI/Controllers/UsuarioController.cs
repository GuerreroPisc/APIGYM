using GYMHECTORAPI.Bussiness;
using GYMHECTORAPI.Entities.Usuario;
using GYMHECTORAPI.Models.GTMHECTOR.DB;
using GYMHECTORAPI.Util;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace GYMHECTORAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioBO _iUsuarioBO;
        private readonly GymHectorContext _context;
        private readonly ILogger<UsuarioController> _log;
        public UsuarioController(IUsuarioBO iUsuarioBO, GymHectorContext context, ILogger<UsuarioController> log)
        {
            _iUsuarioBO = iUsuarioBO;
            _context = context;
            _log = log;
        }

        private void DisposeResources()
        {
            _context.Dispose();
        }

        [HttpGet]
        [Route("listarMaestros")]
        public async Task<IActionResult> ListarMaestros()
        {
            try
            {
                var principal = User;
                var validToken = HelperToken.LeerToken(principal);
                if (validToken.codigo != 1)
                {
                    return Unauthorized(new { Message = "No se pudo validar el token." });
                }

                var idUsuario = Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
                var respuesta = await _iUsuarioBO.ListarMaestros(idUsuario);
                _log.LogInformation("{ListarMaestros} Response: " + JsonSerializer.Serialize(respuesta));
                if (respuesta != null)
                {
                    if (respuesta.codigoRes == HttpStatusCode.OK)
                    {
                        return Ok(new { Message = respuesta.mensajeRes, data = respuesta.data });
                    }
                    else if (respuesta.codigoRes == HttpStatusCode.BadRequest)
                    {
                        return BadRequest(new { MessageError = respuesta.mensajeRes, MessageUser = respuesta.mensajeRes });

                    }
                    else
                    {
                        return StatusCode((int)respuesta.codigoRes, new { MessageError = respuesta.mensajeRes, MessageUser = "Error. Vuelva a intentarlo." });
                    }
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { MessageError = "Error interno en el servicio de listar maestros.", MessageUser = "Error. Vuelva a intentarlo." });
                }
            }
            catch (Exception ex)
            {
                _log.LogError("{ListarMaestros} Error: " + ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError, new { MessageError = "Error interno en el servicio de listar maestros." + ex.ToString(), MessageUser = "Error al cargar. Vuelva a intentarlo." });
            }
            finally
            {
                DisposeResources();
            }
        }

        [HttpGet]
        [Route("listarHorariosGenerales")]
        public async Task<IActionResult> ListarHorariosGenerales()
        {
            try
            {
                var principal = User;
                var validToken = HelperToken.LeerToken(principal);
                if (validToken.codigo != 1)
                {
                    return Unauthorized(new { Message = "No se pudo validar el token." });
                }

                var idUsuario = Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
                var respuesta = await _iUsuarioBO.ListarHorariosGenerales(idUsuario);
                _log.LogInformation("{ListarHorariosGenerales} Response: " + JsonSerializer.Serialize(respuesta));
                if (respuesta != null)
                {
                    if (respuesta.codigoRes == HttpStatusCode.OK)
                    {
                        return Ok(new { Message = respuesta.mensajeRes, data = respuesta.data });
                    }
                    else if (respuesta.codigoRes == HttpStatusCode.BadRequest)
                    {
                        return BadRequest(new { MessageError = respuesta.mensajeRes, MessageUser = respuesta.mensajeRes });

                    }
                    else
                    {
                        return StatusCode((int)respuesta.codigoRes, new { MessageError = respuesta.mensajeRes, MessageUser = "Error. Vuelva a intentarlo." });
                    }
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { MessageError = "Error interno en el servicio de listar horarios generales.", MessageUser = "Error. Vuelva a intentarlo." });
                }
            }
            catch (Exception ex)
            {
                _log.LogError("{ListarHorariosGenerales} Error: " + ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError, new { MessageError = "Error interno en el servicio de listar horarios generales.." + ex.ToString(), MessageUser = "Error al cargar. Vuelva a intentarlo." });
            }
            finally
            {
                DisposeResources();
            }
        }

        [HttpPost]
        [Route("registrarReserva")]
        public async Task<IActionResult> registrarReserva(RegistraReservaRequest req)
        {
            try
            {
                _log.LogInformation("{registrarReserva} Request: " + JsonSerializer.Serialize(req));
                var principal = User;
                var validToken = HelperToken.LeerToken(principal);
                if (validToken.codigo != 1)
                {
                    return Unauthorized(new { Message = "No se pudo validar el token." });
                }

                var idUsuario = Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
                var respuesta = await _iUsuarioBO.registrarReserva(idUsuario, req);
                _log.LogInformation("{EditarUsuario} Response: " + JsonSerializer.Serialize(respuesta));
                if (respuesta != null)
                {
                    if (respuesta.codigoRes == HttpStatusCode.OK)
                    {
                        return Ok(new { Message = respuesta.mensajeRes });
                    }
                    else if (respuesta.codigoRes == HttpStatusCode.BadRequest || respuesta.codigoRes == HttpStatusCode.UnprocessableContent)
                    {
                        return StatusCode((int)respuesta.codigoRes, new { MessageError = respuesta.mensajeRes, MessageUser = respuesta.mensajeRes });
                    }
                    return StatusCode((int)respuesta.codigoRes, new { MessageError = respuesta.mensajeRes, MessageUser = "Error. Vuelva a intentarlo." });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { MessageError = "Error interno en el servicio de editar usuario.", MessageUser = "Error. Vuelva a intentarlo." });
                }
            }
            catch (Exception ex)
            {
                _log.LogError("{EditarUsuario} Error: " + ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError, new { MessageError = "Error interno en el servicio de editar usuario.", MessageUser = "Error al editar. Vuelva a intentarlo." });
            }
            finally
            {
                DisposeResources();
            }
        }

        [HttpGet]
        [Route("CapacidadHorariosIA")]
        public async Task<IActionResult> CapacidadHorariosIA()
        {
            try
            {
                var principal = User;
                var validToken = HelperToken.LeerToken(principal);
                if (validToken.codigo != 1)
                {
                    return Unauthorized(new { Message = "No se pudo validar el token." });
                }

                var idUsuario = Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
                var respuesta = await _iUsuarioBO.CapacidadHorariosIA(idUsuario);
                _log.LogInformation("{ListarHorariosGenerales} Response: " + JsonSerializer.Serialize(respuesta));
                if (respuesta != null)
                {
                    if (respuesta.codigoRes == HttpStatusCode.OK)
                    {
                        return Ok(new { Message = respuesta.mensajeRes, data = respuesta.data });
                    }
                    else if (respuesta.codigoRes == HttpStatusCode.BadRequest)
                    {
                        return BadRequest(new { MessageError = respuesta.mensajeRes, MessageUser = respuesta.mensajeRes });

                    }
                    else
                    {
                        return StatusCode((int)respuesta.codigoRes, new { MessageError = respuesta.mensajeRes, MessageUser = "Error. Vuelva a intentarlo." });
                    }
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { MessageError = "Error interno en el servicio de mostrar capacidad de aforo aproximado.", MessageUser = "Error. Vuelva a intentarlo." });
                }
            }
            catch (Exception ex)
            {
                _log.LogError("{CapacidadHorariosIA} Error: " + ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError, new { MessageError = "Error interno en el servicio de mostrar capacidad de aforo aproximado." + ex.ToString(), MessageUser = "Error al cargar. Vuelva a intentarlo." });
            }
            finally
            {
                DisposeResources();
            }
        }
    }
}
