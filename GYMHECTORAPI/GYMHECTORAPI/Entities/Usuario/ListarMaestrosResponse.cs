namespace GYMHECTORAPI.Entities.Usuario
{
    public class ListarMaestrosResponse : GlobalResponse
    {
        public DataMaestros data { get; set; }
    }
    public class DataMaestros
    {
        public DatosUsuario datosUsuario { get; set; }
        public List<ListaHorariosRegistrados> ListaHorariosRegistrados { get; set; }
    }
    public class DatosUsuario
    {
        public int idRol { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoSuscripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DiasRestantes { get; set; }
        public bool FlgSuscripcionActiva { get; set; }
    }
    public class ListaHorariosRegistrados
    {
        public int idHorarioRegistrado { get; set; }
        public string NombreClase { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string NombreProfesor { get; set; }
    }
}
