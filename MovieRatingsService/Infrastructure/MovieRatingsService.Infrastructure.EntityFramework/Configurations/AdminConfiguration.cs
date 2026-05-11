using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.ValueObject;
using Domain.ValueObject.Validators;
using MovieRatingsService.Domain.Domain.Entities;

namespace MovieRatingsService.Infrastructure.EntityFramework.Configurations
{
    // Конфигурация сущности Admin
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
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

            // Permissions
            builder.Property(x => x.Permissions)
                .IsRequired()
                .HasConversion(
                    perms => perms.Value,
                    str => new Permissions(str));

            // Связь 1 ко многим: Admin -> Movie
            builder.HasMany<Movie>("CreatedMovies")
                .WithOne(m => m.CreatedByAdmin)
                // внешний ключ в Movie
                .HasForeignKey("CreatedByAdminId")
                .HasPrincipalKey(x => x.Id);

            // Связь 1 ко многим: Admin -> Rating
            builder.HasMany<Rating>("DeletedRatings")
                .WithOne(r => r.DeletedByAdmin)
                .HasForeignKey("DeletedByAdminId")
                .HasPrincipalKey(x => x.Id)
                // внешний ключ может быть NULL
                .IsRequired(false);

            // Игнорирование навигационных коллекций только для чтения
            builder.Ignore(x => x.createdMovies);
            builder.Ignore(x => x.deletedRatings);
        }
    }
}