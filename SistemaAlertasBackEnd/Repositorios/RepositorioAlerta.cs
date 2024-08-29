using Microsoft.EntityFrameworkCore;
using SistemaAlertasBackEnd.Entidades;

namespace SistemaAlertasBackEnd.Repositorios
{
    public class RepositorioAlerta : IRepositorioAlerta
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositorioAlerta(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<AlertaEntidad?> ObtenerPorId(int id)
        {
            return await context.AlertaEntitys.AsNoTracking().FirstOrDefaultAsync(g => g.AlertaId == id);
        }

        public async Task<List<AlertaEntidad>> ObtenerTodos()
        {
            return await context.AlertaEntitys.OrderBy(a => a.AlertaId).ToListAsync();
        }

        // Implementación del método para crear una alerta
        public async Task<int> Crear(AlertaEntidad alerta)
        {
            context.Add(alerta);
            await context.SaveChangesAsync();
            return alerta.AlertaId;
        }
    }
}
