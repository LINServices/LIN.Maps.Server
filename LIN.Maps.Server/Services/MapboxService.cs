using LIN.Maps.Server.ApiModel;
using LIN.Types.Maps.Models;
using System.Globalization;

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
    public const string MapboxGeocodingUrl = "https://api.mapbox.com/search/geocode/v6/forward";


    /// <summary>
    /// Buscar de forma general.
    /// </summary>
    /// <param name="param">Parámetro de búsqueda.</param>
    /// <param name="limit">Limite.</param>
    public async Task<IEnumerable<PlaceDataModel>> Search(string param, int limit)
    {

        // Obtener cliente.
        var client = new Global.Http.Services.Client(MapboxGeocodingUrl);

        // Parámetros.
        client.AddParameter("access_token", ApiKey);
        client.AddParameter("limit", limit);
        client.AddParameter("q", param);

        // Obtener respuesta.
        var searchResult = await client.Get<RootObject>();

        // Lista de lugares encontrados.
        var places = new List<PlaceDataModel>();

        // Mapear objetos.
        foreach (var feature in searchResult.Features)
        {
            try
            {
                var place = new PlaceDataModel()
                {
                    Text = feature.Properties.FullAddress,
                    Nombre = feature.Properties.NamePreferred,
                    Longitud = feature.Properties.Coordinates.Longitude.ToString().Replace(',', '.'),
                    Latitud = feature.Properties.Coordinates.Latitude.ToString().Replace(',', '.')
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
        var coordinates = BoundingBox.CreateBoundingBox(double.Parse(longitude, CultureInfo.InvariantCulture), double.Parse(latitude, CultureInfo.InvariantCulture), 10);

        // Parámetros.
        client.AddParameter("access_token", ApiKey);
        client.AddParameter("limit", limit);
        client.AddParameter("bbox", $"{coordinates.MinX.ToString().Replace(',', '.')},{coordinates.MinY.ToString().Replace(',', '.')},{coordinates.MaxX.ToString().Replace(',', '.')},{coordinates.MaxY.ToString().Replace(',', '.')}");

        // Obtener respuesta.
        var searchResult = await client.Get<RootObject>();

        // Lista de lugares encontrados.
        var places = new List<PlaceDataModel>();

        // Mapear objetos.
        foreach (var feature in searchResult.Features)
        {
            try
            {
                var place = new PlaceDataModel()
                {
                    Text = feature.Properties.FullAddress,
                    Nombre = feature.Properties.NamePreferred,
                    Longitud = feature.Properties.Coordinates.Longitude.ToString().Replace(',', '.'),
                    Latitud = feature.Properties.Coordinates.Latitude.ToString().Replace(',', '.')
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