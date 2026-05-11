using MovieRatingsService.Domain.Domain.Entities;

namespace Domain.ValueObject.Exceptions
{
    // Фильм не найден в списке фильмов администратора
    public class MovieNotOwnedByAdminException(Admin admin, Movie movie)
        : DomainOperationException(
            $"Администратор {admin.Username.Value} (id={admin.Id}) не является создателем фильма \"{movie.Title.Value}\" (id={movie.Id}).")
    {
        public Admin Admin => admin;
        public Movie Movie => movie;
    }
}
