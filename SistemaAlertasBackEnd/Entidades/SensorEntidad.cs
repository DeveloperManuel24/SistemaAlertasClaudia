namespace SistemaAlertasBackEnd.Entidades
{
    public class SensorEntidad
    {

        public int SensorId { get; set; }
        public string NombreSensor { get; set; }
        public String Location { get; set; }
        public String Status { get; set; }

        ////Un uno a muchos, sensor con lectura
        public List<LecturaEntidad> LecturaEntidades { get; set; } = new List<LecturaEntidad>();

        ////Un uno a muchos, sensor con Alerta
        public List<AlertaEntidad> AlertaEntidades { get; set; } = new List<AlertaEntidad>();
    }
}
