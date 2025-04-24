using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GYMHECTORAPI.Entities.Modulos;

namespace GYMHECTORAPI.Bussiness
{
    public interface IModuloBO
    {
        Task<ObtenerModulosPorUsuarioResponse> ObtenerModulosPorUsuario(int pintIdUsuario);
    }
}
