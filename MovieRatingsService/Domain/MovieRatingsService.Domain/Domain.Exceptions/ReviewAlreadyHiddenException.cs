using MovieRatingsService.Domain.Domain.Entities;

namespace Domain.ValueObject.Exceptions
{

    // Попытка скрыть уже скрытую рецензию
    public class ReviewAlreadyHiddenException(Review review)
            : DomainOperationException(
                $"Рецензия (id={review.Id}) пользователя {review.User.Username.Value} на фильм \"{review.Movie.Title.Value}\" уже скрыта.")
    {
        public Review Review => review;
    }
}
