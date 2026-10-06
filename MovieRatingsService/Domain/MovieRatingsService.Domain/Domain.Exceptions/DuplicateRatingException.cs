using MovieRatingsService.Domain.Domain.Entities;

namespace Domain.ValueObject.Exceptions
{
    // Попытка повторной оценки фильма тем же пользователем
    public class DuplicateRatingException(User user, Movie movie)
        : DomainOperationException(
            $"Пользователь {user.Username.Value} (id={user.Id}) уже оценил фильм \"{movie.Title.Value}\" (id={movie.Id}).")
    {
        public User User => user;
        public Movie Movie => movie;
    }
}
