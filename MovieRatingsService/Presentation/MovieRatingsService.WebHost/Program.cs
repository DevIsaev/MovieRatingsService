using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using MovieRatingsService.Application.Service;
using MovieRatingsService.Application.Service.Abstractions;
using MovieRatingsService.Application.Service.Mapping;
using MovieRatingsService.Domain.Repositories.Abstractions;
using MovieRatingsService.Infrastructure.EntityFramework;
using MovieRatingsService.Infrastructure.EntityFramework.RepositoriesEF;
using MovieRatingsService.Infrastructure.RepositoriesEF;
using MovieRatingsService.WebHost.Filters;
using MovieRatingsService.WebHost.Helpers;
using MovieRatingsService.WebHost.Mapping;

namespace MovieRatingsService.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString(nameof(ApplicationDbContext));
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string for ApplicationDbContext is not configured.");


            builder.Services.AddNpgsql<ApplicationDbContext>(connectionString, options =>
            {
                options.MigrationsAssembly("MovieRatingsService.Infrastructure.EntityFramework");
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Movie ratings service API",
                    Description = "API for movies, ratings and reviews."
                });
            });

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ApplicationProfile>();
                cfg.AddProfile<PresentationProfile>();
            });

            builder.Services.AddScoped<IUserRepository, EfUserRepository>();
            builder.Services.AddScoped<IUsersApplicationService, UsersApplicationService>();

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddControllers(o => o.Filters.Add<ApiExceptionFilter>());
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();

            app.MigrateDatabase<ApplicationDbContext>();

            app.Run();
        }
    }
}