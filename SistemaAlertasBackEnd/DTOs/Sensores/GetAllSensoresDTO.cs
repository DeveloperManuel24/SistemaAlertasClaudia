using SistemaAlertasBackEnd.DTOs.Lecturas;

public class GetAllSensoresDTO
{
    public int SensorId { get; set; }
    public string Location { get; set; }
    public string Status { get; set; }

    // Cambia a LecturaEntidades para que coincida con la entidad
    public List<GetAllLecturasDTO> LecturaEntidades { get; set; } = new List<GetAllLecturasDTO>();
}
