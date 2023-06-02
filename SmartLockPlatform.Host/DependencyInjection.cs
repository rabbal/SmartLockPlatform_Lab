using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartLockPlatform.Host.Authentication;
using SmartLockPlatform.Host.Authorization;
using SmartLockPlatform.Host.Authorization.ResourceBased;

namespace SmartLockPlatform.Host;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
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
        return services;
    }

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(config =>
        {
            config.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, SiteAuthorizationHandler>();
        return services;
    }

    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var authentication = configuration.GetSection("Authentication");
        services.Configure<TokenOptions>(options => authentication.Bind(options));

        var signingKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authentication[nameof(TokenOptions.SigningKey)]));

        var tokenValidationParameters = new TokenValidationParameters
        {
            // Ensure the token was issued by a trusted authorization server (default true):
            ValidIssuer = authentication[nameof(TokenOptions.Issuer)], // site that make the token
            ValidateIssuer = true,

            // Ensure the token audience matches our audience value (default true):
            ValidAudience =
                authentication[nameof(TokenOptions.Audience)], // site that consumes the token
            ValidateAudience = true,

            RequireSignedTokens = true,
            IssuerSigningKey = signingKey,
            ValidateIssuerSigningKey = true, // verify signature to avoid tampering
            // Ensure the token hasn't expired:
            ValidateLifetime = true,
            RequireExpirationTime = true,

            // tolerance for the expiration date (compensates for server time drift).
            // We recommend 5 minutes or less:
            ClockSkew = TimeSpan.Zero
        };

        services
            .AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
                options.ClaimsIssuer = authentication[nameof(TokenOptions.Issuer)];
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                            .CreateLogger(nameof(JwtBearerEvents));
                        logger.LogError(context.Exception, "Authentication Failed");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        // var validator = context.HttpContext.RequestServices.GetRequiredService<ITokenValidator>();
                        // await validator.ValidateAsync(context);
                        //context.Principal.AddIdentity(new ClaimsIdentity());
                    },
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                            .CreateLogger(nameof(JwtBearerEvents));
                        logger.LogError("OnChallenge Error [{Error}]: [{ErrorDescription}]", context.Error,
                            context.ErrorDescription);
                        return Task.CompletedTask;
                    }
                };
            });

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