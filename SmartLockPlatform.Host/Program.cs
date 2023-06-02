using Microsoft.AspNetCore.Mvc;
using SmartLockPlatform.Application;
using SmartLockPlatform.Host;
using SmartLockPlatform.Host.Controllers.V1.Mappings;
using SmartLockPlatform.Infrastructure;
using SmartLockPlatform.Infrastructure.Hosing;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

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
services.AddAutoMapper(expression =>
{
    expression.AddProfile<MappingProfile>();
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1"); });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.MigrateAppDbContext();
}

await app.RunAsync();

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
{
}