using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartLockPlatform.Infrastructure.Context;

namespace SmartLockPlatform.Infrastructure.Hosing;

public static class HostExtensions
{
    public static void InitializeDb(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();
    }
}