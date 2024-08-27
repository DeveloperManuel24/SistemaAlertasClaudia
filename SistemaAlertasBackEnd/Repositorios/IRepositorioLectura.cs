using SistemaAlertasBackEnd.Entidades;

namespace SistemaAlertasBackEnd.Repositorios
{
    public interface IRepositorioLectura
    {
  
        Task<int> Crear(LecturaEntidad lectura);
     
        
        Task<LecturaEntidad> ObtenerPorIdSensor(int id);
        Task<List<LecturaEntidad>> ObtenerTodos();
    }
}
