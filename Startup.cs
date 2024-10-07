using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using UserAuthAPI.Data;
using UserAuthAPI.Services;
using UserAuthAPI.Extensions;

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


        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();


        // Add services to the container.
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "User Authentication API", Version = "v1" });
            // c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Authentication API", Version = "v1" });

            // Se você tiver um arquivo de comentários XML
            // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            // c.IncludeXmlComments(xmlPath);
        });

        // Add Authentication
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true; // Configure JWT token validation options 
            });

        // Add Authorization
        services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment() || env.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Authentication API V1"); // Set up the Swagger UI
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
