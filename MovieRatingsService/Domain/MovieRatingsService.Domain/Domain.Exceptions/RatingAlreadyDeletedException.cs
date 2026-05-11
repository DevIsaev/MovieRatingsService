using MovieRatingsService.Domain.Domain.Entities;

namespace Domain.ValueObject.Exceptions
{
    // Попытка удалить уже удалённую оценку
    public class RatingAlreadyDeletedException(Rating rating)
        : DomainOperationException(
            $"Оценка (id={rating.Id}) пользователя {rating.User.Username.Value} на фильм \"{rating.Movie.Title.Value}\" уже удалена.")
    {
        public Rating Rating => rating;
    }
}
