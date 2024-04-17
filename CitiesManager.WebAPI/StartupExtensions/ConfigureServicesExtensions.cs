using System.Text;
using CitiesManager.Core.Config;
using CitiesManager.Core.Domain.Identity;
using CitiesManager.Core.Domain.RepositoryContracts;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Core.Services;
using CitiesManager.Infrastucture.DataBaseContext;
using CitiesManager.Infrastucture.Repositories;
using CitiesManager.WebAPI.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CitiesManager.WebAPI.StartupExtensions;

public static class ConfigureServicesExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(new ProducesAttribute("application/json"));
            options.Filters.Add(new ConsumesAttribute("application/json"));
            options.Filters.Add(new ModelValidationFilter());

            // Policy
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        }).AddXmlSerializerFormatters();

        services.AddTransient<IJwtService, JwtService>();

        services.AddApiVersioning(config =>
        {
            config.ApiVersionReader = new UrlSegmentApiVersionReader();
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebAPI.xml"));
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CitiesManager.WebAPI",
                Version = "1.0"
            });

            options.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = "CitiesManager.WebAPI",
                Version = "2.0"
            });
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policyBuilder =>
            {
                policyBuilder.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!);
                policyBuilder.WithHeaders("Authorization", "origin", "content-type", "accept");
                policyBuilder.WithMethods("GET", "POST", "PUT", "DELETE");
                // builder.WithOrigins("*");
            });

            options.AddPolicy("4100Client", policyBuilder =>
            {
                policyBuilder.WithOrigins(configuration.GetSection("AllowedOrigins2").Get<string[]>()!);
                policyBuilder.WithHeaders("Authorization", "origin", "accept");
                policyBuilder.WithMethods("GET");
            });
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

// Jwt
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

        services.AddAutoMapper(typeof(MapperConfig));

        services.AddAuthorization();

        services.AddScoped<ICitiesRepository, CitiesRepository>();
        services.AddScoped<ICitiesGetterService, CitiesGetterService>();
        services.AddScoped<ICitiesAdderService, CitiesAdderService>();
        services.AddScoped<ICitiesUpdaterService, CitiesUpdaterService>();
        services.AddScoped<ICitiesDeleterService, CitiesDeleterService>();

        return services;
    }
}