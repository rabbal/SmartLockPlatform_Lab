using SmartLockPlatform.Application;
using SmartLockPlatform.Host;
using SmartLockPlatform.Host.Controllers.V1.Mappings;
using SmartLockPlatform.Infrastructure;
using SmartLockPlatform.Infrastructure.Hosing;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddApplication();
services.AddInfra();

builder.Services.AddEndpointsApiExplorer();
services.AddHealthChecks();
services.AddControllers()
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
    });
services.AddApiVersioning();

services.AddCustomSwagger();
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

app.UseRouting();

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