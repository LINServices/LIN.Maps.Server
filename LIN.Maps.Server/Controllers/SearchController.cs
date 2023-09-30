using Http.ResponsesList;
using LIN.Types.Developer.Models;
using LIN.Types.Responses;
using Newtonsoft.Json;
using LIN.Types.Maps.Models;
using LIN.Maps.Server.ApiModel;

namespace LIN.Maps.Server.Controllers;


[Route("api/search")]
public class SearchController : ControllerBase
{




    [HttpGet("search")]
    public async Task<HttpReadAllResponse<PlaceDataModel>> Create([FromQuery] string param, [FromQuery] int limit, [FromQuery] string key)
    {


        // Generación del uso
        var uso = new ApiKeyUsesDataModel()
        {
            Valor = 1m
        };

        // Respuesta
        var responseCobro = await Access.Developer.Controllers.ApiKey.GenerateUse(uso, key);


        if (responseCobro.Response != Responses.Success)
        {
            return new ReadAllResponse<PlaceDataModel>()
            {
                Response = Responses.Unauthorized,
                Message = responseCobro.Message
            };
        }

        // Url del servicio 
        var url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{param}.json?access_token=pk.eyJ1IjoiYWxleDIyMDkiLCJhIjoiY2xmeGVqZ2FwMHFsajNjczZlMnY0ZDFucSJ9.NGqSheAZ0xhWtEsudyEhQA&limit={limit}";


        // Ejecución
        try
        {

            // Envía la solicitud
            var response = await new HttpClient().GetAsync(url);

            // Lee la respuesta del servidor
            var responseContent = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<MapboxGeocodingResponse>(responseContent) ?? new();



            var places = new List<PlaceDataModel>();


            foreach (var feature in obj.Features)
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
                catch
                {

                }

            }

            return new ReadAllResponse<PlaceDataModel>(Responses.Success, places ?? new());

        }
        catch
        {
        }

        return new(Responses.UnavailableService);


    }



    [HttpGet("search/around")]
    public async Task<HttpReadAllResponse<PlaceDataModel>> Around([FromQuery] string param, [FromQuery] int limit, [FromQuery] string latitud, [FromQuery] string longitud, [FromQuery] string key)
    {

        // Generación del uso
        var uso = new ApiKeyUsesDataModel()
        {
            Valor = 1m
        };

        // Respuesta
        var responseCobro = await Access.Developer.Controllers.ApiKey.GenerateUse(uso, key);


        if (responseCobro.Response != Responses.Success)
        {
            return new ReadAllResponse<PlaceDataModel>()
            {
                Response = Responses.Unauthorized,
                Message = responseCobro.Message
            };
        }



        longitud = longitud.Replace(".", ",");
        latitud = latitud.Replace(".", ",");
        var coor = BoundingBox.CreateBoundingBox(double.Parse(longitud), double.Parse(latitud), 10);

        // Url del servicio 
        var url =
            $"https://api.mapbox.com/geocoding/v5/mapbox.places/{param}.json?access_token=pk.eyJ1IjoiYWxleDIyMDkiLCJhIjoiY2xmeGVqZ2FwMHFsajNjczZlMnY0ZDFucSJ9.NGqSheAZ0xhWtEsudyEhQA&bbox={coor.MinX.ToString().Replace(',', '.')},{coor.MinY.ToString().Replace(',', '.')},{coor.MaxX.ToString().Replace(',', '.')},{coor.MaxY.ToString().Replace(',', '.')}&limit={limit}";


        // Ejecucion
        try
        {

            // Envia la solicitud
            var response = await new HttpClient().GetAsync(url);

            // Lee la respuesta del servidor
            var responseContent = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<MapboxGeocodingResponse>(responseContent) ?? new();



            var places = new List<PlaceDataModel>();


            foreach (var feature in obj.Features)
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
                catch
                {

                }

            }


            return new ReadAllResponse<PlaceDataModel>(Responses.Success, places ?? new());


        }
        catch
        {
        }

        return new();


    }






}