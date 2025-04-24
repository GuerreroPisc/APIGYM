using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMHECTORAPI.Entities.Modulos
{
    public class ObtenerModulosPorUsuarioResponse : GlobalResponse
    {
        public List<Modulos> data { get; set; }
    }
    public class Modulos
    {
        public string codModulo { get; set; }
        public string dModulo { get; set; }
        public string icon { get; set; }
        public Nullable<int> orden { get; set; }
        public Nullable<int> nivel { get; set; }
        public int nivelMaxSubModulos { get; set; }

        public List<Submodulo> listaSubmodulos { get; set; }
    }
    public class Submodulo
    {
        public string codModulo { get; set; }
        public string dModulo { get; set; }
        public string controller { get; set; }
        public string actionName { get; set; }
        public string codModuloPadre { get; set; }
        public string ruta { get; set; }
        public string icon { get; set; }
        public Nullable<int> orden { get; set; }
        public Nullable<int> nivel { get; set; }
    }
}
