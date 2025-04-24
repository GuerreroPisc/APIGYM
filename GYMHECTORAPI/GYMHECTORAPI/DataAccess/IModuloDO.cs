using GYMHECTORAPI.Entities.Modulos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMHECTORAPI.DataAccess
{
    public interface IModuloDO
    {
        Task<List<Modulos>> ObtenerModulosPorUsuario(int pintIdUsuario);
    }
}
