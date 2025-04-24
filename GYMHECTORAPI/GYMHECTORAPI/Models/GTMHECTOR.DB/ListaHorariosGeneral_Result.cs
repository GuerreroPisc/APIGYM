namespace GYMHECTORAPI.Models.GTMHECTOR.DB
{
    public class ListaHorariosGeneral_Result
    {
        public int idHorario { get; set; }
        public string nombreClase { get; set; }        
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string NombreProfesor { get; set; }
        public bool FlagDiponible { get; set; }
        public string Descripcion { get; set; }
    }
}
