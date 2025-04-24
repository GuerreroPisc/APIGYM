using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using GYMHECTORAPI.Bussiness;
using GYMHECTORAPI.Entities;
using GYMHECTORAPI.Entities.Modulos;
using GYMHECTORAPI.Models.GTMHECTOR.DB;
using GYMHECTORAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GYMHECTORAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuloController : ControllerBase
    {
        private readonly IModuloBO _iModuloBO;
        private readonly GymHectorContext _context;
        private readonly ILogger<ModuloController> _log;
        public ModuloController(IModuloBO iModuloBO, GymHectorContext context, ILogger<ModuloController> log)
        {
            _iModuloBO = iModuloBO;
            _context = context;
            _log = log;
        }

        private void DisposeResources()
        {
            _context.Dispose();
        }

        
        [HttpGet]
        [Route("Modulos")]
        public async Task<IActionResult> ObtenerModulosPorUsuario()
        {
            try
            {
                var principal = User;
                var validToken = HelperToken.LeerToken(principal);
                if (validToken.codigo != 1)
                {
                    return Unauthorized(new { Message = "No se pudo validar el token." });

                }
                var pintIdUsuario = Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
                var respuesta = await _iModuloBO.ObtenerModulosPorUsuario(pintIdUsuario);
                _log.LogInformation("{ObtenerModulosPorUsuario} Response: " + JsonSerializer.Serialize(respuesta));
                if (respuesta != null)
                {
                    if (respuesta.codigoRes == HttpStatusCode.OK)
                    {
                        return Ok(new { Message = respuesta.mensajeRes, data = respuesta.data });
                    }
                    return StatusCode((int)respuesta.codigoRes, new { MessageError = respuesta.mensajeRes, MessageUser = "Error. Vuelva a intentarlo." });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { MessageError = "Error interno en el servicio de listar los módulos del usuario.", MessageUser = "Error. Vuelva a intentarlo." });
                }

            }
            catch (Exception ex)
            {
                _log.LogError("{ObtenerModulosPorUsuario} Error: " + ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError, new { MessageError = "Error interno en el servicio de listar los módulos del usuario.", MessageUser = "Error al cargar. Vuelva a intentarlo." });
            }
            finally
            {
                DisposeResources();
            }
        }
    }
}
