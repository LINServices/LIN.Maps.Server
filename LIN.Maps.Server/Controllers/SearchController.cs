using Http.ResponsesList;
using LIN.Maps.Server.Services;
using LIN.Types.Maps.Models;

namespace LIN.Maps.Server.Controllers;

[Route("api/[controller]")]
public class SearchController(MapboxService mapboxService) : ControllerBase
{

    /// <summary>
    /// Buscar lugares.
    /// </summary>
    /// <param name="param">Parámetro de búsqueda.</param>
    /// <param name="limit">Limite.</param>
    /// <param name="key">Llave.</param>
    /// <returns></returns>
    [HttpGet("search")]
    public async Task<HttpReadAllResponse<PlaceDataModel>> Create([FromQuery] string param, [FromQuery] int limit, [FromQuery] string key)
    {

        // Generar cobro.
        // -------------Realizar acción ---------------

        // Validar cobro.
        // -------------Realizar acción ---------------

        // Encontrar lugares.
        var places = await mapboxService.Search(param, limit);

        // Respuesta.
        return new ReadAllResponse<PlaceDataModel>(Responses.Success, places);

    }


    /// <summary>
    /// Buscar lugares alrededor.
    /// </summary>
    /// <param name="param">Parámetro de búsqueda.</param>
    /// <param name="limit">Limite de lugares.</param>
    /// <param name="latitud">Latitud.</param>
    /// <param name="longitud">Longitud.</param>
    /// <param name="key">Api key.</param>
    [HttpGet("search/around")]
    public async Task<HttpReadAllResponse<PlaceDataModel>> Around([FromQuery] string param, [FromQuery] int limit, [FromQuery] string latitud, [FromQuery] string longitud, [FromQuery] string key)
    {

        // Generar cobro.
        // -------------Realizar acción ---------------

        // Validar cobro.
        // -------------Realizar acción ---------------

        // Encontrar lugares.
        var places = await mapboxService.Search(param, limit, latitud, longitud);

        // Respuesta.
        return new ReadAllResponse<PlaceDataModel>(Responses.Success, places);

    }

}