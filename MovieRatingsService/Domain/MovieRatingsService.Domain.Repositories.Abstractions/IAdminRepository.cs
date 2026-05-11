using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions.Base;

namespace MovieRatingsService.Domain.Repositories.Abstractions;

public interface IAdminRepository : IRepository<Admin, Guid>
{
    // Имя администратора уникально
    Task<Admin?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}