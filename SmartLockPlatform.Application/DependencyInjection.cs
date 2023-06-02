using Microsoft.Extensions.DependencyInjection;
using SmartLockPlatform.Application.Authorization;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries;

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
        services.AddScoped<ISiteQueries, SiteQueries>();
        services.AddScoped<IUserQueries, UserQueries>();
        services.AddScoped<IResourceProtector, ResourceProtector>();
        return services;
    }
}