using GYMHECTORAPI.Entities.Usuario;

namespace GYMHECTORAPI.Bussiness
{
    public interface IUsuarioBO
    {
        Task<ListarMaestrosResponse> ListarMaestros(int idUsuario);
        Task<ListarHorariosGenerales> ListarHorariosGenerales(int idUsuario);
    }
}
