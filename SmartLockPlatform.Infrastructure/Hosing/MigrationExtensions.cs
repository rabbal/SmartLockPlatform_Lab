using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using SmartLockPlatform.Infrastructure.Context;

namespace SmartLockPlatform.Infrastructure.Hosing;

public static class MigrationExtensions
{
    public static Task MigrateAppDbContext(this IServiceProvider sp, Action<IServiceProvider>? postMigration = null)
    {
        return sp.MigrateDbContext<AppDbContext>(postMigration);
    }

    private static async Task MigrateDbContext<TContext>(this IServiceProvider sp,
        Action<IServiceProvider>? postMigration = null)
        where TContext : DbContext
    {
        using var scope = sp.CreateScope();
        var provider = scope.ServiceProvider;

        if (provider.GetRequiredService<IHostEnvironment>().IsEnvironment("Testing"))
        {
            return;
        }

        var logger = provider.GetRequiredService<ILogger<TContext>>();
        var contextFactory = provider.GetRequiredService<IDbContextFactory<TContext>>();

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

            var retry = Policy.Handle<Exception>().WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(15),
                },
                (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogWarning(exception,
                        "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                        nameof(TContext), exception.GetType().Name, exception.Message, retry, 3);
                });

            await retry.ExecuteAsync(async () =>
            {
                await using var context = await contextFactory.CreateDbContextAsync();

                await context.Database.MigrateAsync();

                postMigration?.Invoke(provider);
            });

            logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
        }
    }
}