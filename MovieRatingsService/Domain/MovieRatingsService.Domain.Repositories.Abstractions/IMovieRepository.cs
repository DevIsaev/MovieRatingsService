using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions.Base;

namespace MovieRatingsService.Domain.Repositories.Abstractions;

public interface IMovieRepository : IRepository<Movie, Guid>
{
    // Получить только не удалённые фильмы
    Task<IEnumerable<Movie>> GetActiveMoviesAsync(CancellationToken cancellationToken);

    // Поиск по названию
    Task<IEnumerable<Movie>> SearchByTitleAsync(string title, CancellationToken cancellationToken);

    // Получить фильмы, созданные конкретным администратором
    Task<IEnumerable<Movie>> GetByAdminAsync(Guid adminId, CancellationToken cancellationToken);
}