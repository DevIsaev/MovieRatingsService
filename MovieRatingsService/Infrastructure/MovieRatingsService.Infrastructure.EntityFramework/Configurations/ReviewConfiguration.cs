using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.ValueObject;
using Domain.ValueObject.Validators;
using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Domain.Enums;

namespace MovieRatingsService.Infrastructure.EntityFramework.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        // Конфигурация сущности Review
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            // Первичный ключ
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            // Content
            builder.Property(x => x.Content)
                .IsRequired()
                .HasConversion(
                    content => content.Value,
                    str => new ReviewContent(str))
                .HasMaxLength(ReviewContentValidator.MaxLength);

            // Status
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

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

            // HiddenAt
            builder.Property(x => x.HiddenAt)
                .IsRequired(false)
                .HasConversion(
                    src => !src.HasValue ? src : src.Value.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src.Value, DateTimeKind.Utc),
                    dst => !dst.HasValue ? dst : dst.Value.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst.Value, DateTimeKind.Utc));

            // Связь с пользователем
            builder.HasOne(x => x.User)
                .WithMany("Reviews")
                .HasForeignKey("UserId");

            // Связь с фильмом
            builder.HasOne(x => x.Movie)
                .WithMany("Reviews")
                .HasForeignKey("MovieId");

            // Связь с администратором, скрывшим рецензию
            builder.HasOne(x => x.HiddenByAdmin)
                .WithMany()
                .HasForeignKey("HiddenByAdminId")
                .IsRequired(false);

            // Один пользователь может оставить только одну активную рецензию на фильм
            builder.HasIndex("UserId", "MovieId").IsUnique();
        }
    }
}