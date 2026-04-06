namespace Domain.ValueObject.Exceptions
{
    // Попытка повторной оценки фильма тем же пользователем
    public class DuplicateRatingException : DomainOperationException
    {
        public DuplicateRatingException(Guid userId, Guid movieId)
            : base($"Пользователь {userId} уже оценил фильм {movieId}.") { }
    }

    // Попытка повторного написания рецензии на тот же фильм
    public class DuplicateReviewException : DomainOperationException
    {
        public DuplicateReviewException(Guid userId, Guid movieId)
            : base($"Пользователь {userId} уже оставил рецензию на фильм {movieId}.") { }
    }

    // Попытка скрыть уже скрытую рецензию
    public class ReviewAlreadyHiddenException : DomainOperationException
    {
        public ReviewAlreadyHiddenException(Guid reviewId)
            : base($"Рецензия {reviewId} уже скрыта.") { }
    }

    // Попытка удалить уже удалённую оценку
    public class RatingAlreadyDeletedException : DomainOperationException
    {
        public RatingAlreadyDeletedException(Guid ratingId)
            : base($"Оценка {ratingId} уже удалена.") { }
    }

    // Некорректные данные фильма
    public class InvalidMovieDataException : DomainException
    {
        public InvalidMovieDataException(string details) : base(details) { }
    }

    // Операция запрещена из-за недостатка прав
    public class UnauthorizedOperationException : DomainOperationException
    {
        public UnauthorizedOperationException(string message) : base(message) { }
    }

    // Сущность не найдена в репозитории
    public class EntityNotFoundException : DomainException
    {
        public EntityNotFoundException(string entityName, object id) : base($"{entityName} с идентификатором {id} не найден.") { }
    }

    // Попытка удалить фильм, у которого есть оценки или рецензии.
    public class MovieHasDependenciesException : DomainOperationException
    {
        public MovieHasDependenciesException(Guid movieId)
            : base($"Невозможно удалить фильм {movieId}, так как у него есть оценки или рецензии.") { }
    }
}
