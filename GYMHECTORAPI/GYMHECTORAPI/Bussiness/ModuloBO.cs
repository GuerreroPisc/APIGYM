using GYMHECTORAPI.DataAccess;
using GYMHECTORAPI.Entities.Modulos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GYMHECTORAPI.Bussiness
{
    public class ModuloBO : IModuloBO
    {
        private readonly IModuloDO _moduloDO;
        private readonly ILogger<ModuloBO> _log;
        public ModuloBO(IModuloDO moduloDO, ILogger<ModuloBO> log)
        {
            _moduloDO = moduloDO;
            _log = log;
        }

        public async Task<ObtenerModulosPorUsuarioResponse> ObtenerModulosPorUsuario(int pintIdUsuario)
        {
            var response = await _moduloDO.ObtenerModulosPorUsuario(pintIdUsuario);

            if (response != null && response.Count > 0)
            {
                return new ObtenerModulosPorUsuarioResponse()
                {
                    codigoRes = HttpStatusCode.OK,
                    mensajeRes = "Módulos extraídos correctamente",
                    data = response
                };
            }
            else
            {
                return new ObtenerModulosPorUsuarioResponse
                {
                    codigoRes = HttpStatusCode.BadRequest,
                    mensajeRes = "No se pudo obtener los módulos."
                };
            }
        }
    }
}
