using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SistemaAlertasBackEnd.DTOs.Lecturas;
using SistemaAlertasBackEnd.DTOs.Sensores;
using SistemaAlertasBackEnd.Entidades;
using SistemaAlertasBackEnd.Repositorios;

namespace SistemaAlertasBackEnd.EndPoints
{
    public static class SensorEndpoint
    {
        public static RouteGroupBuilder MapSensores(this RouteGroupBuilder group)
        {
            group.MapPost("/sensores", CrearSensor);
            group.MapGet("/sensores", ObtenerTodos);
            group.MapGet("/sensores/{id:int}", ObtenerPorId);
            group.MapPut("/sensores/{id:int}", ActualizarSensor);
            group.MapDelete("/sensores/{id:int}", BorrarSensor);

            return group;
        }

        // Crear ---------------------------------------------------------------------------------------
        static async Task<Results<Created<GetAllSensoresDTO>, ValidationProblem>> CrearSensor(
            CrearSensorDTO crearSensorDTO,
            IRepositorioSensor repositorioSensor,
            IMapper mapper)
        {
            // Mapeo del DTO a la entidad
            var sensor = mapper.Map<SensorEntidad>(crearSensorDTO);

            // Lógica de negocio adicional puede ir aquí

            // Creación del sensor en el repositorio
            var id = await repositorioSensor.Crear(sensor);

            // Mapear la entidad creada a un DTO para devolver al cliente
            var sensorDTO = mapper.Map<GetAllSensoresDTO>(sensor);

            // Retornar resultado Created con la ubicación del nuevo recurso
            return TypedResults.Created($"/sensores/{id}", sensorDTO);
        }

        // Obtener todos ---------------------------------------------------------------------------------------
        static async Task<Ok<List<GetAllSensoresDTO>>> ObtenerTodos(
            IRepositorioSensor repositorioSensor,
            IMapper mapper)
        {
            var sensores = await repositorioSensor.ObtenerTodos();
            var sensoresDTO = mapper.Map<List<GetAllSensoresDTO>>(sensores);
            return TypedResults.Ok(sensoresDTO);
        }

        // Obtener por ID ---------------------------------------------------------------------------------------
        static async Task<Results<Ok<GetAllSensoresDTO>, NotFound>> ObtenerPorId(
                                                         int id,
                                                         IRepositorioSensor repositorioSensor,
                                                         IMapper mapper)
        {
            var sensor = await repositorioSensor.ObtenerPorId(id);

            if (sensor is null)
            {
                return TypedResults.NotFound();
            }

            // Mapear usando AutoMapper
            var sensorDTO = mapper.Map<GetAllSensoresDTO>(sensor);

            return TypedResults.Ok(sensorDTO);
        }



        // Actualizar ---------------------------------------------------------------------------------------
        static async Task<Results<NoContent, NotFound>> ActualizarSensor(
            int id,
            CrearSensorDTO crearSensorDTO,
            IRepositorioSensor repositorioSensor,
            IMapper mapper)
        {
            var sensorDB = await repositorioSensor.ObtenerPorId(id);

            if (sensorDB is null)
            {
                return TypedResults.NotFound();
            }

            var sensorParaActualizar = mapper.Map<SensorEntidad>(crearSensorDTO);
            sensorParaActualizar.SensorId = id;

            await repositorioSensor.Actualizar(sensorParaActualizar);
            return TypedResults.NoContent();
        }

        // Borrar ---------------------------------------------------------------------------------------
        static async Task<Results<NoContent, NotFound>> BorrarSensor(
            int id,
            IRepositorioSensor repositorioSensor)
        {
            var sensorDB = await repositorioSensor.ObtenerPorId(id);

            if (sensorDB is null)
            {
                return TypedResults.NotFound();
            }

            await repositorioSensor.Borrar(id);
            return TypedResults.NoContent();
        }
    }
}
