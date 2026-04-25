using MovieRatingsService.Domain.Domain.Entities;

namespace Domain.ValueObject.Exceptions
{
    // Попытка удалить фильм, у которого есть активные оценки или рецензии
    public class MovieHasDependenciesException(Movie movie)
        : DomainOperationException(
            $"Невозможно удалить фильм \"{movie.Title.Value}\" (id={movie.Id}): у него есть активные оценки или рецензии.")
    {
        public Movie Movie => movie;
    }
}
