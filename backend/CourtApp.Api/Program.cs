using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CourtApp.Api.Interfaces.Repositories;
using CourtApp.Api.Interfaces.Services;
using FluentValidation.AspNetCore;
using CourtApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Microsoft.AspNetCore.Builder; // Added for WebApplication
using Microsoft.AspNetCore.Hosting; // Added for IWebHostEnvironment
using Microsoft.Extensions.Configuration; // Added for IConfiguration
using Microsoft.Extensions.DependencyInjection; // Added for IServiceCollection
using Microsoft.AspNetCore.Routing; // Added for IEndpointRouteBuilder
using Microsoft.AspNetCore.Diagnostics.HealthChecks; // Added for HealthChecks

namespace CourtApp.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

        var app = builder.Build();

        Configure(app, app.Environment);

        app.Run();
    }

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {

        services.AddControllers();
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddFluentValidationAutoValidation();

        if (environment.EnvironmentName != "Testing")
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }

        // Add Health Checks
        services.AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!, name: "PostgreSQL DB Check");

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CourtApp.Api", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. Example: ""Authorization: Bearer {token}""",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? ""))
                };
            });

        services.AddAuthorization();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IContactRequestRepository>(provider =>
            new ContactRequestRepository(
                provider.GetRequiredService<AppDbContext>(),
                provider.GetRequiredService<IConfiguration>(),
                provider.GetRequiredService<ILogger<ContactRequestRepository>>()));
        services.AddScoped<IContactRequestService, ContactRequestService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddAutoMapper(typeof(Program));
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        // Configure the HTTP request pipeline.
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting(); // Add UseRouting

        app.UseCors("AllowAll");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => // Use UseEndpoints
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health"); // Map Health Checks endpoint
        });
    }
}