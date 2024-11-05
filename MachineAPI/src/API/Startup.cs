using Microsoft.EntityFrameworkCore;
using UserAuth.API.Extensions;
using UserAuth.Application.Interfaces;
using UserAuth.Application.Services;
using UserAuth.Domain.Interfaces;
using UserAuth.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserAuth.Infrastructure.Repositories;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"))
        );
 
        // Add services to the container.
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { 
                Title = "Machines API", 
                Version = "v1" });
        });  

        services.AddAuthentication();
        services.AddAuthorization();

        services.AddSingleton<IConfiguration>(_configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment() || env.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Machine API V1"); // Set up the Swagger UI
                c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
            });
            app.ApplyMigrations();
        }

        // app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
