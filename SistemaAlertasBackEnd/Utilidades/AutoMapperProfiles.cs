using AutoMapper;
using SistemaAlertasBackEnd.DTOs.Sensores;
using SistemaAlertasBackEnd.Entidades;

namespace SistemaAlertasBackEnd.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Mapeo de DTO a Sensor Entidad
            CreateMap<CrearSensorDTO, SensorEntidad>(); // De DTO a Entidad
            CreateMap<SensorEntidad, GetAllSensoresDTO>(); // De Entidad a DTO
        }
    }
}
