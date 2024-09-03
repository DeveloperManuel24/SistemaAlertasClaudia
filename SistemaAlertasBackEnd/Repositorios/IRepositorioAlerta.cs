    using SistemaAlertasBackEnd.Entidades;

    namespace SistemaAlertasBackEnd.Repositorios
    {
        public interface IRepositorioAlerta
        {
            Task<AlertaEntidad> ObtenerPorId(int id);
            Task<List<AlertaEntidad>> ObtenerTodos();

            // Nuevo método para crear una alerta
            Task<int> Crear(AlertaEntidad alerta);
        }
    }
