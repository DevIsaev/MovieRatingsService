using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions.Base;

namespace MovieRatingsService.Domain.Repositories.Abstractions;

public interface IUserRepository : IRepository<User, Guid>
{
    // Имя пользователя уникально
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}