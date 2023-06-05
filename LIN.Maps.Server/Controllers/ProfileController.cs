using Http.ResponsesList;
using LIN.Maps.Server.ApiModel;
using LIN.Types.Developer.Models;
using LIN.Types.Maps.Models;
using Newtonsoft.Json;

namespace LIN.Maps.Server.Controllers;


[Route("api/profile")]
public class ProfileController : ControllerBase
{


    [HttpGet("point")]
    public async Task<HttpCreateResponse> Create([FromQuery] double longitude, [FromQuery] double latitude, [FromQuery] string token)
    {

        // Login 
        var login = await LIN.Access.Auth.Controllers.Authentication.Login(token);

        // SI no se acepto
        if (login.Response != Responses.Success)
        {
            return new(Responses.Unauthorized);
        }


        var profile = Data.Conte

        var modelo = new LIN.Types.Maps.Models.PlacePoint()
        {
            ID = 0,
            Latitude = latitude,
            Longitude = longitude,
            Time = DateTime.Now,
            Profile = new()

        }








        // Generación del uso
        var uso = new ApiKeyUsesDataModel()
        {
            Valor = 1m
        };

        // Respuesta
        var responseCobro = await LIN.Access.Developer.Controllers.ApiKey.GenerateUse(uso, key);


        if (responseCobro.Response != Responses.Success)
        {
            return new ReadAllResponse<PlaceDataModel>()
            {
                Response = Responses.Unauthorized,
                Message = responseCobro.Message
            };
        }

        // Url del servicio 
        string url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{param}.json?access_token=pk.eyJ1IjoiYWxleDIyMDkiLCJhIjoiY2xmeGVqZ2FwMHFsajNjczZlMnY0ZDFucSJ9.NGqSheAZ0xhWtEsudyEhQA&limit={limit}";


        // Ejecución
        try
        {

            // Envía la solicitud
            var response = await new HttpClient().GetAsync(url);

            // Lee la respuesta del servidor
            string responseContent = await response.Content.ReadAsStringAsync();

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



}