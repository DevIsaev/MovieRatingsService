using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions.Base;

namespace MovieRatingsService.Domain.Repositories.Abstractions;

public interface IReviewRepository : IRepository<Review, Guid>
{
    // Получить рецензию пользователя на конкретный фильм
    Task<Review?> GetByUserAndMovieAsync(Guid userId, Guid movieId, CancellationToken cancellationToken);

    // Все активные рецензии фильма
    Task<IEnumerable<Review>> GetActiveByMovieAsync(Guid movieId, CancellationToken cancellationToken);

    // Все рецензии пользователя
    Task<IEnumerable<Review>> GetByUserAsync(Guid userId, CancellationToken cancellationToken);
}