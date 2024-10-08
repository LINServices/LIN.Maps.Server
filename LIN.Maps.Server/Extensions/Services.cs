using LIN.Maps.Server.Services;

namespace LIN.Maps.Server.Extensions;

public static class Services
{

    /// <summary>
    /// Agregar LIN Services.
    /// </summary>
    public static IServiceCollection AddMapBox(this IServiceCollection services, string apiKey)
    {
        MapboxService.ApiKey = apiKey;
        services.AddSingleton<MapboxService, MapboxService>();
        return services;
    }

}