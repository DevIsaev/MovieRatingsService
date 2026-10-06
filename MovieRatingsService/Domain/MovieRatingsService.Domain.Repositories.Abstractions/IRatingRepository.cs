using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions.Base;

namespace MovieRatingsService.Domain.Repositories.Abstractions;

public interface IRatingRepository : IRepository<Rating, Guid>
{
    // Получить активную оценку пользователя на конкретный фильм
    Task<Rating?> GetByUserAndMovieAsync(Guid userId, Guid movieId, CancellationToken cancellationToken);

    // Все активные оценки фильма
    Task<IEnumerable<Rating>> GetActiveByMovieAsync(Guid movieId, CancellationToken cancellationToken);

    // Все активные оценки пользователя
    Task<IEnumerable<Rating>> GetActiveByUserAsync(Guid userId, CancellationToken cancellationToken);
}