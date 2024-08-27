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

            // Método para crear una lectura
            public async Task<int> Crear(LecturaEntidad lectura)
            {
                context.Add(lectura);
                await context.SaveChangesAsync();
                return lectura.ReadId;
            }

            // Método para obtener un sensor por ID
            public async Task<LecturaEntidad?> ObtenerPorIdSensor(int id)
            {
                return await context.LecturaEntitys.AsNoTracking().FirstOrDefaultAsync(g => g.ReadId == id);
            }

            // Método para obtener todos los sensores
            public async Task<List<LecturaEntidad>> ObtenerTodos()
            {
                return await context.LecturaEntitys.OrderBy(a => a.ReadId).ToListAsync();
            }
        }
}
