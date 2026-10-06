using MovieRatingsService.Domain.Domain.Entities;

namespace Domain.ValueObject.Exceptions
{
    // Попытка восстановить рецензию, которая не скрыта
    public class ReviewNotHiddenException(Review review)
        : DomainOperationException(
            $"Рецензия (id={review.Id}) пользователя {review.User.Username.Value} на фильм \"{review.Movie.Title.Value}\" не скрыта.")
    {
        public Review Review => review;
    }
}
