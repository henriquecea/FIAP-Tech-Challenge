using Elastic.Clients.Elasticsearch;
using FCG.Domain.Configuration;
using FCG.Domain.Interface.Client;
using FCG.Domain.Interface.Settings;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Settings.Elastic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FCG.WebAPI.Extension;

public static class BuilderConfiguration
{
    public static void AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    }

    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

        var key = Encoding.ASCII.GetBytes(jwtSettings!.SecretKey);

        builder.Services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                }
            );
        builder.Services.AddAuthorization();
    }

    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fiap Tech Challenge", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }

    public static void AddElasticSearch(this WebApplicationBuilder builder)
    {
        // Bind do appsettings
        builder.Services.Configure<ElasticSettings>(
            builder.Configuration.GetSection("ElasticSettings")
        );

        // Interface para settings
        builder.Services.AddSingleton<IElasticSettings>(sp =>
            sp.GetRequiredService<IOptions<ElasticSettings>>().Value
        );

        // ElasticsearchClient (singleton)
        builder.Services.AddSingleton(sp =>
        {
            var settings = sp
                .GetRequiredService<IOptions<ElasticSettings>>()
                .Value;

            var clientSettings = new ElasticsearchClientSettings(
                new Uri(settings.Uri)
            );

            return new ElasticsearchClient(clientSettings);
        });

        builder.Services.AddSingleton(typeof(IElasticClient<>), typeof(ElasticClient<>));
    }
}
