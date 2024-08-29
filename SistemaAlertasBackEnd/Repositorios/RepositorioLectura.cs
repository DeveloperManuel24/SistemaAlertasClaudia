using Microsoft.EntityFrameworkCore;
using SistemaAlertasBackEnd.Entidades;

namespace SistemaAlertasBackEnd.Repositorios
{
    public class RepositorioLectura : IRepositorioLectura
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositorioLectura(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<int> Crear(LecturaEntidad lectura)
        {
            context.Add(lectura);
            await context.SaveChangesAsync();
            return lectura.ReadId;
        }

        public async Task<LecturaEntidad?> ObtenerPorIdSensor(int id)
        {
            return await context.LecturaEntitys.AsNoTracking().FirstOrDefaultAsync(g => g.ReadId == id);
        }

        public async Task<List<LecturaEntidad>> ObtenerTodos()
        {
            return await context.LecturaEntitys.OrderBy(a => a.ReadId).ToListAsync();
        }

        // Nuevo método para obtener la última lectura por sensor ID
        public async Task<LecturaEntidad?> ObtenerUltimaLecturaPorSensorId(int sensorId)
        {
            return await context.LecturaEntitys
                .Where(l => l.SensorId == sensorId)
                .OrderByDescending(l => l.RegisterDate)
                .FirstOrDefaultAsync();
        }
    }
}
