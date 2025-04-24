using GYMHECTORAPI.Entities.Modulos;
using GYMHECTORAPI.Models.GTMHECTOR.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMHECTORAPI.DataAccess
{
    public class ModuloDO : IModuloDO
    {
        private readonly GymHectorContext _context;
        private readonly ILogger<ModuloDO> _log;

        public ModuloDO(GymHectorContext context, ILogger<ModuloDO> log)
        {
            _context = context;
            _log = log;
        }

        public async Task<List<Modulos>> ObtenerModulosPorUsuario(int pintIdUsuario)
        {
            var response = new List<Modulos>();

            var listaOpciones = await _context.MpSp_ModuloRolUsuarioObtener.FromSqlRaw("EXEC MpSp_ModuloRolUsuarioObtener {0}", pintIdUsuario).ToListAsync();

            var nivelMaxSubModulos = listaOpciones.Max(x => x.intNivel);
            nivelMaxSubModulos = nivelMaxSubModulos == null ? 1 : nivelMaxSubModulos - 1;

            var listaModulosPadre = listaOpciones.Where(x => x.strCodModuloPadre == null || x.strCodModuloPadre == "").Select(x => new Modulos()
            {
                codModulo = x.strCodModulo,
                orden = x.intOrden.GetValueOrDefault(),
                dModulo = x.strNombreModulo,
                icon = x.strIcono,
                nivelMaxSubModulos = nivelMaxSubModulos.GetValueOrDefault()
            }).OrderBy(x => x.orden).ToList();

            foreach (var moduloPadre in listaModulosPadre)
            {
                var listaSubmodulos = listaOpciones.Where(x => x.strCodModuloPadre == moduloPadre.codModulo).Select(x => new Submodulo()
                {
                    codModulo = x.strCodModulo,
                    dModulo = x.strNombreModulo,
                    controller = x.strController,
                    actionName = x.strAction,
                    ruta = x.strUrl,
                    icon = x.strIcono,
                    orden = x.intOrden.GetValueOrDefault(),
                    nivel = x.intNivel.GetValueOrDefault()
                }).OrderBy(x => x.orden).ToList();

                if (listaSubmodulos != null && listaSubmodulos.Count > 0)
                {
                    moduloPadre.listaSubmodulos = listaSubmodulos;
                }
            }
            if (listaModulosPadre != null && listaModulosPadre.Count > 0)
            {
                response = listaModulosPadre;
            }
            return response;

        }
    }
}
