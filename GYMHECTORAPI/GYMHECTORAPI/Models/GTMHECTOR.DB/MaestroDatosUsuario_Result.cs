namespace GYMHECTORAPI.Models.GTMHECTOR.DB
{
    public class MaestroDatosUsuario_Result
    {
        public int idRol { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoSuscripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DiasRestantes { get; set; }
        public bool FlgSuscripcionActiva { get; set; }
    }
}
