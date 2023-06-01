using Microsoft.Extensions.DependencyInjection;
using SmartLockPlatform.Application.Base;

namespace SmartLockPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IDateTime, SystemDateTime>();
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
        });
        return services;
    }
}