using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SistemaAlertasBackEnd.DTOs.Alertas;
using SistemaAlertasBackEnd.Entidades;
using SistemaAlertasBackEnd.Repositorios;

namespace SistemaAlertasBackEnd.EndPoints
{
    public static class AlertaEndpoint
    {
        public static RouteGroupBuilder MapAlertas(this RouteGroupBuilder group)
        {
            group.MapGet("/alertas", ObtenerTodos)
            .RequireAuthorization();
            group.MapGet("/alertas/{id:int}", ObtenerPorId)
            .RequireAuthorization();


            return group;
        }

       
        // Obtener todos ---------------------------------------------------------------------------------------
        static async Task<Ok<List<GetAllAlertasDTO>>> ObtenerTodos(
             IRepositorioAlerta repositorioAlerta,
            IMapper mapper)
        {
            var alertas = await repositorioAlerta.ObtenerTodos();
            var alertasDTO = mapper.Map<List<GetAllAlertasDTO>>(alertas);
            return TypedResults.Ok(alertasDTO);
        }

        // Obtener por ID ---------------------------------------------------------------------------------------
        static async Task<Results<Ok<GetAllAlertasDTO>, NotFound>> ObtenerPorId(
            int id,
            IRepositorioAlerta repositorioAlerta,
            IMapper mapper)
        {
            var alerta = await repositorioAlerta.ObtenerPorId(id);

            if (alerta is null)
            {
                return TypedResults.NotFound();
            }

            var alertaDTO = mapper.Map<GetAllAlertasDTO>(alerta);
            return TypedResults.Ok(alertaDTO);
        }
    }
}
