using Mapster;

namespace Birahe.EndPoint.Mapster;

public static class MapsterConfigs {
    public static IServiceCollection AddMapsterConfigs(this IServiceCollection services)
    {
        services.AddMapster();
        UserConfigs.AddConfigs();


        return services;
    }


}