using MovieRatingsService.Domain.Domain.Entities;

namespace Domain.ValueObject.Exceptions
{
    // Фильм уже удален
    public class MovieAlreadyDeletedException(Movie movie)
        : DomainOperationException(
            $"Фильм \"{movie.Title.Value}\" (id={movie.Id}) уже удалён.")
    {
        public Movie Movie => movie;
    }
}
