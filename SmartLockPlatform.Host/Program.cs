using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using SmartLockPlatform.Application;
using SmartLockPlatform.Host;
using SmartLockPlatform.Host.Authentication;
using SmartLockPlatform.Host.Controllers.V1.Mappings;
using SmartLockPlatform.Infrastructure;
using SmartLockPlatform.Infrastructure.Hosing;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddApplication();
services.AddInfra();

var authentication = builder.Configuration.GetSection("Authentication");
services.Configure<TokenOptions>(options => authentication.Bind(options));

builder.Services.AddEndpointsApiExplorer();
services.AddHealthChecks();
services.AddControllers(options =>
    {
        options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
        options.OutputFormatters.Insert(0, new HttpNoContentOutputFormatter
        {
            TreatNullValueAsNoContent = false
        });

        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;

        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    })
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();

        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    });

services.AddApiVersioning();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.AddAuthorization(config =>
{
    config.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
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