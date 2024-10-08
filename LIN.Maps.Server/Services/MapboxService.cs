using LIN.Maps.Server.ApiModel;
using LIN.Types.Maps.Models;

namespace LIN.Maps.Server.Services;

public class MapboxService
{

    /// <summary>
    /// Api key.
    /// </summary>
    internal static string ApiKey { get; set; } = string.Empty;


    /// <summary>
    /// Url del servicio.
    /// </summary>
    public const string MapboxGeocodingUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places/{0}.json";


    /// <summary>
    /// Buscar de forma general.
    /// </summary>
    /// <param name="param">Parámetro de búsqueda.</param>
    /// <param name="limit">Limite.</param>
    public async Task<IEnumerable<PlaceDataModel>> Search(string param, int limit)
    {

        // Obtener cliente.
        var client = new Global.Http.Services.Client(string.Format(MapboxGeocodingUrl, param));

        // Parámetros.
        client.AddParameter("access_token", ApiKey);
        client.AddParameter("limit", limit);

        // Obtener respuesta.
        var searchResult = await client.Get<MapboxGeocodingResponse>();

        // Lista de lugares encontrados.
        var places = new List<PlaceDataModel>();

        // Mapear objetos.
        foreach (var feature in searchResult.Features)
        {
            try
            {
                var place = new PlaceDataModel()
                {
                    Text = feature.Text,
                    Nombre = feature.PlaceName,
                    Longitud = feature.Center[0].ToString().Replace(',', '.'),
                    Latitud = feature.Center[1].ToString().Replace(',', '.')
                };
                places.Add(place);
            }
            catch (Exception)
            {
            }
        }

        return places;

    }


    /// <summary>
    /// Buscar de forma general.
    /// </summary>
    /// <param name="param">Parámetro de búsqueda.</param>
    /// <param name="limit">Limite.</param>
    public async Task<IEnumerable<PlaceDataModel>> Search(string param, int limit, string latitude, string longitude)
    {

        // Obtener cliente.
        var client = new Global.Http.Services.Client(string.Format(MapboxGeocodingUrl, param));

        // Obtener coordenadas.
        var coordinates = BoundingBox.CreateBoundingBox(double.Parse(longitude), double.Parse(latitude), 10);

        // Parámetros.
        client.AddParameter("access_token", ApiKey);
        client.AddParameter("limit", limit);
        client.AddParameter("bbox", $"{coordinates.MinX.ToString().Replace(',', '.')},{coordinates.MinY.ToString().Replace(',', '.')},{coordinates.MaxX.ToString().Replace(',', '.')},{coordinates.MaxY.ToString().Replace(',', '.')}");

        // Obtener respuesta.
        var searchResult = await client.Get<MapboxGeocodingResponse>();

        // Lista de lugares encontrados.
        var places = new List<PlaceDataModel>();

        // Mapear objetos.
        foreach (var feature in searchResult.Features)
        {
            try
            {
                var place = new PlaceDataModel()
                {
                    Text = feature.Text,
                    Nombre = feature.PlaceName,
                    Longitud = feature.Center[0].ToString().Replace(',', '.'),
                    Latitud = feature.Center[1].ToString().Replace(',', '.')
                };
                places.Add(place);
            }
            catch (Exception)
            {
            }
        }
        return places;
    }

}