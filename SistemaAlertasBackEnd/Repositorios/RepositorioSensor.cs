using Microsoft.EntityFrameworkCore;
using SistemaAlertasBackEnd.DTOs;
using SistemaAlertasBackEnd.Entidades;
using SistemaAlertasBackEnd.Migrations;

namespace SistemaAlertasBackEnd.Repositorios
{
    public class RepositorioSensor : IRepositorioSensor
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositorioSensor(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }

        // Método para crear un sensor
        public async Task<int> Crear(SensorEntidad sensor)
        {
            context.Add(sensor);
            await context.SaveChangesAsync();
            return sensor.SensorId;
        }
        public async Task<SensorEntidad?> ObtenerPorId(int sensorId)
        {
            return await context.SensorEntitys
                .Include(s => s.LecturaEntidades)  // Asegúrate de que las lecturas están incluidas
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SensorId == sensorId);
        }




        // Método para obtener todos los sensores
        public async Task<List<SensorEntidad>> ObtenerTodos()
        {
            return await context.SensorEntitys.OrderBy(a => a.SensorId).ToListAsync();
        }

        // Método para verificar si un sensor existe por ID
        public async Task<bool> Existe(int id)
        {
            return await context.SensorEntitys.AnyAsync(x => x.SensorId == id);
        }

        // Método para verificar si un conjunto de sensores existe por sus IDs
        public async Task<List<int>> Existen(List<int> ids)
        {
            return await context.SensorEntitys.Where(a => ids.Contains(a.SensorId)).Select(a => a.SensorId).ToListAsync();
        }

        // Método para actualizar un sensor
        public async Task Actualizar(SensorEntidad sensor)
        {
            context.Update(sensor);
            await context.SaveChangesAsync();
        }

        // Método para borrar un sensor por ID
        public async Task Borrar(int id)
        {
            await context.SensorEntitys.Where(x => x.SensorId == id).ExecuteDeleteAsync();
        }

       
    }
}
