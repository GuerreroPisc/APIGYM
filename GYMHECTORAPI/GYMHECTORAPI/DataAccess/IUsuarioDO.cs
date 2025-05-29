using GYMHECTORAPI.Entities.Usuario;
using GYMHECTORAPI.Models.GTMHECTOR.DB;

namespace GYMHECTORAPI.DataAccess
{
    public interface IUsuarioDO
    {
        Task<DatosUsuario> ObtenerDatosUsuario(int idUsuario);
        Task<List<HorarioRegistrosUsuario_Result>> ListarHorariosRegistrados(int idUsuario);
        Task<List<ListaHorariosGeneral_Result>> ListaHorariosGenerales(int idUsuario);
        Task<RegistrarReservaResponse> registrarReserva(int idUsuarioEdita, int idHorario, int flagImpedimentos);
        Task<List<ListarPosiblesAsistencias_Result>> CapacidadHorariosIA(int idUsuario);
        Task<List<ListarAsistencias_Result>> AsistenciasIA(int idUsuario);
        Task<List<ListarReservas_Result>> ReservasIA(int idUsuario);
    }
}
