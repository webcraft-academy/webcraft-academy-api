namespace webcnAPI.V1.Installers;
using webcnAPI.Service;
using webcnAPI.Domain;
using webcnAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class DependencyInjectionSetup 
{
    public static IServiceCollection RegisterMvcAndCors (this IServiceCollection services) 
    {
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        
        return services;
    }

    public static IServiceCollection RegisterServices (this IServiceCollection services) 
    {
        
        services.AddScoped<NinjaService>();
        
        return services;
    }

    public static IServiceCollection RegisterRepositories (this IServiceCollection services) 
    {
        
        services.AddScoped<INinjaRepository, NinjaRepository>();
        return services;
    }

    public static IServiceCollection RegisterSwagger (this IServiceCollection services) 
    {

        services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
        {
            // Configure Swagger options
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web Craft Ninja API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
        return services;
    }

    public static IServiceCollection RegisterAuthentication (this IServiceCollection services) 
    {
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Configure JWT Bearer authentication options
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "junks",
                    ValidAudience = "junks",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"))
                };
            });
        
        return services;
    }
}