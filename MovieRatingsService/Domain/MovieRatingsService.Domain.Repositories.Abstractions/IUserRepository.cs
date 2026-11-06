using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions.Base;

namespace MovieRatingsService.Domain.Repositories.Abstractions;

public interface IUserRepository : IRepository<User, Guid>
{
    // Имя пользователя уникально
    Task<IEnumerable<User>> SearchByUsernameAsync(string username, CancellationToken cancellationToken);
}