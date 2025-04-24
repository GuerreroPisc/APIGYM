namespace GYMHECTORAPI.Entities.Usuario
{
    public class ListarHorariosGenerales : GlobalResponse
    {
        public DataHorarios data { get; set; }
    }

    public class DataHorarios
    {
        public List<ListaHorariosGeneral> ListaHorarios { get; set; }
    }
    public class ListaHorariosGeneral
    {
        public string nombreClase { get; set; }
        public List<ListaHorariosClase> horariosClase { get; set; }
    }
    public class ListaHorariosClase
    {
        public int idHorario { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string NombreProfesor { get; set; }
        public bool FlagDiponible { get; set; }
        public string Descripcion { get; set; }
    }
}
