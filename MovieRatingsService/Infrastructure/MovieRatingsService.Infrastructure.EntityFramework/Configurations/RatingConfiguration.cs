using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.ValueObject;
using Domain.ValueObject.Validators;
using MovieRatingsService.Domain.Domain.Entities;

namespace MovieRatingsService.Infrastructure.EntityFramework.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        // Конфигурация сущности Rating
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            // Первичный ключ
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            // Score
            builder.Property(x => x.Score)
                .IsRequired()
                .HasConversion(
                    score => score.Value,
                    val => new Score(val));

            // CreatedAt
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasConversion(
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc));

            // UpdatedAt
            builder.Property(x => x.UpdatedAt)
                .IsRequired(false)
                .HasConversion(
                    src => !src.HasValue ? src : src.Value.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src.Value, DateTimeKind.Utc),
                    dst => !dst.HasValue ? dst : dst.Value.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst.Value, DateTimeKind.Utc));

            // DeletedAt
            builder.Property(x => x.DeletedAt)
                .IsRequired(false)
                .HasConversion(
                    src => !src.HasValue ? src : src.Value.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src.Value, DateTimeKind.Utc),
                    dst => !dst.HasValue ? dst : dst.Value.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst.Value, DateTimeKind.Utc));

            // Связь с пользователем
            builder.HasOne(x => x.User)
                .WithMany("Ratings")
                .HasForeignKey("UserId");

            // Связь с фильмом
            builder.HasOne(x => x.Movie)
                .WithMany("Ratings")
                .HasForeignKey("MovieId");

            // Связь с администратором, удалившим оценку
            builder.HasOne(x => x.DeletedByAdmin)
                .WithMany("DeletedRatings")
                .HasForeignKey("DeletedByAdminId")
                .IsRequired(false);

            // Один пользователь может иметь только одну активную оценку на фильм
            builder.HasIndex("UserId", "MovieId").IsUnique();
        }
    }
}
