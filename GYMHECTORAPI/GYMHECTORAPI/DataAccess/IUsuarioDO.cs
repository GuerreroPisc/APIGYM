using GYMHECTORAPI.Entities.Usuario;
using GYMHECTORAPI.Models.GTMHECTOR.DB;

namespace GYMHECTORAPI.DataAccess
{
    public interface IUsuarioDO
    {
        Task<DatosUsuario> ObtenerDatosUsuario(int idUsuario);
        Task<List<HorarioRegistrosUsuario_Result>> ListarHorariosRegistrados(int idUsuario);
        Task<List<ListaHorariosGeneral_Result>> ListaHorariosGenerales(int idUsuario);
    }
}
