using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.ValueObject;
using Domain.ValueObject.Validators;
using MovieRatingsService.Domain.Domain.Entities;

namespace MovieRatingsService.Infrastructure.EntityFramework.Configurations
{
    // Конфигурация сущности Movie
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            // Первичный ключ
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            // Title
            builder.Property(x => x.Title)
                .IsRequired()
                .HasConversion(
                    title => title.Value,
                    str => new Title(str))
                .HasMaxLength(TitleValidator.MaxLength);

            // Description
            builder.Property(x => x.Description)
                .IsRequired(false)
                .HasConversion(
                    desc => desc!.Value,
                    str => new Description(str))
                .HasMaxLength(DescriptionValidator.MaxLength);

            // ReleaseYear 
            builder.Property(x => x.ReleaseYear)
                .IsRequired(false)
                .HasConversion(
                    ry => (int?)ry!.Value,
                    val => val.HasValue ? new ReleaseYear(val.Value) : null);

            // Genre
            builder.Property(x => x.Genre)
                .IsRequired(false)
                .HasConversion(
                    genre => genre!.Value,
                    str => new Genre(str))
                .HasMaxLength(GenreValidator.MaxLength);

            // PosterUrl
            builder.Property(x => x.PosterUrl)
                .IsRequired(false)
                .HasConversion(
                    url => url!.Value,
                    str => new PosterUrl(str))
                .HasMaxLength(PosterUrlValidator.MaxLength);

            // CreatedAt
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasConversion(
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc));

            builder.Property(x => x.IsDeleted).IsRequired();

            // Связь с администратором
            builder.HasOne(x => x.CreatedByAdmin)
                .WithMany("CreatedMovies")
                .HasForeignKey("CreatedByAdminId");

            // Связь с оценками
            builder.HasMany<Rating>("Ratings")
                .WithOne(r => r.Movie)
                .HasForeignKey("MovieId")
                .HasPrincipalKey(x => x.Id);

            // Связь с рецензиями
            builder.HasMany<Review>("Reviews")
                .WithOne(r => r.Movie)
                .HasForeignKey("MovieId")
                .HasPrincipalKey(x => x.Id);
        }
    }
}