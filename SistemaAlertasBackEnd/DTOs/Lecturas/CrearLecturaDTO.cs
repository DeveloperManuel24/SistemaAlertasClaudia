﻿namespace SistemaAlertasBackEnd.DTOs.Lecturas
{
    public class CrearLecturaDTO
    {
       
        public int SensorId { get; set; }

        public DateTime RegisterDate { get; set; }
        public string Unity { get; set; }
        public decimal? ph_parameter { get; set; }
        public decimal? orp_parameter { get; set; }
        public decimal? turbidez_parameter { get; set; }
    }
}
