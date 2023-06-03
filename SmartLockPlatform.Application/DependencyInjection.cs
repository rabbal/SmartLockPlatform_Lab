using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Refit;
using SmartLockPlatform.Application.Authorization;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.External;
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

        var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5);
        var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
            .OrResult(response => (int)response.StatusCode == 429) // RetryAfter
            .WaitAndRetryAsync(delay);

        var circuitPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        services.AddRefitClient<ISmartMqttClient>()
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<SmartMqttClientOptions>>();

                client.BaseAddress = new Uri("http://localhost:5298");
                client.DefaultRequestHeaders.Add("User-Agent", "SmartLockPlatform");
            })
            .AddPolicyHandler(circuitPolicy)
            .AddPolicyHandler(retryPolicy);

        return services;
    }
}