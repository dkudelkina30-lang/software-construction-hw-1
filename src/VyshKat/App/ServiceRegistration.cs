using Microsoft.Extensions.DependencyInjection;
using VyshKat.Services;

namespace VyshKat.App;

public static class ServiceRegistration
{
    public static IServiceCollection AddVyshKat(this IServiceCollection services, ConsoleIO io)
    {
        services.AddSingleton<IServiceCenter, ServiceCenter>();
        services.AddSingleton<FleetService>();
        services.AddSingleton(io);
        services.AddSingleton<ConsoleApp>();
        return services;
    }
}
