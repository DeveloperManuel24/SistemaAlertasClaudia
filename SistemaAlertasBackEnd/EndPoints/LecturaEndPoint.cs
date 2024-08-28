using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SistemaAlertasBackEnd.DTOs.Lecturas;
using SistemaAlertasBackEnd.DTOs.Sensores;
using SistemaAlertasBackEnd.Entidades;
using SistemaAlertasBackEnd.Repositorios;

namespace SistemaAlertasBackEnd.EndPoints
{
    public static class LecturaEndPoint
    {
        public static RouteGroupBuilder MapLecturas(this RouteGroupBuilder group)
        {
            group.MapPost("/lecturas", CrearLectura);
            group.MapGet("/lecturas", ObtenerTodos);
            group.MapGet("/lecturas/{id:int}", ObtenerPorId);
          

            return group;
        }

        // Crear ---------------------------------------------------------------------------------------
        static async Task<Results<Created<GetAllLecturasDTO>, ValidationProblem>> CrearLectura(
            CrearLecturaDTO crearLecturaDTO,
            IRepositorioLectura repositorioLectura,
            IMapper mapper)
        {
            // Mapeo del DTO a la entidad
            var lectura = mapper.Map<LecturaEntidad>(crearLecturaDTO);

            // Creación de la lectura en el repositorio
            var id = await repositorioLectura.Crear(lectura);

            // Mapear la entidad creada a un DTO para devolver al cliente
            var lecturaDTO = mapper.Map<GetAllLecturasDTO>(lectura);

            // Retornar resultado Created con la ubicación del nuevo recurso
            return TypedResults.Created($"/lecturas/{id}", lecturaDTO);
        }

        // Obtener todos ---------------------------------------------------------------------------------------
        static async Task<Ok<List<GetAllLecturasDTO>>> ObtenerTodos(
             IRepositorioLectura repositorioLectura,
            IMapper mapper)
        {
            var lecturas = await repositorioLectura.ObtenerTodos();
            var lecturasDTO = mapper.Map<List<GetAllLecturasDTO>>(lecturas);
            return TypedResults.Ok(lecturasDTO);
        }

        // Obtener por ID ---------------------------------------------------------------------------------------
        static async Task<Results<Ok<GetAllLecturasDTO>, NotFound>> ObtenerPorId(
            int id,
             IRepositorioLectura repositorioLectura,
            IMapper mapper)
        {
            var lectura = await repositorioLectura.ObtenerPorIdSensor(id);

            if (lectura is null)
            {
                return TypedResults.NotFound();
            }

            var lecturaDTO = mapper.Map<GetAllLecturasDTO>(lectura);
            return TypedResults.Ok(lecturaDTO);
        }

    }
}
