using SistemaAlertasBackEnd.Entidades;

namespace SistemaAlertasBackEnd.Repositorios
{
    public interface IRepositorioSensor
    {
        Task Actualizar(SensorEntidad sensor);
        Task Borrar(int id);
        Task<int> Crear(SensorEntidad sensor);
        Task<bool> Existe(int id);
        Task<List<int>> Existen(List<int> ids);
        Task<SensorEntidad?> ObtenerPorId(int sensorId);
        Task<List<SensorEntidad>> ObtenerTodos();
    }
}