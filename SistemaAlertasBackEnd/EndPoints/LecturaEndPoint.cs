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
            group.MapGet("/lecturas", ObtenerTodos)
            .RequireAuthorization();
            group.MapGet("/lecturas/{id:int}", ObtenerPorId)
                .RequireAuthorization();

            return group;
        }

        // Crear ---------------------------------------------------------------------------------------
        static async Task<Results<Created<GetAllLecturasDTO>, ValidationProblem>> CrearLectura(
            CrearLecturaDTO crearLecturaDTO,
            IRepositorioLectura repositorioLectura,
            IRepositorioAlerta repositorioAlerta,
            IMapper mapper)
        {
            // Mapeo del DTO a la entidad
            var lectura = mapper.Map<LecturaEntidad>(crearLecturaDTO);

            // Creación de la lectura en el repositorio
            var id = await repositorioLectura.Crear(lectura);

            // Después de crear la lectura, verificamos si se necesita crear una alerta
            await VerificarYCrearAlerta(lectura, repositorioAlerta, mapper);

            // Mapear la entidad creada a un DTO para devolver al cliente
            var lecturaDTO = mapper.Map<GetAllLecturasDTO>(lectura);

            // Retornar resultado Created con la ubicación del nuevo recurso
            return TypedResults.Created($"/lecturas/{id}", lecturaDTO);
        }

        // Método para verificar los rangos y crear una alerta si es necesario
        private static async Task VerificarYCrearAlerta(
            LecturaEntidad ultimaLectura,
            IRepositorioAlerta repositorioAlerta,
            IMapper mapper)
        {
            // Definir los umbrales para los parámetros
            const double MinPH = 6.5;
            const double MaxPH = 8.5;
            const double MinORP = 200;
            const double MaxORP = 600;
            const double MaxTurbidez = 1.0;
            const double MinTurbidez = 0.1;

            // Inicializar variables para los mensajes de alerta
            string descripcionAlerta = "";
            string nivelAlerta = "Advertencia";

            // Condiciones para generar las alertas
            if (ultimaLectura.ph_parameter >= (decimal)MinPH && ultimaLectura.ph_parameter <= (decimal)MaxPH &&
                ultimaLectura.orp_parameter >= (decimal)MinORP && ultimaLectura.orp_parameter <= (decimal)MaxORP &&
                ultimaLectura.turbidez_parameter > (decimal)MaxTurbidez)
            {
                descripcionAlerta = "Advertencia: El nivel de turbidez es elevado.";
            }
            else if (ultimaLectura.ph_parameter >= (decimal)MinPH && ultimaLectura.ph_parameter <= (decimal)MaxPH &&
                     ultimaLectura.orp_parameter >= (decimal)MinORP && ultimaLectura.orp_parameter <= (decimal)MaxORP &&
                     ultimaLectura.turbidez_parameter < (decimal)MinTurbidez)
            {
                descripcionAlerta = "Advertencia: El nivel de turbidez es bajo.";
            }
            else if (ultimaLectura.ph_parameter >= (decimal)MinPH && ultimaLectura.ph_parameter <= (decimal)MaxPH &&
                     ultimaLectura.turbidez_parameter <= (decimal)MaxTurbidez && ultimaLectura.turbidez_parameter >= (decimal)MinTurbidez &&
                     (ultimaLectura.orp_parameter < (decimal)MinORP || ultimaLectura.orp_parameter > (decimal)MaxORP))
            {
                descripcionAlerta = "Alerta: El nivel de ORP es " +
                                    (ultimaLectura.orp_parameter < (decimal)MinORP ? "bajo." : "elevado.");
                nivelAlerta = "Alerta";
            }
            else if (ultimaLectura.orp_parameter >= (decimal)MinORP && ultimaLectura.orp_parameter <= (decimal)MaxORP &&
                     ultimaLectura.turbidez_parameter <= (decimal)MaxTurbidez && ultimaLectura.turbidez_parameter >= (decimal)MinTurbidez &&
                     (ultimaLectura.ph_parameter < (decimal)MinPH || ultimaLectura.ph_parameter > (decimal)MaxPH))
            {
                descripcionAlerta = "Alerta: El nivel de pH es " +
                                    (ultimaLectura.ph_parameter < (decimal)MinPH ? "bajo." : "alto.");
                nivelAlerta = "Alerta";
            }



            // Los que estan abajo o arriba del nivel seguro 
            else if ((ultimaLectura.ph_parameter < (decimal)MinPH) &&
                     (ultimaLectura.orp_parameter < (decimal)MinORP) &&
                     ultimaLectura.turbidez_parameter < (decimal)MinTurbidez)
            {
                descripcionAlerta = "Alerta: Los niveles de pH, ORP y Turbidez están abajo de los rangos seguros.";
                nivelAlerta = "Alerta";
            }
            // ph y orp bajos turbidez normal
            else if ((ultimaLectura.ph_parameter < (decimal)MinPH) &&
                    (ultimaLectura.orp_parameter < (decimal)MinORP) &&
                    ultimaLectura.turbidez_parameter >= (decimal)MinTurbidez)
            {
                descripcionAlerta = "Alerta: Los niveles de pH, ORP  están abajo de los rangos seguros.";
                nivelAlerta = "Alerta";
            }


            else if (( ultimaLectura.ph_parameter > (decimal)MaxPH) &&
                     (ultimaLectura.orp_parameter > (decimal)MaxORP) &&
                     ultimaLectura.turbidez_parameter > (decimal)MaxTurbidez)
            {
                descripcionAlerta = "Alerta: Los niveles de pH, ORP y Turbidez están arriba de los rangos seguros.";
                nivelAlerta = "Alerta";
            }

            // Si se ha generado una descripción para la alerta, crear la alerta
            if (!string.IsNullOrEmpty(descripcionAlerta))
            {
                var nuevaAlerta = new AlertaEntidad
                {
                    Type = "Alerta de Calidad del Agua",
                    Description = descripcionAlerta,
                    RegisterDate = DateTime.UtcNow,
                    Level = nivelAlerta,
                    SensorId = ultimaLectura.SensorId
                };

                await repositorioAlerta.Crear(nuevaAlerta);
            }
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
