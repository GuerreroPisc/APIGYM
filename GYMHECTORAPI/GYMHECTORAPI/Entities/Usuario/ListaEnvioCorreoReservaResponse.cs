namespace GYMHECTORAPI.Entities.Usuario
{
    public class ListaEnvioCorreoReservaResponse : GlobalResponse
    {
        public DataEnvioCorreo data { get; set; }
    }

    public class DataEnvioCorreo
    {
        public List<ListaDataEnvioCorreo> listaEnvioCorreo { get; set; }
    }

    public class ListaDataEnvioCorreo
    {
        public string Nombre { get; set; }
        public string NombreCurso { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }

    }
}
