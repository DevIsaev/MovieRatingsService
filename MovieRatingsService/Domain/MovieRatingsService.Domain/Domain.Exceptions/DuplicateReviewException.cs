using MovieRatingsService.Domain.Domain.Entities;

namespace Domain.ValueObject.Exceptions
{
    // Попытка повторного написания рецензии на тот же фильм
    public class DuplicateReviewException(User user, Movie movie)
        : DomainOperationException(
            $"Пользователь {user.Username.Value} (id={user.Id}) уже оставил рецензию на фильм \"{movie.Title.Value}\" (id={movie.Id}).")
    {
        public User User => user;
        public Movie Movie => movie;
    }
}
