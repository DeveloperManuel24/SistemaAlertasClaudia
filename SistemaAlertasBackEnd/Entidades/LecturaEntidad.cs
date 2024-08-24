using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAlertasBackEnd.Entidades
{
    public class LecturaEntidad
    {
        public int ReadId { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Unity { get; set; }
        public decimal? ph_parameter { get; set; }
        public decimal? orp_parameter { get; set; }
        public decimal? turbidez_parameter { get; set; }

    }
}
