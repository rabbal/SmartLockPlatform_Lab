using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Infrastructure.Context;
using SmartLockPlatform.Infrastructure.Identity;
using SmartLockPlatform.Infrastructure.Interception;

namespace SmartLockPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddPooledDbContextFactory<AppDbContext>(ConfigureOptions)
            .AddDbContextPool<AppDbContext>(ConfigureOptions);

        services.AddSingleton<TrackingFieldsInterceptor>();

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddScoped<ISqlConnectionFactory>(sp =>
        {
            var connectionString =
                sp.GetRequiredService<IConfiguration>().GetConnectionString("Npgsql");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("The Npgsql hasn't been specified in appsetting.json");

            return new SqlConnectionFactory(connectionString);
        });

        services.AddScoped<IDbConnection>(sp => sp.GetRequiredService<ISqlConnectionFactory>().GetOpenConnection());

        services.AddSingleton<IUserIdentitySession, UserIdentitySession>();
        services.AddSingleton<IPasswordHashAlgorithm, PasswordHashAlgorithm>();
        services.AddHttpContextAccessor();
        
        return services;
    }

    private static void ConfigureOptions(IServiceProvider sp, DbContextOptionsBuilder builder)
    {
        // This is useful for test executions to prevent collision when running in parallel
        var databaseName = Guid.NewGuid().ToString("N"); //Must be here due to scoped lifetime of factory

        var configuration = sp.GetRequiredService<IConfiguration>();
        
        var connectionString = configuration
            .GetConnectionString("Npgsql")?.Replace("{uuid}", databaseName);

        if (!sp.GetRequiredService<IHostEnvironment>().IsProduction())
        {
            builder.ConfigureWarnings(c => c.Log((RelationalEventId.CommandExecuted, LogLevel.Debug)));
        
            // Not suitable for production
            builder.EnableSensitiveDataLogging();
        }

        builder.UseNpgsql(connectionString, options =>
        {
            options.MigrationsHistoryTable("Migration");
            var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
            options.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName);
            options.CommandTimeout(minutes);
            options.MaxBatchSize(1000);
            options.EnableRetryOnFailure();
        });

        builder.AddInterceptors(sp.GetRequiredService<TrackingFieldsInterceptor>());
    }
}