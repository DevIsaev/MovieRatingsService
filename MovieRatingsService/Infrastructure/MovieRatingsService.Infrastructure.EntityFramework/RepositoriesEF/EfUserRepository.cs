using Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions;
using MovieRatingsService.Infrastructure.EntityFramework;
using MovieRatingsService.Infrastructure.RepositoriesEF;

namespace MovieRatingsService.Infrastructure.EntityFramework.RepositoriesEF
{
    public class EfUserRepository(ApplicationDbContext context)
            : EfRepository<User, Guid>(context), IUserRepository
    {
        private readonly DbSet<User> _users = context.Set<User>();

        public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
            => _users.FirstOrDefaultAsync(u => u.Username.Equals(new Username(username)), cancellationToken);

        public async Task<IEnumerable<User>> SearchByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var users = await _users.AsNoTracking().ToListAsync(cancellationToken);

            return users
                .Where(u => u.Username.Value.Contains(username, StringComparison.OrdinalIgnoreCase))
                .OrderBy(u => u.Username.Value)
                .ToList();
        }
    }
}
