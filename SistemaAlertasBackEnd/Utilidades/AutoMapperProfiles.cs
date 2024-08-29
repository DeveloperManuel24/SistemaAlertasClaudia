using AutoMapper;
using SistemaAlertasBackEnd.DTOs.Alertas;
using SistemaAlertasBackEnd.DTOs.Lecturas;
using SistemaAlertasBackEnd.DTOs.Sensores;
using SistemaAlertasBackEnd.Entidades;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // Mapeo de DTO a Sensor Entidad
        CreateMap<CrearSensorDTO, SensorEntidad>(); // De DTO a Entidad

        CreateMap<SensorEntidad, GetAllSensoresDTO>()
            .ForMember(dest => dest.LecturaEntidades, opt => opt.MapFrom(src => src.LecturaEntidades));

        // Mapeo de DTO a Lectura Entidad
        CreateMap<CrearLecturaDTO, LecturaEntidad>(); // De DTO a Entidad

        CreateMap<LecturaEntidad, GetAllLecturasDTO>(); // De Entidad a DTO

        // Mapeo de DTO a Lectura Entidad


        CreateMap<AlertaEntidad, GetAllAlertasDTO>(); // De Entidad a DTO
    }
}
