using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.ValueObject;
using Domain.ValueObject.Validators;
using MovieRatingsService.Domain.Domain.Entities;

namespace MovieRatingsService.Infrastructure.EntityFramework.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        // Конфигурация сущности User
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Первичный ключ
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            // Username
            builder.Property(x => x.Username)
                .IsRequired()
                .HasConversion(
                    username => username.Value,
                    str => new Username(str))
                .HasMaxLength(UsernameValidator.MaxLength);

            // Связь с оценками
            builder.HasMany<Rating>("Ratings")
                .WithOne(r => r.User)
                .HasForeignKey("UserId")
                .HasPrincipalKey(x => x.Id);

            // Связь с рецензиями
            builder.HasMany<Review>("Reviews")
                .WithOne(r => r.User)
                .HasForeignKey("UserId")
                .HasPrincipalKey(x => x.Id);
        }
    }
}