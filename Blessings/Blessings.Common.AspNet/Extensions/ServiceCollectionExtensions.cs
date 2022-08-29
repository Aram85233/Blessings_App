using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Blessings.Common.AspNet.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddAuthenticationLayer(this IServiceCollection services, byte[] secret)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "bearer";
            }).AddJwtBearer("bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddSwaggerLayer(this IServiceCollection services)
        {
            services.AddSwaggerGen(
               options =>
               {
                   options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                   {
                       Name = "Authorization",
                       Type = SecuritySchemeType.ApiKey,
                       BearerFormat = "JWT",
                       In = ParameterLocation.Header,
                       Description = "JWT Authorization header using the Bearer scheme."
                   });
                   options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                   {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                   });
               });

            return services;
        }
    }
}
