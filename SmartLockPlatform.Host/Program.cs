using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SmartLockPlatform.Application;
using SmartLockPlatform.Host;
using SmartLockPlatform.Host.Controllers.V1.Mappings;
using SmartLockPlatform.Infrastructure;
using SmartLockPlatform.Infrastructure.Hosing;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    builder.Host.UseSerilog();
    var services = builder.Services;

    services.AddProblemDetails(setup =>
    {
        setup.IncludeExceptionDetails =
            (_, _) => builder.Environment.IsDevelopment() || builder.Environment.IsStaging();
    });
    services.AddApplication();
    services.AddInfra();
    services.AddCustomAuthorization();
    services.AddCustomAuthentication(builder.Configuration);
    services.AddCustomSwagger();
    services.AddCustomControllers();
    services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });

    services.AddHealthChecks();
    services.AddAutoMapper(expression => { expression.AddProfile<MappingProfile>(); });
    var app = builder.Build();

    app.UseProblemDetails();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1"); });
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

//TODO: Utilize EFCore migration bundles and get rid of it
    if (!app.Environment.IsEnvironment("Testing"))
    {
        using var scope = app.Services.CreateScope();
        await scope.ServiceProvider.MigrateAppDbContext();
    }

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
{
}