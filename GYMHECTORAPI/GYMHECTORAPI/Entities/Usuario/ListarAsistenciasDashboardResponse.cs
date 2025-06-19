namespace GYMHECTORAPI.Entities.Usuario
{
    public class ListarAsistenciasDashboardResponse : GlobalResponse
    {
        public DataAsistencia data { get; set; }
    }

    public class DataAsistencia
    {
        public List<ListaAsistenciaGeneral> listaAsistencia { get; set; }
    }

    public class ListaAsistenciaGeneral
    {
        public string FechaAsistencia { get; set; }

    }
}
