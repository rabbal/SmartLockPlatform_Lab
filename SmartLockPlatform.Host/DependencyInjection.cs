using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using SmartLockPlatform.Host.Authorization;
using SmartLockPlatform.Host.Authorization.ResourceBased;

namespace SmartLockPlatform.Host;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, SiteAuthorizationHandler>();
        return services;
    }
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.FullName);

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "SmartLockPlatform.Host API Documentation",
                Description = "API Documentation",
                Contact = new OpenApiContact
                {
                    Email = "info@smartplatform.net",
                    Name = "SmartLockPlatform",
                    Url = new Uri("http://github.com/smp")
                }
            });

            c.CustomSchemaIds(s => s.FullName?.Replace("+", "."));

            c.AddSecurityDefinition("JWT",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Authorization jwt using",
                });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme,
                        },
                        Name = "JWT",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }
}