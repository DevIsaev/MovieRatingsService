using MovieRatingsService.Domain.Domain.Entities;

namespace Domain.ValueObject.Exceptions
{
    // Оценка уже находится среди удалённых
    public class RatingAlreadyInDeletedListException(Rating rating)
        : DomainOperationException(
            $"Оценка (id={rating.Id}) пользователя {rating.User.Username.Value} уже присутствует в списке удалённых.")
    {
        public Rating Rating => rating;
    }
}
