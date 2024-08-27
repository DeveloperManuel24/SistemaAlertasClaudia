using SistemaAlertasBackEnd.Migrations;

namespace SistemaAlertasBackEnd.Entidades
{
    public class AlertaEntidad
    {
        public int AlertaId { get; set; }

        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Level { get; set; }

        // Clave foránea para Sensor
        public int SensorId { get; set; }
    }
}
