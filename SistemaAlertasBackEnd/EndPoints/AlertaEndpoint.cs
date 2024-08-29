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
            group.MapGet("/alertas", ObtenerTodos);
            group.MapGet("/alertas/{id:int}", ObtenerPorId);
            group.MapPost("/alertas/{sensorId:int}", CrearAlerta);

            return group;
        }

        // Crear Alerta ---------------------------------------------------------------------------------------
        static async Task<Results<Created<GetAllAlertasDTO>, ValidationProblem>> CrearAlerta(
            int sensorId,
            IRepositorioLectura repositorioLectura,
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

            // Obtener la última lectura para el sensor dado
            var ultimaLectura = await repositorioLectura.ObtenerUltimaLecturaPorSensorId(sensorId);

            if (ultimaLectura == null)
            {
                return TypedResults.ValidationProblem(new Dictionary<string, string[]> { { "Error", new[] { "No se encontraron lecturas para el sensor especificado." } } });
            }

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
            //Turbidez
            else if (ultimaLectura.ph_parameter >= (decimal)MinPH && ultimaLectura.ph_parameter <= (decimal)MaxPH &&
                     ultimaLectura.orp_parameter >= (decimal)MinORP && ultimaLectura.orp_parameter <= (decimal)MaxORP &&
                     ultimaLectura.turbidez_parameter < (decimal)MaxTurbidez)
            {
                descripcionAlerta = "Advertencia: El nivel de turbidez es bajo.";
            }
            //ORP--------------------------------------------------------------------------
            else if (ultimaLectura.ph_parameter >= (decimal)MinPH && ultimaLectura.ph_parameter <= (decimal)MaxPH &&
                     ultimaLectura.turbidez_parameter <= (decimal)MaxTurbidez && ultimaLectura.turbidez_parameter >= (decimal)MinTurbidez &&
                     (ultimaLectura.orp_parameter < (decimal)MinORP || ultimaLectura.orp_parameter > (decimal)MaxORP))
            {
                descripcionAlerta = "Alerta: El nivel de ORP es " +
                                    (ultimaLectura.orp_parameter < (decimal)MinORP ? "bajo." : "elevado.");
                nivelAlerta = "Alerta";
            }
            //PH
            else if (ultimaLectura.orp_parameter >= (decimal)MinORP && ultimaLectura.orp_parameter <= (decimal)MaxORP &&
                     ultimaLectura.turbidez_parameter <= (decimal)MaxTurbidez && ultimaLectura.turbidez_parameter >= (decimal)MinTurbidez && 
                     (ultimaLectura.ph_parameter < (decimal)MinPH || ultimaLectura.ph_parameter > (decimal)MaxPH))
            {
                descripcionAlerta = "Alerta: El nivel de pH es " +
                                    (ultimaLectura.ph_parameter < (decimal)MinPH ? "bajo." : "alto.");
                nivelAlerta = "Alerta";
            }
            else if ((ultimaLectura.ph_parameter < (decimal)MinPH || ultimaLectura.ph_parameter > (decimal)MaxPH) &&
                     (ultimaLectura.orp_parameter < (decimal)MinORP || ultimaLectura.orp_parameter > (decimal)MaxORP) &&
                     ultimaLectura.turbidez_parameter > (decimal)MaxTurbidez)
            {
                descripcionAlerta = "Alerta: Los niveles de pH, ORP y Turbidez están fuera de los rangos seguros.";
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
                    SensorId = sensorId
                };

                var id = await repositorioAlerta.Crear(nuevaAlerta);

                var alertaDTO = mapper.Map<GetAllAlertasDTO>(nuevaAlerta);
                return TypedResults.Created($"/alertas/{id}", alertaDTO);
            }

            // Si no se generó una alerta, retornar una advertencia genérica
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> { { "Error", new[] { "Los parámetros están dentro de los rangos seguros." } } });
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
