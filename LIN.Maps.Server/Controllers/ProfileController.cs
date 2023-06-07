using Http.ResponsesList;

namespace LIN.Maps.Server.Controllers;


[Route("api/profile")]
public class ProfileController : ControllerBase
{


    [HttpPost("point")]
    public async Task<HttpCreateResponse> Create([FromQuery] double longitude, [FromQuery] double latitude, [FromQuery] string token)
    {

        // Login 
        var login = await LIN.Access.Auth.Controllers.Authentication.Login(token);

        // SI no se acepto
        if (login.Response != Responses.Success)
        {
            return new(Responses.Unauthorized);
        }


        var profile = await Data.Profiles.ReadByAccount(login.Model.ID);


        if (profile.Response == Responses.Success)
        {
        }
        else if (profile.Response == Responses.NotExistProfile)
        {
            var profileModel = new LIN.Types.Maps.Models.ProfileModel()
            {
                AccountID = login.Model.ID,
                ID = 0,
                PlacesPoint = new()
            };

            var profCreate = await Data.Profiles.Create(profileModel);

            if (profCreate.Response != Responses.Success)
            {
                return new(Responses.Unauthorized);
            }

            profile.Model = new()
            {
                ID = profCreate.LastID
            };
        }
        else
        {
            return new(Responses.Unauthorized);
        }

        var modelo = new LIN.Types.Maps.Models.PlacePoint()
        {
            ID = 0,
            Latitude = latitude,
            Longitude = longitude,
            Time = DateTime.Now,
            Profile = new() { ID = profile.Model.ID }
        };


        _ = Data.Points.Create(modelo);




        return new(Responses.Success);


    }




    [HttpGet("point")]
    public async Task<HttpReadAllResponse<LIN.Types.Maps.Models.PlacePoint>> Get([FromQuery] string token)
    {

        // Login 
        var login = await LIN.Access.Auth.Controllers.Authentication.Login(token);

        // SI no se acepto
        if (login.Response != Responses.Success)
        {
            return new(Responses.Unauthorized);
        }


        var profile = await Data.Profiles.ReadByAccount(login.Model.ID);


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