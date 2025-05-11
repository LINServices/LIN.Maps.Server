using Http.ResponsesList;

namespace LIN.Maps.Server.Controllers;

[Route("api/profile")]
public class ProfileController : ControllerBase
{

    /// <summary>
    /// Guardar punto de ubicación.
    /// </summary>
    /// <param name="longitude">Longitude.</param>
    /// <param name="latitude">Latitud.</param>
    /// <param name="token">Token de acceso.</param>
    [HttpPost("point")]
    public async Task<HttpCreateResponse> Create([FromQuery] double longitude, [FromQuery] double latitude, [FromQuery] string token)
    {
        // Autenticarse en LIN Identity.
        var authentication = await Access.Auth.Controllers.Authentication.Login(token);

        // SI no se acepto
        if (authentication.Response != Responses.Success)
            return new(Responses.Unauthorized);

        // Buscar el perfil de mapas.
        var profile = await Data.Profiles.ReadByAccount(authentication.Model.Id);

        switch (profile.Response)
        {
            case Responses.Success:
                break;
            case Responses.NotExistProfile:
                var profileModel = new Types.Maps.Models.ProfileModel()
                {
                    AccountID = authentication.Model.Id,
                    ID = 0,
                    PlacesPoint = []
                };

                // Crear el perfil.
                var profCreate = await Data.Profiles.Create(profileModel);

                if (profCreate.Response != Responses.Success)
                    return new(Responses.Unauthorized);
                
                profile.Model = new()
                {
                    ID = profCreate.LastId
                };
                break;
            default:
                return new(Responses.Unauthorized);
        }

        // Crear el punto de ubicación.
        var modelo = new Types.Maps.Models.PlacePoint()
        {
            ID = 0,
            Latitude = latitude,
            Longitude = longitude,
            Time = DateTime.UtcNow,
            Profile = new()
            {
                ID = profile.Model.ID,
                PlacesPoint = []
            }
        };

        // Crear el punto.
        var result = await Data.Points.Create(modelo);

        return result;
    }




    [HttpGet("point")]
    public async Task<HttpReadAllResponse<LIN.Types.Maps.Models.PlacePoint>> Get([FromQuery] string token)
    {

        // Login 
        var login = await Access.Auth.Controllers.Authentication.Login(token);

        // SI no se acepto
        if (login.Response != Responses.Success)
        {
            return new(Responses.Unauthorized);
        }


        var profile = await Data.Profiles.ReadByAccount(login.Model.Id);


        if (profile.Response != Responses.Success)
        {
            return new(Responses.Unauthorized);
        }



        var result = await Data.Points.ReadAll(profile.Model.ID);




        return new ReadAllResponse<LIN.Types.Maps.Models.PlacePoint>(Responses.Success)
        {
            Models = result.Models,
            Response = Responses.Success
        };


    }


}