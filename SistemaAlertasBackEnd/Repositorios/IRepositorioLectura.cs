using SistemaAlertasBackEnd.Entidades;

namespace SistemaAlertasBackEnd.Repositorios
{
    public interface IRepositorioLectura
    {
        Task<int> Crear(LecturaEntidad lectura);
        Task<LecturaEntidad> ObtenerPorIdSensor(int id);
        Task<List<LecturaEntidad>> ObtenerTodos();

        // Nuevo método para obtener la última lectura por sensor ID
        Task<LecturaEntidad?> ObtenerUltimaLecturaPorSensorId(int sensorId);
    }
}
