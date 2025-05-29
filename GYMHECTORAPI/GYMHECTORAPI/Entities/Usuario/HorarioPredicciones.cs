namespace GYMHECTORAPI.Entities.Usuario
{
    public class HorarioPredicciones : GlobalResponse
    {
        public DataHorarioPredicciones data { get; set; }
    }

    public class DataHorarioPredicciones
    {
        public List<ListaHorarioPredicciones> ListaEstimaciones { get; set; }
    }
    public class ListaHorarioPredicciones
    {
        public string fecha { get; set; }
        public List<Estimaciones> estimaciones { get; set; }
    }
    public class Estimaciones
    {
        public string rango { get; set; }
        public int asistentesPromedio { get; set; }
    }
}
