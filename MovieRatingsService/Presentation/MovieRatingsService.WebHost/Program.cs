
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieRatingsService.Infrastructure.EntityFramework;
using MovieRatingsService.WebHost.Helpers;

namespace MovieRatingsService.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString(nameof(ApplicationDbContext));

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string for NotesServiceDbContext is not configured.");
            }

            builder.Services.AddNpgsql<ApplicationDbContext>(connectionString, options =>
            {
                options.MigrationsAssembly("MovieRatingsService.Infrastructure.EntityFramework");

            });

            builder.Services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Movies service API",
                        Description = ""
                    });
                });

            builder.Services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseNpgsql(connectionString);
                });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MigrateDatabase<ApplicationDbContext>();

            app.Run();
        }
    }
}
